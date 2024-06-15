using MessagePack;
using System;
using UnityEngine;

namespace Audune.Persistence
{
  // Class that defines an abstract state
  [MessagePackFormatter(typeof(MessagePackStateFormatter))]
  public abstract class State
  {
    #region Enum methods
    public static StringState FromEnum<TEnum>(TEnum value) where TEnum : struct, Enum
    {
      return new StringState(Enum.GetName(typeof(TEnum), value));
    }

    public static TEnum ToEnum<TEnum>(StringState state, TEnum defaultValue) where TEnum : struct, Enum
    {
      return Enum.TryParse<TEnum>(state, out var value) ? value : defaultValue;
    }
    #endregion

    #region Implicit operators
    public static implicit operator State(bool value) => value ? BoolState.True : BoolState.False;
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