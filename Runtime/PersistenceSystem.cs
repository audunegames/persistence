using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Audune.Persistence
{
  // Class that defines the system for persistent data
  [AddComponentMenu("Audune Persistence/Persistence System")]
  public sealed class PersistenceSystem : MonoBehaviour
  {
    // Enum that defines the backend type of the persistence system
    public enum BackendType
    {
      [InspectorName("MessagePack")]
      MessagePack
    }


    // Persistence system properties
    [SerializeField, Tooltip("The type of the backend to use")]
    private BackendType _backendType = BackendType.MessagePack;

    // Internal state of the persistence system
    private List<IAdapter> _adapters = new List<IAdapter>();
    private IBackend _backend;

    // Persistence system events
    public event Action<File> OnFileRead;
    public event Action<File> OnFileWritten;
    public event Action<File, File> OnFileMoved;
    public event Action<File, File> OnFileCopied;
    public event Action<File> OnFileDeleted;


    // Awake is called when the script instance is being loaded
    private void Awake()
    {
      // Initialize the backend
      _backend = _backendType switch {
        BackendType.MessagePack => new MessagePackBackend(),
        _ => throw new PersistenceException($"Unknown backend type {_backendType}"),
      };
    }


    #region Adapter management
    // Return all registered adapters
    public IEnumerable<IAdapter> GetAdapters()
    {
      return _adapters;
    }

    // Return all enabled registered adapters
    public IEnumerable<IAdapter> GetEnabledAdapters()
    {
      return _adapters.Where(adapter => adapter.adapterEnabled);
    }

    // Return if an adapter with the specified name exists
    public bool TryGetAdapter(string name, out IAdapter adapter)
    {
      adapter = _adapters.Find(adapter => adapter.adapterName == name);
      return adapter != null;
    }

    // Return the adapter with the specified name
    public IAdapter GetAdapter(string name)
    {
      if (TryGetAdapter(name, out IAdapter adapter))
        return adapter;
      else
        throw new PersistenceException($"Could not find a registered adapter with name {name}");
    }

    // Return if there is a first adapter that is enabled
    public bool TryGetFirstEnabledAdapter(out IAdapter adapter)
    {
      adapter = GetEnabledAdapters().FirstOrDefault();
      return adapter != null;
    }

    // Return the first adapter that is enabled
    public IAdapter GetFirstEnabledAdapter()
    {
      if (TryGetFirstEnabledAdapter(out IAdapter adapter))
        return adapter;
      else
        throw new PersistenceException("Could not find an accessible registered adapter");
    }

    // Register an adapter
    public void RegisterAdapter(IAdapter adapter)
    {
      _adapters.Add(adapter);
    }

    // Unregister an adapter
    public void UnregisterAdapter(IAdapter adapter)
    {
      _adapters.Remove(adapter);
    }
    #endregion

    // List the available files
    public IEnumerable<File> List(Predicate<string> predicate = null)
    {
      return _adapters.SelectMany(adapter => adapter.List(predicate).Select(path => adapter.GetFile(path)));
    }

    // Return if the specified file exists
    public bool Exists(File file)
    {
      return file.Exists();
    }

    // Read a state from the specified file
    public TState Read<TState>(File file) where TState : State
    {
      // Read the data
      var data = file.Read();

      // Deserialize the state
      var state = _backend.Deserialize(data);

      // Check if the state is the correct type
      if (state is not TState expectedState)
        throw new PersistenceException($"Could not cast the state: expected state of type {typeof(TState)} but got {state.GetType()}");

      // Invoke the read event
      OnFileRead?.Invoke(file);

      // Return the state
      return expectedState;
    }

    // Read a deserializable object from the specified file into an existing object
    public void Read<TState, TDeserializable>(File file, TDeserializable deserializable) where TState : State where TDeserializable : IDeserializable<TState>
    {
      var state = Read<TState>(file);
      deserializable.Deserialize(state);
    }

    // Read a deserializable object from the specified file into an existing object with the provided context
    public void Read<TState, TContext, TDeserializable>(File file, TDeserializable deserializable, TContext context) where TState : State where TDeserializable : IDeserializable<TState, TContext>
    {
      var state = Read<TState>(file);
      deserializable.Deserialize(state, context);
    }

    // Write the specified state to the specified file
    public void Write(File file, State state)
    {
      // Serialize the state
      var data = _backend.Serialize(state);

      // Write the data
      file.Write(data);

      // Invoke the written event
      OnFileWritten?.Invoke(file);
    }

    // Write the specified serializable object to the specified file
    public void Write<TState>(File file, ISerializable<TState> serializable) where TState : State
    {
      var state = serializable.Serialize();
      Write(file, state);
    }

    // Write the specified serializable object to the specified file with the provided context
    public void Write<TState, TContext>(File file, ISerializable<TState, TContext> serializable, TContext context) where TState : State
    {
      var state = serializable.Serialize(context);
      Write(file, state);
    }

    // Move the specified file to a new destination file
    public void Move(File file, File destination)
    {
      // Move the file
      if (file.adapter == destination.adapter)
      {
        file.Move(destination.path);
      }
      else
      {
        var data = file.Read();
        destination.Write(data);
        file.Delete();
      }

      // Invoke the moved event
      OnFileMoved?.Invoke(file, destination);
    }

    // Copy the specified file to a new destination file
    public void Copy(File file, File destination)
    {
      // Copy the file
      if (file.adapter == destination.adapter)
      {
        file.Move(destination.path);
      }
      else
      {
        var data = file.Read();
        destination.Write(data);
      }

      // Invoke the copied event
      OnFileCopied?.Invoke(file, destination);
    }

    // Delete the specified source file
    public void Delete(File source)
    {
      // Delete the file
      source.Delete();

      // Invoke the deleted event
      OnFileDeleted?.Invoke(source);
    }
  }
}