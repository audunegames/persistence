using System;

namespace Audune.Persistence
{
  // Class that defines an exception thrown while evaluating paths
  public class StatePathEvaluationException : PersistenceException
  {
    // Constructor
    public StatePathEvaluationException(string message) : base(message) { }
    public StatePathEvaluationException(string message, Exception innerException) : base(message, innerException) { }
  }
}
