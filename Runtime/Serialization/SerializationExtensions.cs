using System;
using System.Collections.Generic;

namespace Audune.Persistence
{
  // Class that defines extension methods for serialization
  public static class SerializationExtensions
  {
    #region Serializing enumerables of persistables
    // Serialize all objects in a dictionary to an object state using the specified serializer
    private static ObjectState Serialize<TState, TObject>(this IReadOnlyDictionary<string, TObject> dictionary, Func<TObject, TState> serializer) where TState : State
    {
      var state = new ObjectState();
      if (dictionary == null)
        return state;

      foreach (var e in dictionary)
        state.Set(e.Key, serializer(e.Value));

      return state;
    }

    // Serialize all persistables in a dictionary to an object state
    public static ObjectState Serialize<TState, TSerializable>(this IReadOnlyDictionary<string, TSerializable> dictionary) where TState : State where TSerializable : ISerializable<TState>
    {
      return dictionary.Serialize<TState, TSerializable>(obj => obj.Serialize());
    }

    // Serialize all persistables in a dictionary to an object state with the specified context
    public static ObjectState Serialize<TState, TContext, TSerializable>(this IReadOnlyDictionary<string, TSerializable> dictionary, TContext context) where TState : State where TSerializable : ISerializable<TState, TContext>
    {
      return dictionary.Serialize<TState, TSerializable>(obj => obj.Serialize(context));
    }


    // Deserialize all objects in a dictionary from an object state using the specified deserializer
    private static void Deserialize<TState, TObject>(this IReadOnlyDictionary<string, TObject> dictionary, ObjectState state, Action<TObject, TState> deserializer) where TState : State
    {
      if (dictionary == null)
        return;

      foreach (var e in dictionary)
      {
        if (state.TryGet<TState>(e.Key, out var eState))
          deserializer(e.Value, eState);
      }
    }

    // Deserialize all persistables in a dictionary from an object state
    public static void Deserialize<TState, TDeserializable>(this IReadOnlyDictionary<string, TDeserializable> dictionary, ObjectState state) where TState : State where TDeserializable : IDeserializable<TState>
    {
      dictionary.Deserialize<TState, TDeserializable>(state, (obj, state) => obj.Deserialize(state));
    }

    // Deserialize all persistables in a dictionary from an object state with the specified context
    public static void Deserialize<TState, TContext, TDeserializable>(this IReadOnlyDictionary<string, TDeserializable> dictionary, ObjectState state, TContext context) where TState : State where TDeserializable : IDeserializable<TState, TContext>
    {
      dictionary.Deserialize<TState, TDeserializable>(state, (obj, state) => obj.Deserialize(state, context));
    }
    #endregion
  }
}