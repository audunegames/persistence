using System.Collections.Generic;
using System;

namespace Audune.Persistence
{
  // Interface that defines an adapter for the persistence system, i.e. a location where files are stored
  public interface IAdapter
  {
    // Return the name of the adapter
    public string adapterName { get; }

    // Return if the adapter is enabled
    public bool adapterEnabled { get; }


    // List the available files in the adapter
    public IEnumerable<string> List(Predicate<string> predicate = null);

    // Return if the specified file exists
    public bool Exists(string path);

    // Read a stream of bytes from the specified file
    public byte[] Read(string path);

    // Write the specified stream of bytes to the specified file
    public void Write(string path, byte[] data);

    // Move the specified file to a new destination file
    public void Move(string path, string destination);

    // Copy the specified file to a new destination file
    public void Copy(string path, string destination);

    // Delete the specified file
    public void Delete(string path);


    // Return a file in the adapter
    public File GetFile(string path)
    {
      return new File(this, path);
    }
  }
}