using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Audune.Persistence
{
  // Interface that defines an object state backed by a list of properties
  internal interface IDynamicObjectState : IObjectState
  {
    // Return the fields for the dynamic object
    public IReadOnlyDictionary<string, StateProperty> fields { get; }


    // Return the count of the fields
    int IReadOnlyCollection<KeyValuePair<string, State>>.Count => fields.Count;


    // Get a field with the specified name
    TState IObjectState.Get<TState>(string name, TState defaultState = null)
    {
      return fields.TryGetValue(name, out var property) ? property.value as TState ?? defaultState : defaultState;
    }

    // Return if a field with the specified name exists
    bool IObjectState.TryGet<TState>(string name, out TState state)
    {
      state = Get<TState>(name);
      return state != null;
    }

    // Set a field with the specified name
    void IObjectState.Set(string name, State state)
    {
      if (fields.TryGetValue(name, out var property))
        property.value = state;
    }

    // Return a generic enumerator
    IEnumerator<KeyValuePair<string, State>> IEnumerable<KeyValuePair<string, State>>.GetEnumerator()
    {
      return fields.Select(e => KeyValuePair.Create(e.Key, e.Value.value)).GetEnumerator();
    }

    // Return an enumerator
    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }
  }
}