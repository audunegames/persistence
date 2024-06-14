using MessagePack;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Audune.Persistence
{
  // State that contains a Quaternion value
  [MessagePackFormatter(typeof(MessagePackStateFormatter))]
  public class QuaternionState : State, IDynamicObjectState
  {
    // The value of the quaternion
    private Quaternion _value;


    // Return the fields for the dynamic object
    IReadOnlyDictionary<string, StateProperty> IDynamicObjectState.fields => new Dictionary<string, StateProperty> {
      { "x", new StateProperty(() => _value.x, x => _value.x = x as FloatState ?? _value.x) },
      { "y", new StateProperty(() => _value.y, y => _value.y = y as FloatState ?? _value.y) },
      { "z", new StateProperty(() => _value.z, z => _value.z = z as FloatState ?? _value.z) },
      { "w", new StateProperty(() => _value.w, w => _value.w = w as FloatState ?? _value.w) },
    };


    // Constructor
    public QuaternionState(Quaternion value)
    {
      _value = value;
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
      return _value == other._value;
    }

    // Return the hash code of the integer
    public override int GetHashCode()
    {
      return HashCode.Combine(_value);
    }
    #endregion

    #region Implicit operators
    public static implicit operator Quaternion(QuaternionState state) => state._value;
    #endregion
  }
}