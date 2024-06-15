using MessagePack;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Audune.Persistence
{
  // State that contains a Vector3Int value
  [MessagePackFormatter(typeof(MessagePackStateFormatter))]
  public class Vector3IntState : State, IDynamicObjectState, IDynamicListState
  {
    // The value of the vector
    private Vector3Int _value;


    // Return the fields for the dynamic object
    IReadOnlyDictionary<string, StateProperty> IDynamicObjectState.fields => new Dictionary<string, StateProperty> {
      { "x", new StateProperty(() => _value.x, x => _value.x = x as IntState ?? _value.x) },
      { "y", new StateProperty(() => _value.y, y => _value.y = y as IntState ?? _value.y) },
      { "z", new StateProperty(() => _value.z, z => _value.z = z as IntState ?? _value.z) },
    };

    // Return the items for the dynamic list
    IReadOnlyList<StateProperty> IDynamicListState.items => new List<StateProperty> {
      { new StateProperty(() => _value.x, x => _value.x = x as IntState ?? _value.x) },
      { new StateProperty(() => _value.y, y => _value.y = y as IntState ?? _value.y) },
      { new StateProperty(() => _value.z, z => _value.z = z as IntState ?? _value.z) },
    };


    // Constructor
    public Vector3IntState(Vector3Int value)
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
      return other is Vector3IntState state && Equals(state);
    }

    // Return if the vector equals another vector
    public bool Equals(Vector3IntState other)
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
    public static implicit operator Vector3Int(Vector3IntState state) => state._value;
    public static implicit operator Vector3(Vector3IntState state) => state._value;
    public static implicit operator Vector3IntState(Vector3Int value) => new Vector3IntState(value);
    #endregion
  }
}