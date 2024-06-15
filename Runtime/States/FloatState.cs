using MessagePack;
using System;

namespace Audune.Persistence
{
  // State that contains a float value
  [MessagePackFormatter(typeof(MessagePackStateFormatter))]
  public class FloatState : State, IEquatable<FloatState>
  {
    // The value of the float
    private readonly float _value;


    // Constructor
    public FloatState(float value)
    {
      _value = value;
    }


    #region Equatable implementation
    // Return if the float equals another object
    public override bool Equals(object other)
    {
      return other is FloatState state && Equals(state);
    }

    // Return if the float equals another float
    public bool Equals(FloatState other)
    {
      return _value == other._value;
    }

    // Return the hash code of the float
    public override int GetHashCode()
    {
      return HashCode.Combine(_value);
    }
    #endregion

    #region Implicit operators
    public static implicit operator float(FloatState state) => state._value;
    public static implicit operator double(FloatState state) => state._value;
    public static implicit operator FloatState(float value) => new FloatState(value);
    #endregion
  }
}