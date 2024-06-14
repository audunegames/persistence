using MessagePack;
using System;

namespace Audune.Persistence
{
  // State that contains a double value
  [MessagePackFormatter(typeof(MessagePackStateFormatter))]
  public class DoubleState : State, IEquatable<DoubleState>
  {
    // The value of the double
    private readonly double _value;


    // Constructor
    public DoubleState(double value)
    {
      _value = value;
    }


    #region Equatable implementation
    // Return if the double equals another object
    public override bool Equals(object other)
    {
      return other != null && Equals(other as DoubleState);
    }

    // Return if the double equals another double
    public bool Equals(DoubleState other)
    {
      return _value == other._value;
    }

    // Return the hash code of the double
    public override int GetHashCode()
    {
      return HashCode.Combine(_value);
    }
    #endregion

    #region Implicit operators
    public static implicit operator double(DoubleState state) => state._value;
    #endregion
  }
}