using MessagePack;
using System;

namespace Audune.Persistence
{
  // State that contains a long value
  [MessagePackFormatter(typeof(MessagePackStateFormatter))]
  public class LongState : State, IEquatable<LongState>
  {
    // The value of the long
    private long _value;


    // Constructor
    public LongState(long value)
    {
      _value = value;
    }


    #region Equatable implementation
    // Return if the long equals another object
    public override bool Equals(object other)
    {
      return other is LongState state && Equals(state);
    }

    // Return if the long equals another long
    public bool Equals(LongState other)
    {
      return _value == other._value;
    }

    // Return the hash code of the long
    public override int GetHashCode()
    {
      return HashCode.Combine(_value);
    }
    #endregion

    #region Implicit operators
    public static implicit operator long(LongState state) => state._value;
    public static implicit operator LongState(long value) => new LongState(value);
    public static implicit operator LongState(int value) => new LongState(value);
    #endregion
  }
}