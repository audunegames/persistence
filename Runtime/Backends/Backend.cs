namespace Audune.Persistence
{
  // Base class that defines a backend for the persistence system
  public abstract class Backend
  {
    // Serialize a state to a stream of bytes
    public abstract byte[] Serialize(State state);

    // Deserialize a state from a stream of bytes
    public abstract State Deserialize(byte[] data);
  }
}