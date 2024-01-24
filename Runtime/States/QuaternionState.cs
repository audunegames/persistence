using MessagePack;
using System;
using UnityEngine;

namespace Audune.Persistence
{
  // State that contains a Quaternion value
  [MessagePackFormatter(typeof(MessagePackStateFormatter))]
  public class QuaternionState : State
  {
    // The value of the quaternion
    private readonly Quaternion value;


    // Constructor
    public QuaternionState(Quaternion value)
    {
      this.value = value;
    }


    #region Equatable implementation
    // Return if the quaternion equals another object
    public override bool Equals(object other)
    {
      return other != null && Equals(other as QuaternionState);
    }

    // Return if the quaternion equals another quaternion
    public bool Equals(QuaternionState other)
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
    public static implicit operator Quaternion(QuaternionState state) => state.value;
    #endregion
  }
}