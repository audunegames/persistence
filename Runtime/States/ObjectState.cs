using MessagePack;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Audune.Persistence
{
  // State that defines an object containing key-state pairs
  [MessagePackFormatter(typeof(MessagePackStateFormatter))]
  public class ObjectState : State, IObjectState, IEquatable<ObjectState>
  {
    // The dictionary of key-value pairs
    private readonly Dictionary<string, State> _fields;


    // Return the keys and values
    public IEnumerable<string> keys => _fields.Keys;
    public IEnumerable<State> values => _fields.Values;


    // Constructor
    public ObjectState(IEnumerable<KeyValuePair<string, State>> items = null)
    {
      if (items != null)
        _fields = new Dictionary<string, State>(items);
      else
        _fields = new Dictionary<string, State>();
    }


    #region Getting fields
    // Get a field with the specified name
    public TState Get<TState>(string name, TState defaultState = null) where TState : State
    {
      return _fields.TryGetValue(name, out var state) ? state as TState ?? defaultState : defaultState;
    }

    // Return if a field with the specified name exists
    public bool TryGet<TState>(string name, out TState state) where TState : State
    {
      state = Get<TState>(name);
      return state != null;
    }

    // Enumerate over all fields of the specified type
    public IEnumerable<KeyValuePair<string, TState>> GetAll<TState>() where TState : State
    {
      return _fields.Where(e => e.Value is TState state).Select(e => KeyValuePair.Create(e.Key, e.Value as TState));
    }

    // Enumerate over all field keys of the specified type
    public IEnumerable<string> GetAllKeys<TState>() where TState : State
    {
      return _fields.Where(e => e.Value is TState state).Select(e => e.Key);
    }

    // Enumerate over all field values of the specified type
    public IEnumerable<TState> GetAllValues<TState>() where TState : State
    {
      return _fields.Where(e => e.Value is TState state).Select(e => e.Value as TState);
    }

    // Enumerate over fields of the specified type that match the specified predicate
    public IEnumerable<KeyValuePair<string, TState>> GetAllWhere<TState>(Func<TState, bool> predicate) where TState : State
    {
      return _fields.Where(e => e.Value is TState state && predicate(state)).Select(e => KeyValuePair.Create(e.Key, e.Value as TState));
    }

    // Enumerate over all field keys of the specified type that match the specified predicate
    public IEnumerable<string> GetAllKeysWhere<TState>(Func<TState, bool> predicate) where TState : State
    {
      return _fields.Where(e => e.Value is TState state && predicate(state)).Select(e => e.Key);
    }

    // Enumerate over field values of the specified type that match the specified predicate
    public IEnumerable<TState> GetAllValuesWhere<TState>(Func<TState, bool> predicate) where TState : State
    {
      return _fields.Where(e => e.Value is TState state && predicate(state)).Select(e => e.Value as TState);
    }

    // Return if the object contains the specified field key
    public bool ContainsKey(string name)
    {
      return _fields.ContainsKey(name);
    }

    // Return if the object contains the specified field value
    public bool ContainsValue(State value)
    {
      return _fields.ContainsValue(value);
    }
    #endregion

    #region Setting fields
    // Set a field with the specified name
    public void Set(string name, State state)
    {
      _fields[name] = state;
    }

    // Set a new list with the specified name and return it
    public ListState SetNewList(string name)
    {
      var state = new ListState();
      Set(name, state);
      return state;
    }

    // Set a new object with the specified name and return it
    public ObjectState SetNewObject(string name)
    {
      var objectField = new ObjectState();
      Set(name, objectField);
      return objectField;
    }
    #endregion

    #region Removing fields
    // Remove the field with the specified name
    public void Remove(string name)
    {
      _fields.Remove(name);
    }
    #endregion

    #region Collection implementation
    // Return the number of fields
    public int Count => _fields.Count;


    // Return a generic enumerator over the fields
    public IEnumerator<KeyValuePair<string, State>> GetEnumerator()
    {
      return _fields.GetEnumerator();
    }

    // Return an enumerator over the fields
    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }
    #endregion

    #region Equatable implementation
    // Return if the hash equals another object
    public override bool Equals(object other)
    {
      return other != null && Equals(other as ObjectState);
    }

    // Return if the hash equals another hash
    public bool Equals(ObjectState other)
    {
      return EqualityComparer<Dictionary<string, State>>.Default.Equals(_fields, other._fields);
    }

    // Return the hash code of the hash
    public override int GetHashCode()
    {
      return HashCode.Combine(_fields);
    }
    #endregion
  }
}