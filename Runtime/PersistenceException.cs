using System;

namespace Audune.Persistence
{
  // Class that defines an exception thrown while interacting with the persistence system
  public class PersistenceException : Exception
  {
    // Constructor
    public PersistenceException(string message) : base(message) { }
    public PersistenceException(string message, Exception innerException) : base(message, innerException) { }
  }
}
