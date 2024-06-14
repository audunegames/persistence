using MessagePack;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Audune.Persistence
{
  // State that contains a Vector2 value
  [MessagePackFormatter(typeof(MessagePackStateFormatter))]
  public class Vector2State : State, IDynamicObjectState, IDynamicListState
  {
    // The value of the vector
    private Vector2 _value;


    // Return the fields for the dynamic object
    IReadOnlyDictionary<string, StateProperty> IDynamicObjectState.fields => new Dictionary<string, StateProperty> {
      { "x", new StateProperty(() => _value.x, x => _value.x = x as IntState ?? _value.x) },
      { "y", new StateProperty(() => _value.y, y => _value.y = y as IntState ?? _value.y) },
    };

    // Return the items for the dynamic list
    IReadOnlyList<StateProperty> IDynamicListState.items => new List<StateProperty> {
      { new StateProperty(() => _value.x, x => _value.x = x as IntState ?? _value.x) },
      { new StateProperty(() => _value.y, y => _value.y = y as IntState ?? _value.y) },
    };


    // Constructor
    public Vector2State(Vector2 value)
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
      return other != null && Equals(other as Vector2State);
    }

    // Return if the vector equals another vector
    public bool Equals(Vector2State other)
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
    public static implicit operator Vector2(Vector2State state) => state._value;
    #endregion
  }
}