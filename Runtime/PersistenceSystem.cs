using Audune.Utils.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Audune.Persistence
{
  // Class that defines the system for persistent data
  [AddComponentMenu("Audune/Persistence/Persistence System")]
  public sealed class PersistenceSystem : MonoBehaviour
  {
    // Persistence system properties
    [SerializeField, Tooltip("The format of the persistence files"), SerializableTypeOptions(typeof(Backend), TypeDisplayOptions.DontShowNamespace)]
    private SerializableType _persistenceFileFormat = typeof(Backend).GetChildTypes().FirstOrDefault();

    // Internal state of the persistence system
    private Backend _backend;

    // Persistence system events
    public event Action<File> OnFileRead;
    public event Action<File> OnFileWritten;
    public event Action<File, File> OnFileMoved;
    public event Action<File, File> OnFileCopied;
    public event Action<File> OnFileDeleted;


    // Awake is called when the script instance is being loaded
    private void Awake()
    {
      // Create the backend
      if (_persistenceFileFormat.type == null)
        throw new ArgumentException("Cannot initialize persistence system without a specified format");

      _backend = Activator.CreateInstance(_persistenceFileFormat) as Backend;
    }


    #region Adapter management
    // Return all registered adapters
    public IEnumerable<Adapter> GetAdapters()
    {
      return GetComponents<Adapter>().OrderBy(a => a.adapterPriority);
    }

    // Return all enabled registered adapters
    public IEnumerable<Adapter> GetEnabledAdapters()
    {
      return GetAdapters().Where(adapter => adapter.adapterEnabled);
    }

    // Return if an adapter with the specified name exists
    public bool TryGetAdapter(string name, out Adapter adapter)
    {
      adapter = GetAdapters().Where(adapter => adapter.adapterName == name).FirstOrDefault();
      return adapter != null;
    }

    // Return the adapter with the specified name
    public Adapter GetAdapter(string name)
    {
      if (TryGetAdapter(name, out Adapter adapter))
        return adapter;
      else
        throw new PersistenceException($"Could not find a registered adapter with name {name}");
    }

    // Return if there is a first adapter that is enabled
    public bool TryGetFirstEnabledAdapter(out Adapter adapter)
    {
      adapter = GetEnabledAdapters().FirstOrDefault();
      return adapter != null;
    }

    // Return the first adapter that is enabled
    public Adapter GetFirstEnabledAdapter()
    {
      if (TryGetFirstEnabledAdapter(out Adapter adapter))
        return adapter;
      else
        throw new PersistenceException("Could not find an enabled registered adapter");
    }
    #endregion

    #region File management
    // List the available files
    public IEnumerable<File> List(Predicate<string> predicate = null)
    {
      return GetAdapters().SelectMany(adapter => adapter.List(predicate).Select(path => adapter.GetFile(path)));
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
    public void Read<TState>(File file, IDeserializable<TState> deserializable) where TState : State
    {
      var state = Read<TState>(file);
      deserializable.Deserialize(state);
    }

    // Read a deserializable object from the specified file into an existing object with the provided context
    public void Read<TState, TContext>(File file, IDeserializable<TState, TContext> deserializable, TContext context) where TState : State
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
    #endregion
  }
}