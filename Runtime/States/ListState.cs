using MessagePack;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Audune.Persistence
{
  // State that defines an list of state items
  [MessagePackFormatter(typeof(MessagePackStateFormatter))]
  public class ListState : State, IReadOnlyCollection<State>, IEquatable<ListState>
  {
    // The items of the list
    private readonly List<State> _items;


    // Constructor
    public ListState(IEnumerable<State> items = null) 
    { 
      if (items != null)
        _items = new List<State>(items);
      else
        _items = new List<State>();
    }


    #region Getting items
    // Get an item with the specified index
    public TState Get<TState>(int index) where TState : State
    {
      return index >= 0 && index < _items.Count ? _items[index] as TState : null;
    }

    // Return if an item with the specified index exists
    public bool TryGet<TState>(int index, out TState state) where TState : State
    {
      state = Get<TState>(index);
      return state != null;
    }

    // Enumerate over all items of the specified type
    public IEnumerable<TState> GetAll<TState>() where TState : State
    {
      return _items.Where(item => item is TState state).Select(item => item as TState);
    }

    // Enumerate over items of the specified type that match the specified predicate
    public IEnumerable<TState> GetAllWhere<TState>(Func<TState, bool> predicate) where TState : State
    {
      return _items.Where(item => item is TState state && predicate(state)).Select(item => item as TState);
    }

    // Return if the list contains the specified item
    public bool Contains(State item)
    {
      return _items.Contains(item);
    }
    #endregion

    #region Setting items
    // Set an item with the specified index
    public void Set(int index, State state)
    {
      _items[index] = state;
    }

    // Set a new list with the specified index and return it
    public ListState SetNewList(int index)
    {
      var state = new ListState();
      Set(index, state);
      return state;
    }

    // Set a new object with the specified index and return it
    public ObjectState SetNewObject(int index)
    {
      var objectField = new ObjectState();
      Set(index, objectField);
      return objectField;
    }

    // Add an item
    public void Add(State state)
    {
      _items.Add(state);
    }

    // Add a new list and return it
    public ListState AddNewList()
    {
      var state = new ListState();
      Add(state);
      return state;
    }

    // Add a new object and return it
    public ObjectState AddNewObject()
    {
      var objectField = new ObjectState();
      Add(objectField);
      return objectField;
    }
    #endregion

    #region Removing items
    // Remove the item with the specified index
    public void Remove(int index)
    {
      _items.RemoveAt(index);
    }
    #endregion

    #region Collection implementation
    // Return the number of items
    public int Count => _items.Count;


    // Return a generic enumerator over the values
    public IEnumerator<State> GetEnumerator()
    {
      return _items.GetEnumerator();
    }

    // Return an enumerator over the values
    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }
    #endregion

    #region Equatable implementation
    // Return if the list equals another object
    public override bool Equals(object other)
    {
      return other != null && Equals(other as ListState);
    }

    // Return if the list equals another list
    public bool Equals(ListState other)
    {
      return EqualityComparer<List<State>>.Default.Equals(_items, other._items);
    }

    // Return the hash code of the list
    public override int GetHashCode()
    {
      return HashCode.Combine(_items);
    }
    #endregion
  }
}