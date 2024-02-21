using System;

namespace Audune.Persistence
{
  // Class that defines an exception thrown while interacting with an adapter
  public class AdapterException : PersistenceException
  {
    // Constructor
    public AdapterException(string message) : base(message) { }
    public AdapterException(string message, Exception innerException) : base(message, innerException) { }
  }
}
