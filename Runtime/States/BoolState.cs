using MessagePack;
using System;

namespace Audune.Persistence
{
  // State that contains a boolean value
  [MessagePackFormatter(typeof(MessagePackStateFormatter))]
  public class BoolState : State, IEquatable<BoolState>
  {
    // Static instances of bool states
    public static readonly BoolState False = new BoolState(false);
    public static readonly BoolState True = new BoolState(true);


    // The value of the boolean
    private readonly bool _value;


    // Constructor
    public BoolState(bool value)
    {
      _value = value;
    }


    #region Equatable implementation
    // Return if the equals another object
    public override bool Equals(object other)
    {
      return other is BoolState state && Equals(state);
    }

    // Return if the boolean equals another boolean
    public bool Equals(BoolState other)
    {
      return _value == other._value;
    }

    // Return the hash code of the boolean
    public override int GetHashCode()
    {
      return HashCode.Combine(_value);
    }
    #endregion

    #region Implicit operators
    public static implicit operator bool(BoolState state) => state._value;
    public static implicit operator BoolState(bool value) => new BoolState(value);
    #endregion
  }
}