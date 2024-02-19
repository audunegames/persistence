using System;

namespace Audune.Persistence
{
  // Class that defines an exception thrown while evaluating paths
  public class PathEvaluationException : Exception
  {
    // Constructor
    public PathEvaluationException() : base() { }
    public PathEvaluationException(string message) : base(message) { }
    public PathEvaluationException(string message, Exception innerException) : base(message, innerException) { }
  }
}
