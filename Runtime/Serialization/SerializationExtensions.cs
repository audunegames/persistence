using System.Collections.Generic;

namespace Audune.Persistence
{
  // Class that defines extension methods for serialization
  public static class SerializationExtensions
  {
    #region Serializing enumerables of persistables
    // Serialize all persistables in a dictionary to an object state
    public static ObjectState Serialize<TState, TSerializable>(this IReadOnlyDictionary<string, TSerializable> dictionary) where TState : State where TSerializable : ISerializable<TState>
    {
      var state = new ObjectState();
      if (dictionary == null)
        return state;

      foreach (var e in dictionary)
        state.Set(e.Key, e.Value.Serialize());

      return state;
    }

    // Serialize all persistables in a dictionary to an object state with the provided context
    public static ObjectState Serialize<TState, TContext, TSerializable>(this IReadOnlyDictionary<string, TSerializable> dictionary, TContext context) where TState : State where TSerializable : ISerializable<TState, TContext>
    {
      var state = new ObjectState();
      if (dictionary == null)
        return state;

      foreach (var e in dictionary)
        state.Set(e.Key, e.Value.Serialize(context));

      return state;
    }

    // Deserialize all persistables in a dictionary from an object state
    public static void Deserialize<TState, TDeserializable>(this IReadOnlyDictionary<string, TDeserializable> dictionary, ObjectState state) where TState : State where TDeserializable : IDeserializable<TState>
    {
      if (dictionary == null)
        return;

      foreach (var e in dictionary)
      {
        if (state.TryGet<TState>(e.Key, out var eState))
          e.Value.Deserialize(eState);
      }
    }

    // Deserialize all persistables in a dictionary from an object state
    public static void Deserialize<TState, TContext, TDeserializable>(this IReadOnlyDictionary<string, TDeserializable> dictionary, ObjectState state, TContext context) where TState : State where TDeserializable : IDeserializable<TState, TContext>
    {
      if (dictionary == null)
        return;

      foreach (var e in dictionary)
      {
        if (state.TryGet<TState>(e.Key, out var eState))
          e.Value.Deserialize(eState, context);
      }
    }
    #endregion
  }
}