using MessagePack;
using System;

namespace Audune.Persistence
{
  // State that contains an integer value
  [MessagePackFormatter(typeof(MessagePackStateFormatter))]
  public class IntState : State, IEquatable<IntState>
  {
    // The value of the integer
    private readonly int value;


    // Constructor
    public IntState(int value)
    {
      this.value = value;
    }


    #region Equatable implementation
    // Return if the integer equals another object
    public override bool Equals(object other)
    {
      return other != null && Equals(other as IntState);
    }

    // Return if the integer equals another integer
    public bool Equals(IntState other)
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
    public static implicit operator int(IntState state) => state.value;
    public static implicit operator long(IntState state) => state.value;
    #endregion
  }
}