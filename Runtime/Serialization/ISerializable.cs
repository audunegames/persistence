namespace Audune.Persistence
{
  // Interface that defines a class as serializable
  public interface ISerializable<TState> where TState : State
  {
    // Serialize the object to a state
    public TState Serialize();
  }

  // Interface that defines a class as serializable with the provided context
  public interface ISerializable<TState, TContext> where TState : State
  {
    // Serialize the object to a state
    public TState Serialize(TContext context);
  }
}