using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Audune.Persistence
{
  // Class that defines an adapter that stores data on the local filesystem
  [AddComponentMenu("Audune Persistence/Local Adapter")]
  public class LocalAdapter : Adapter
  {
    // Enum that defines the directory type
    public enum DirectoryType
    {
      DataPath,             // Stores files relative to Application.dataPath
      PersistentDataPath,   // Stores files relative to Application.persistentDataPath
      AppData,              // Stores files relative to %APPDATA% on Windows
      LocalAppData,         // Stores files relative to %LOCALAPPDATA% on Windows
      UserProfile,          // Stores files relative to %USERPROFILE% on Windows
    }


    // Local persistence adapter properties
    [SerializeField, Tooltip("The root location to store files"), Space]
    private DirectoryType _root = DirectoryType.PersistentDataPath;
    [SerializeField, Tooltip("The directory to store files, relative to the directory type")]
    private string _directory = "Saves";
    [SerializeField, Tooltip("The extension to use to store files")]
    private string _extension = ".save";


    // Return if the adapter is accessible
    public override bool adapterEnabled => true;

    // Return the absolute directory to store files
    public string directory => _root switch {
      DirectoryType.DataPath => Path.Combine(Application.dataPath, _directory),
      DirectoryType.PersistentDataPath => Path.Combine(Application.persistentDataPath, _directory),
      DirectoryType.AppData => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), _directory),
      DirectoryType.LocalAppData => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), _directory),
      DirectoryType.UserProfile => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), _directory),
      _ => _directory,
    };


    // Return the path for the specified source file
    public string GetPath(string source)
    {
      return Path.Combine(directory, $"{source}{_extension}");
    }


    // List the available files in the adapter
    public override IEnumerable<string> List(Predicate<string> predicate = null)
    {
      try
      {
        if (!Directory.Exists(directory))
          return Enumerable.Empty<string>();

        return Directory.EnumerateFiles(directory, $"*{_extension}")
          .Select(path => {
            var relativePath = Path.GetRelativePath(directory, path);
            return relativePath[..(relativePath.Length - _extension.Length)];
          })
          .Where(path => predicate == null || predicate(path));
      }
      catch (IOException ex)
      {
        throw new AdapterException($"Could not list files: {ex.Message}", ex);
      }
    }

    // Return the specified file
    public override bool Exists(string path)
    {
      var sourcePath = GetPath(path);

      return System.IO.File.Exists(sourcePath);
    }

    // Read a stream of bytes from the specified file
    public override byte[] Read(string path)
    {
      try
      {
        var sourcePath = GetPath(path);

        return System.IO.File.ReadAllBytes(sourcePath);
      }
      catch (IOException ex)
      {
        throw new AdapterException($"Could not read file \"{path}\": {ex.Message}", ex);
      }
    }

    // Write the specified stream of bytes to the specified file
    public override void Write(string path, byte[] data)
    {
      try
      {
        var sourcePath = GetPath(path);

        if (!Directory.Exists(Path.GetDirectoryName(sourcePath)))
          Directory.CreateDirectory(Path.GetDirectoryName(sourcePath));

        System.IO.File.WriteAllBytes(sourcePath, data);
      }
      catch (IOException ex)
      {
        throw new AdapterException($"Could not write file \"{path}\": {ex.Message}", ex);
      }
    }

    // Move the specified file to a new destination file
    public override void Move(string path, string destination)
    {
      try
      {
        var sourcePath = GetPath(path);
        var destinationPath = GetPath(destination);

        if (!Directory.Exists(Path.GetDirectoryName(destinationPath)))
          Directory.CreateDirectory(Path.GetDirectoryName(destinationPath));

        System.IO.File.Move(sourcePath, destinationPath);
      }
      catch (IOException ex)
      {
        throw new AdapterException($"Could not move file \"{path}\": {ex.Message}", ex);
      }
    }

    // Copy the specified file to a new destination file
    public override void Copy(string path, string destination)
    {
      try
      {
        var sourcePath = GetPath(path);
        var destinationPath = GetPath(destination);

        if (!Directory.Exists(Path.GetDirectoryName(destinationPath)))
          Directory.CreateDirectory(Path.GetDirectoryName(destinationPath));

        System.IO.File.Copy(sourcePath, destinationPath);
      }
      catch (IOException ex)
      {
        throw new AdapterException($"Could not copy file \"{path}\": {ex.Message}", ex);
      }
    }

    // Delete the specified file
    public override void Delete(string path)
    {
      try
      {
        var sourcePath = GetPath(path);

        System.IO.File.Delete(sourcePath);
      }
      catch (IOException ex)
      {
        throw new AdapterException($"Could not delete file \"{path}\": {ex.Message}", ex);
      }
    }
  }
}