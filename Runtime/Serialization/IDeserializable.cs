namespace Audune.Persistence
{
  // Interface that defines a class as deserializable
  public interface IDeserializable<TState> where TState : State
  {
    // Deserialize the object from a state
    public void Deserialize(TState state);
  }

  // Interface that defines a class as deserializable with the provided context
  public interface IDeserializable<TState, TContext> where TState : State
  {
    // Deserialize the object from a state
    public void Deserialize(TState state, TContext context);
  }
}