using MessagePack;
using System;
using UnityEngine;

namespace Audune.Persistence
{
  // State that contains a Vector2Int value
  [MessagePackFormatter(typeof(MessagePackStateFormatter))]
  public class Vector2IntState : State
  {
    // The value of the vector
    private readonly Vector2Int value;


    // Constructor
    public Vector2IntState(Vector2Int value)
    {
      this.value = value;
    }


    #region Equatable implementation
    // Return if the vector equals another object
    public override bool Equals(object other)
    {
      return other != null && Equals(other as Vector2IntState);
    }

    // Return if the vector equals another vector
    public bool Equals(Vector2IntState other)
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
    public static implicit operator Vector2Int(Vector2IntState state) => state.value;
    public static implicit operator Vector2(Vector2IntState state) => state.value;
    #endregion
  }
}