using MessagePack;
using System;

namespace Audune.Persistence
{
  // State that contains an integer value
  [MessagePackFormatter(typeof(MessagePackStateFormatter))]
  public class IntState : State, IEquatable<IntState>
  {
    // The value of the integer
    private readonly int _value;


    // Constructor
    public IntState(int value)
    {
      _value = value;
    }


    #region Equatable implementation
    // Return if the integer equals another object
    public override bool Equals(object other)
    {
      return other is IntState state && Equals(state);
    }

    // Return if the integer equals another integer
    public bool Equals(IntState other)
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
    public static implicit operator int(IntState state) => state._value;
    public static implicit operator long(IntState state) => state._value;
    public static implicit operator IntState(int value) => new IntState(value);
    #endregion
  }
}