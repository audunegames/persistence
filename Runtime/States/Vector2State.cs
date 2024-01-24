using MessagePack;
using System;
using UnityEngine;

namespace Audune.Persistence
{
  // State that contains a Vector2 value
  [MessagePackFormatter(typeof(MessagePackStateFormatter))]
  public class Vector2State : State
  {
    // The value of the vector
    private readonly Vector2 value;


    // Constructor
    public Vector2State(Vector2 value)
    {
      this.value = value;
    }


    #region Equatable implementation
    // Return if the vector equals another object
    public override bool Equals(object other)
    {
      return other != null && Equals(other as Vector2State);
    }

    // Return if the vector equals another vector
    public bool Equals(Vector2State other)
    {
      return value == other.value;
    }

    // Return the hash code of the vector
    public override int GetHashCode()
    {
      return HashCode.Combine(value);
    }
    #endregion

    #region Implicit operators
    public static implicit operator Vector2(Vector2State state) => state.value;
    #endregion
  }
}