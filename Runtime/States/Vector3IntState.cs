using MessagePack;
using System;
using UnityEngine;

namespace Audune.Persistence
{
  // State that contains a Vector3Int value
  [MessagePackFormatter(typeof(MessagePackStateFormatter))]
  public class Vector3IntState : State
  {
    // The value of the vector
    private readonly Vector3Int value;


    // Constructor
    public Vector3IntState(Vector3Int value)
    {
      this.value = value;
    }


    #region Equatable implementation
    // Return if the vector equals another object
    public override bool Equals(object other)
    {
      return other != null && Equals(other as Vector3IntState);
    }

    // Return if the vector equals another vector
    public bool Equals(Vector3IntState other)
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
    public static implicit operator Vector3Int(Vector3IntState state) => state.value;
    public static implicit operator Vector3(Vector3IntState state) => state.value;
    #endregion
  }
}