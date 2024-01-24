using MessagePack;
using UnityEngine;

namespace Audune.Persistence
{
  // Class that defines an abstract state
  [MessagePackFormatter(typeof(MessagePackStateFormatter))]
  public abstract class State
  {
    #region Implicit operators
    public static implicit operator State(bool value) => new BoolState(value);
    public static implicit operator State(int value) => new IntState(value);
    public static implicit operator State(long value) => new LongState(value);
    public static implicit operator State(float value) => new FloatState(value);
    public static implicit operator State(double value) => new DoubleState(value);
    public static implicit operator State(string value) => new StringState(value);
    public static implicit operator State(Vector2 value) => new Vector2State(value);
    public static implicit operator State(Vector3 value) => new Vector3State(value);
    public static implicit operator State(Vector2Int value) => new Vector2IntState(value);
    public static implicit operator State(Vector3Int value) => new Vector3IntState(value);
    public static implicit operator State(Quaternion value) => new QuaternionState(value);
    #endregion
  }
}