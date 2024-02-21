namespace Audune.Persistence
{
  // Interface that defines a backend for the persistence system
  public interface IBackend
  {
    // Serialize a state to a stream of bytes
    public abstract byte[] Serialize(State state);

    // Deserialize a state from a stream of bytes
    public abstract State Deserialize(byte[] data);
  }
}