using System;

namespace Audune.Persistence
{
  // Class that defines an exception thrown while saving or loading persistent data
  public class PersistenceException : Exception
  {
    // Constructor
    public PersistenceException() : base() { }
    public PersistenceException(string message) : base(message) { }
    public PersistenceException(string message, Exception innerException) : base(message, innerException) { }
  }
}
