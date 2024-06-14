using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Audune.Persistence
{
  // Interface that defines an list state backed by a list of properties
  internal interface IDynamicListState : IListState
  {
    // Return the items for the dynamic list
    public IReadOnlyList<StateProperty> items { get; }


    // Return the count of the items
    int IReadOnlyCollection<State>.Count => items.Count;


    // Get an item with the specified index
    TState IListState.Get<TState>(int index, TState defaultState = null)
    {
      return index >= 0 && index < items.Count ? items[index].value as TState ?? defaultState : defaultState;
    }

    // Return if an item with the specified index exists
    bool IListState.TryGet<TState>(int index, out TState state)
    {
      state = Get<TState>(index);
      return state != null;
    }

    // Set an item with the specified index
    void IListState.Set(int index, State state)
    {
      if (index >= 0 && index < items.Count)
        items[index].value = state;
    }

    // Return a generic enumerator
    IEnumerator<State> IEnumerable<State>.GetEnumerator()
    {
      return items.Select(p => p.value).GetEnumerator();
    }

    // Return an enumerator
    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }
  }
}