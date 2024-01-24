namespace Audune.Persistence
{
  // Interface that defines a class as persistable
  public interface IPersistable<TState> : ISerializable<TState>, IDeserializable<TState> where TState : State
  {
  }

  // Interface that defines a class as persistable with the provided context
  public interface IPersistable<TState, TContext> : ISerializable<TState, TContext>, IDeserializable<TState, TContext> where TState : State
  {
  }
}