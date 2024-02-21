using System;

namespace Audune.Persistence
{
  // Class that defines an exception thrown while interacting with a backend
  public class BackendException : PersistenceException
  {
    // Constructor
    public BackendException(string message) : base(message) { }
    public BackendException(string message, Exception innerException) : base(message, innerException) { }
  }
}
