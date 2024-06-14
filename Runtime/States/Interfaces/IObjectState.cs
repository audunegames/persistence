using System.Collections.Generic;

namespace Audune.Persistence
{
  // Interface that defines an object state
  public interface IObjectState : IReadOnlyCollection<KeyValuePair<string, State>>
  {
    // Get a field with the specified name
    public TState Get<TState>(string name, TState defaultState = null) where TState : State;

    // Return if a field with the specified name exists
    public bool TryGet<TState>(string name, out TState state) where TState : State;

    // Set a field with the specified name
    public void Set(string name, State state);
  }
}