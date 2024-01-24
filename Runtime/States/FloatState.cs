using MessagePack;
using System;

namespace Audune.Persistence
{
  // State that contains a float value
  [MessagePackFormatter(typeof(MessagePackStateFormatter))]
  public class FloatState : State, IEquatable<FloatState>
  {
    // The value of the float
    private readonly float value;


    // Constructor
    public FloatState(float value)
    {
      this.value = value;
    }


    #region Equatable implementation
    // Return if the float equals another object
    public override bool Equals(object other)
    {
      return other != null && Equals(other as FloatState);
    }

    // Return if the float equals another float
    public bool Equals(FloatState other)
    {
      return value == other.value;
    }

    // Return the hash code of the float
    public override int GetHashCode()
    {
      return HashCode.Combine(value);
    }
    #endregion

    #region Implicit operators
    public static implicit operator float(FloatState state) => state.value;
    public static implicit operator double(FloatState state) => state.value;
    #endregion
  }
}