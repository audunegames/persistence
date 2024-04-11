using Audune.Utils.Types;
using MessagePack;

namespace Audune.Persistence
{
  // Class that defines a MessagePack backend for the persistence system
  [TypeDisplayName("MessagePack")]
  public sealed class MessagePackBackend : Backend
  {
    // Serialize a state to a stream of bytes
    public override byte[] Serialize(State state)
    {
      try
      {
        return MessagePackSerializer.Serialize(state);
      }
      catch (MessagePackSerializationException ex)
      {
        throw new BackendException($"Could not serialize the data: {ex.Message}", ex);
      }
    }

    // Deserialize a state from a stream of bytes
    public override State Deserialize(byte[] data)
    {
      try
      {
        return MessagePackSerializer.Deserialize<State>(data);
      }
      catch (MessagePackSerializationException ex)
      {
        throw new BackendException($"Could not deserialize the data: {ex.Message}", ex);
      }
    }
  }
}