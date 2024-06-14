using MessagePack;
using System;

namespace Audune.Persistence
{
  // State that contains a string value
  [MessagePackFormatter(typeof(MessagePackStateFormatter))]
  public class StringState : State
  {
    // The value of the string
    private readonly string _value;


    // Constructor
    public StringState(string value)
    {
      _value = value;
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
      return _value == other._value;
    }

    // Return the hash code of the integer
    public override int GetHashCode()
    {
      return HashCode.Combine(_value);
    }
    #endregion
    
    #region Implicit operators
    public static implicit operator string(StringState state) => state._value;
    #endregion
  }
}