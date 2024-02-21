using System;
using System.Collections.Generic;
using System.Linq;

namespace Audune.Persistence
{
  // Class that defines extensions for state paths
  public static class StatePathExtensions
  {
    #region Getting a state from a path
    // Get a state with the specified path in the specified state
    public static TState GetAtPath<TState>(this State rootState, StatePath path) where TState : State
    {
      return path.EvaluateGetter(rootState) as TState;
    }

    // Return if a state with the specified path exists in the specified state
    public static bool TryGetAtPath<TState>(this State rootState, StatePath path, out TState state) where TState : State
    {
      try
      {
        state = rootState.GetAtPath<TState>(path);
        return state != null;
      }
      catch (StatePathEvaluationException)
      {
        state = null;
        return false;
      }
    }

    // Get all items with the specified path of the specified type
    public static IEnumerable<TState> GetAllItemsAtPath<TState>(this State rootState, StatePath path) where TState : State
    {
      var state = rootState.GetAtPath<ListState>(path);
      return state?.GetAll<TState>() ?? Enumerable.Empty<TState>();
    }

    // Get all fields with the specified path of the specified type
    public static IEnumerable<KeyValuePair<string, TState>> GetAllFieldsAtPath<TState>(this State rootState, StatePath path) where TState : State
    {
      var state = rootState.GetAtPath<ObjectState>(path);
      return state?.GetAll<TState>() ?? Enumerable.Empty<KeyValuePair<string, TState>>();
    }

    // Get all field keys with the specified path of the specified type
    public static IEnumerable<string> GetAllKeysAtPath<TState>(this State rootState, StatePath path) where TState : State
    {
      var state = rootState.GetAtPath<ObjectState>(path);
      return state?.GetAllKeys<TState>() ?? Enumerable.Empty<string>();
    }

    // Get all field values with the specified path of the specified type
    public static IEnumerable<TState> GetAllValuesAtPath<TState>(this State rootState, StatePath path) where TState : State
    {
      var state = rootState.GetAtPath<ObjectState>(path);
      return state?.GetAllValues<TState>() ?? Enumerable.Empty<TState>();
    }

    // Get items with the specified path of the specified type that match the specified predicate
    public static IEnumerable<TState> GetAllItemsAtPathWhere<TState>(this State rootState, StatePath path, Func<TState, bool> predicate) where TState : State
    {
      var state = rootState.GetAtPath<ListState>(path);
      return state?.GetAllWhere(predicate) ?? Enumerable.Empty<TState>();
    }

    // Get fields with the specified path of the specified type that match the specified predicate
    public static IEnumerable<KeyValuePair<string, TState>> GetAllFieldsAtPathWhere<TState>(this State rootState, StatePath path, Func<TState, bool> predicate) where TState : State
    {
      var state = rootState.GetAtPath<ObjectState>(path);
      return state?.GetAllWhere(predicate) ?? Enumerable.Empty<KeyValuePair<string, TState>>();
    }

    // Get field keys with the specified path of the specified type that match the specified predicate
    public static IEnumerable<string> GetAllKeysAtPathWhere<TState>(this State rootState, StatePath path, Func<TState, bool> predicate) where TState : State
    {
      var state = rootState.GetAtPath<ObjectState>(path);
      return state?.GetAllKeysWhere(predicate) ?? Enumerable.Empty<string>();
    }

    // Get field values with the specified path of the specified type that match the specified predicate
    public static IEnumerable<TState> GetAllValuesAtPathWhere<TState>(this State rootState, StatePath path, Func<TState, bool> predicate) where TState : State
    {
      var state = rootState.GetAtPath<ObjectState>(path);
      return state?.GetAllValuesWhere(predicate) ?? Enumerable.Empty<TState>();
    }
    #endregion

    #region Setting a state from a path
    // Set a state with the specified path in the specified state
    public static void SetAtPath(this State rootState, StatePath path, State state)
    {
      path.EvaluateSetter(rootState, state);
    }

    // Return if a state with the specified path can be set in the specified state
    public static bool TrySetAtPath(this State rootState, StatePath path, State state)
    {
      try
      {
         rootState.SetAtPath(path, state);
        return true;
      }
      catch (StatePathEvaluationException)
      {
        return false;
      }
    }
    #endregion
  }
}