using MessagePack;
using System;

namespace Audune.Persistence
{
  // State that contains a string value
  [MessagePackFormatter(typeof(MessagePackStateFormatter))]
  public class StringState : State
  {
    // The value of the string
    private readonly string value;


    // Constructor
    public StringState(string value)
    {
      this.value = value;
    }


    #region Equatable implementation
    // Return if the string equals another object
    public override bool Equals(object other)
    {
      return other != null && Equals(other as StringState);
    }

    // Return if the string equals another string
    public bool Equals(StringState other)
    {
      return value == other.value;
    }

    // Return the hash code of the integer
    public override int GetHashCode()
    {
      return HashCode.Combine(value);
    }
    #endregion
    
    #region Implicit operators
    public static implicit operator string(StringState state) => state.value;
    #endregion
  }
}