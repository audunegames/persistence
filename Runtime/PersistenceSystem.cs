using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Audune.Persistence
{
  using FilePath = System.IO.Path;


  // Class that defines the system for persistent data
  public sealed class PersistenceSystem : MonoBehaviour
  {
    // Persistence system properties
    [SerializeField, Tooltip("The directory for persistent data, relative to the persistent data path of the application")]
    private string _directory = "Saves";
    [SerializeField, Tooltip("The extension for persistent data files")]
    private string _extension = ".save";

    // PErsistent system state
    private Backend _backend;

    // Persistence system events
    public event Action<string> OnPersistentDataSaved;
    public event Action<string> OnPersistentDataLoaded;
    public event Action<string> OnPersistentDataDeleted;


    // Return the directory
    public string directory => _directory;

    // Return the extension
    public string extension => _extension;

    // Return the absolute directory
    public string absoluteDirectory => FilePath.Combine(Application.persistentDataPath, _directory);


    // Awake is called when the script instance is being loaded
    private void Awake()
    {
      // Initialize the backend
      _backend = new MessagePackBackend();
    }


    // Return the path for a name
    public string GetPathForName(string name)
    {
      return FilePath.Combine(absoluteDirectory, $"{name}{_extension}");
    }


    // Enumerate over the existing persistent data
    public IEnumerable<string> Enumerate(Func<string, bool> predicate = null)
    {
      // If the directory doesn't exist, return an empty enumerable
      if (!Directory.Exists(absoluteDirectory))
        return Enumerable.Empty<string>();

      // Enumerate over the files in the directory that match the predicate
      return Directory.EnumerateFiles(absoluteDirectory, $"*{_extension}")
        .Select(path => FilePath.GetFileNameWithoutExtension(path))
        .Where(path => predicate == null || predicate(path));
    }

    // Return if a persistent data file exists
    public bool Exists(string name)
    {
      // Get the path for the name
      var path = GetPathForName(name);

      // Return if the file exists
      return File.Exists(path);
    }

    // Save the specified state to a persistent data file
    public void Save(string name, State state)
    {
      // Get the path for the name
      var path = GetPathForName(name);

      try
      {
        // Create the save directory if it doesn't exist
        if (!Directory.Exists(absoluteDirectory))
          Directory.CreateDirectory(absoluteDirectory);

        // Serialize the state
        var data = _backend.Serialize(state);

        // Write the data to the stream
        File.WriteAllBytes(path, data);

        // Invoke the saved event
        OnPersistentDataSaved?.Invoke(name);
      }
      catch (IOException ex)
      {
        throw new PersistenceException($"Could not write the data: {ex.Message}", ex);
      }
    }

    // Save the specified serializable object to a persistent data file
    public void Save<TState>(string name, ISerializable<TState> serializable) where TState : State
    {
      var state = serializable.Serialize();
      Save(name, state);
    }

    // Save the specified serializable object to a persistent data file with the provided context
    public void Save<TState, TContext>(string name, ISerializable<TState, TContext> serializable, TContext context) where TState : State
    {
      var state = serializable.Serialize(context);
      Save(name, state);
    }

    // Load a state from a persistent data file
    public TState Load<TState>(string name) where TState: State
    {
      // Get the path for the name
      var path = GetPathForName(name);

      try
      {
        // Read the data from the stream
        var data = File.ReadAllBytes(path);

        // Deserialize the state
        var state = _backend.Deserialize(data);

        // Check if the state is the correct type
        if (state is not TState expectedState)
          throw new PersistenceException($"Could not cast the state: expected state of type {typeof(TState)} but got {state.GetType()}");

        // Invoke the loaded event
        OnPersistentDataLoaded?.Invoke(name);

        return expectedState;
          
      }
      catch (IOException ex)
      {
        throw new PersistenceException($"Could not read the data: {ex.Message}", ex);
      }
    }

    // Load a deserializable object from a persistent data file
    public TDeserializable Load<TState, TDeserializable>(string name) where TState : State where TDeserializable : IDeserializable<TState>, new()
    {
      var state = Load<TState>(name);
      var deserializable = new TDeserializable();
      deserializable.Deserialize(state);
      return deserializable;
    }

    // Load a deserializable object from a persistent data file with the provided context
    public TDeserializable Load<TState, TContext, TDeserializable>(string name, TContext context) where TState : State where TDeserializable : IDeserializable<TState, TContext>, new()
    {
      var state = Load<TState>(name);
      var deserializable = new TDeserializable();
      deserializable.Deserialize(state, context);
      return deserializable;
    }

    // Load a deserializable object from a persistent data file into an existing object
    public void LoadInto<TState, TDeserializable>(string name, TDeserializable deserializable) where TState : State where TDeserializable : IDeserializable<TState>
    {
      var state = Load<TState>(name);
      deserializable.Deserialize(state);
    }

    // Load a deserializable object from a persistent data file into an existing object with the provided context
    public void LoadInto<TState, TContext, TDeserializable>(string name, TDeserializable deserializable, TContext context) where TState : State where TDeserializable : IDeserializable<TState, TContext>
    {
      var state = Load<TState>(name);
      deserializable.Deserialize(state, context);
    }

    // Delete a persistent data file
    public void Delete(string name)
    {
      // Get the file path for the name
      var filePath = GetPathForName(name);

      // Delete the file
      File.Delete(filePath);

      // Invoke the deleted event
      OnPersistentDataDeleted?.Invoke(name);
    }
  }
}