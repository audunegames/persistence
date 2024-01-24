using MessagePack;
using System;

namespace Audune.Persistence
{
  // State that contains a long value
  [MessagePackFormatter(typeof(MessagePackStateFormatter))]
  public class LongState : State, IEquatable<LongState>
  {
    // The value of the long
    private readonly long value;


    // Constructor
    public LongState(long value)
    {
      this.value = value;
    }


    #region Equatable implementation
    // Return if the long equals another object
    public override bool Equals(object other)
    {
      return other != null && Equals(other as LongState);
    }

    // Return if the long equals another long
    public bool Equals(LongState other)
    {
      return value == other.value;
    }

    // Return the hash code of the long
    public override int GetHashCode()
    {
      return HashCode.Combine(value);
    }
    #endregion

    #region Implicit operators
    public static implicit operator long(LongState state) => state.value;
    #endregion
  }
}