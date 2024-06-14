using System.Collections.Generic;

namespace Audune.Persistence
{
  // Interface that defines a list state
  public interface IListState : IReadOnlyCollection<State>
  {
    // Get an item with the specified index
    public TState Get<TState>(int index, TState defaultState = null) where TState : State;

    // Return if an item with the specified index exists
    public bool TryGet<TState>(int index, out TState state) where TState : State;

    // Set an item with the specified index
    public void Set(int index, State state);
  }
}