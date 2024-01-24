using MessagePack;
using System;

namespace Audune.Persistence
{
  // State that contains a boolean value
  [MessagePackFormatter(typeof(MessagePackStateFormatter))]
  public class BoolState : State, IEquatable<BoolState>
  {
    // The value of the boolean
    private readonly bool value;


    // Constructor
    public BoolState(bool value)
    {
      this.value = value;
    }


    #region Equatable implementation
    // Return if the equals another object
    public override bool Equals(object other)
    {
      return other != null && Equals(other as BoolState);
    }

    // Return if the boolean equals another boolean
    public bool Equals(BoolState other)
    {
      return value == other.value;
    }

    // Return the hash code of the boolean
    public override int GetHashCode()
    {
      return HashCode.Combine(value);
    }
    #endregion

    #region Implicit operators
    public static implicit operator bool(BoolState state) => state.value;
    #endregion
  }
}