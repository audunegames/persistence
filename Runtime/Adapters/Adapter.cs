using System.Collections.Generic;
using System;
using UnityEngine;

namespace Audune.Persistence
{
  // Class that defines an adapter contained in a Unity component
  public abstract class Adapter : MonoBehaviour, IAdapter
  {
    // Adapter properties
    [SerializeField, Tooltip("The name of the adapter")]
    private string _adapterName;

    // Internal state of the adapter
    private PersistenceSystem _system;


    // Return the name of the adapter
    public string adapterName => _adapterName;

    // Return if the adapter is enabled
    public abstract bool adapterEnabled { get; }


    // Awake is called when the script instance is being loaded
    protected void Awake()
    {
      _system = FindObjectOfType<PersistenceSystem>();
      if (_system == null)
        throw new PersistenceException("Could not find a persistence system in the scene");
    }

    // OnEnable is called when the component becomes enabled
    protected void OnEnable()
    {
      // Check if the system is not null
      if (_system == null)
        throw new AdapterException($"There is no system set for adapter {adapterName} of type {GetType()}");

      // Register the adapter
      _system.RegisterAdapter(this);
    }

    // OnDisable is called when the component becomes disabled
    protected void OnDisable()
    {
      // Check if the system is not null
      if (_system == null)
        throw new AdapterException($"There is no system set for adapter {adapterName} of type {GetType()}");

      // Unregister the adapter
      _system.UnregisterAdapter(this);
    }


    // List the available files in the adapter
    public abstract IEnumerable<string> List(Predicate<string> predicate = null);

    // Return if the specified file exists
    public abstract bool Exists(string path);

    // Read a stream of bytes from the specified file
    public abstract byte[] Read(string path);

    // Write the specified stream of bytes to the specified file
    public abstract void Write(string path, byte[] data);

    // Move the specified file to a new destination file
    public abstract void Move(string path, string destination);

    // Copy the specified file to a new destination file
    public abstract void Copy(string path, string destination);

    // Delete the specified file
    public abstract void Delete(string path);

    // Return a persistence file in the adapter
    public File GetFile(string path)
    {
      return new File(this, path);
    }
  }
}