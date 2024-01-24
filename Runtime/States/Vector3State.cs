using MessagePack;
using System;
using UnityEngine;

namespace Audune.Persistence
{
  // State that contains a Vector3 value
  [MessagePackFormatter(typeof(MessagePackStateFormatter))]
  public class Vector3State : State
  {
    // The value of the vector
    private readonly Vector3 value;


    // Constructor
    public Vector3State(Vector3 value)
    {
      this.value = value;
    }


    #region Equatable implementation
    // Return if the vector equals another object
    public override bool Equals(object other)
    {
      return other != null && Equals(other as Vector3State);
    }

    // Return if the vector equals another vector
    public bool Equals(Vector3State other)
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
    public static implicit operator Vector3(Vector3State state) => state.value;
    #endregion
  }
}