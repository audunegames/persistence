using MessagePack;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Audune.Persistence
{
  // State that contains a Vector3 value
  [MessagePackFormatter(typeof(MessagePackStateFormatter))]
  public class Vector3State : State, IDynamicObjectState, IDynamicListState
  {
    // The value of the vector
    private Vector3 _value;


    // Return the fields for the dynamic object
    IReadOnlyDictionary<string, StateProperty> IDynamicObjectState.fields => new Dictionary<string, StateProperty> {
      { "x", new StateProperty(() => _value.x, x => _value.x = x as FloatState ?? _value.x) },
      { "y", new StateProperty(() => _value.y, y => _value.y = y as FloatState ?? _value.y) },
      { "z", new StateProperty(() => _value.z, z => _value.z = z as FloatState ?? _value.z) },
    };

    // Return the items for the dynamic list
    IReadOnlyList<StateProperty> IDynamicListState.items => new List<StateProperty> {
      { new StateProperty(() => _value.x, x => _value.x = x as FloatState ?? _value.x) },
      { new StateProperty(() => _value.y, y => _value.y = y as FloatState ?? _value.y) },
      { new StateProperty(() => _value.z, z => _value.z = z as FloatState ?? _value.z) },
    };


    // Constructor
    public Vector3State(Vector3 value)
    {
      _value = value;
    }


    #region Dynamic state implementation
    // Return an enumerator
    IEnumerator IEnumerable.GetEnumerator()
    {
      return ((IDynamicObjectState)this).GetEnumerator();
    }
    #endregion

    #region Equatable implementation
    // Return if the vector equals another object
    public override bool Equals(object other)
    {
      return other is Vector3State state && Equals(state);
    }

    // Return if the vector equals another vector
    public bool Equals(Vector3State other)
    {
      return _value == other._value;
    }

    // Return the hash code of the vector
    public override int GetHashCode()
    {
      return HashCode.Combine(_value);
    }
    #endregion

    #region Implicit operators
    public static implicit operator Vector3(Vector3State state) => state._value;
    public static implicit operator Vector3State(Vector3 value) => new Vector3State(value);
    public static implicit operator Vector3State(Vector3Int value) => new Vector3State(value);
    #endregion
  }
}