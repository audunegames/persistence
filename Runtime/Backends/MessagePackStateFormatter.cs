using MessagePack;
using MessagePack.Formatters;
using System.Buffers;
using UnityEngine;

namespace Audune.Persistence
{
  // Class that defines a MessagePack formatter for states
  public class MessagePackStateFormatter : IMessagePackFormatter<State>
  {
    // Delegate that defines a MessagePack serializer
    public delegate void Serializer<TValue>(ref MessagePackWriter writer, TValue value, MessagePackSerializerOptions options);

    // Delegate that defines a MessagePack deserializer
    public delegate TValue Deserializer<TValue>(ref MessagePackReader reader, MessagePackSerializerOptions options);


    // Type codes for extensions
    public const sbyte Vector2TypeCode = 0x01;
    public const sbyte Vector3TypeCode = 0x02;
    public const sbyte Vector2IntTypeCode = 0x04;
    public const sbyte Vector3IntTypeCode = 0x05;
    public const sbyte QuaternionTypeCode = 0x07;


    #region Serialization methods
    // Serialize a state
    public void Serialize(ref MessagePackWriter writer, State state, MessagePackSerializerOptions options)
    {
      switch (state)
      {
        case BoolState boolState:
          writer.Write((bool)boolState);
          break;

        case IntState intState:
          writer.Write((int)intState);
          break;

        case LongState longState:
          writer.WriteInt64((long)longState);
          break;

        case FloatState floatState:
          writer.Write((float)floatState);
          break;

        case DoubleState doubleState:
          writer.Write((double)doubleState);
          break;

        case StringState stringState:
          writer.Write((string)stringState);
          break;

        case Vector2State vector2State:
          SerializeExtensionType(ref writer, Vector2TypeCode, (Vector2)vector2State, SerializeVector2, options);
          break;

        case Vector3State vector3State:
          SerializeExtensionType(ref writer,  Vector3TypeCode, (Vector3)vector3State, SerializeVector3, options);
          break;

        case Vector2IntState vector2IntState:
          SerializeExtensionType(ref writer, Vector2IntTypeCode, (Vector2Int)vector2IntState, SerializeVector2Int, options);
          break;

        case Vector3IntState vector3IntState:
          SerializeExtensionType(ref writer, Vector3IntTypeCode, (Vector3Int)vector3IntState, SerializeVector3Int, options);
          break;

        case QuaternionState quaternionState:
          SerializeExtensionType(ref writer, QuaternionTypeCode, (Quaternion)quaternionState, SerializeQuaternion, options);
          break;

        case ListState listState:
          SerizalizeList(ref writer, listState, options);
          break;

        case ObjectState objectState:
          SerializeObject(ref writer, objectState, options);
          break;

        default:
          throw new MessagePackSerializationException($"Invalid state type {state.GetType()}");
      }
    }

    // Serialize an extension type
    private void SerializeExtensionType<TValue>(ref MessagePackWriter writer, sbyte typeCode, TValue value, Serializer<TValue> valueSerializer, MessagePackSerializerOptions options)
    {
      var extensionBuffer = new ArrayBufferWriter<byte>();
      var extensionWriter = writer.Clone(extensionBuffer);

      valueSerializer(ref extensionWriter, value, options);

      extensionWriter.Flush();

      writer.WriteExtensionFormat(new ExtensionResult(typeCode, new ReadOnlySequence<byte>(extensionBuffer.WrittenMemory)));
    }

    // Serialize a list state
    private void SerizalizeList(ref MessagePackWriter writer, ListState listState, MessagePackSerializerOptions options)
    {
      writer.WriteArrayHeader(listState.Count);
      foreach (var item in listState)
      {
        Serialize(ref writer, item, options);
      }
    }

    // Serialize an object state
    private void SerializeObject(ref MessagePackWriter writer, ObjectState objectState, MessagePackSerializerOptions options)
    {
      writer.WriteMapHeader(objectState.Count);
      foreach (var field in objectState)
      {
        writer.Write(field.Key);
        Serialize(ref writer, field.Value, options);
      }
    }

    // Serialize a Vector2
    private void SerializeVector2(ref MessagePackWriter writer, Vector2 value, MessagePackSerializerOptions options)
    {
      writer.Write(value.x);
      writer.Write(value.y);
    }

    // Serialize a Vector3
    private void SerializeVector3(ref MessagePackWriter writer, Vector3 value, MessagePackSerializerOptions options)
    {
      writer.Write(value.x);
      writer.Write(value.y);
      writer.Write(value.z);
    }

    // Serialize a Vector2Int
    private void SerializeVector2Int(ref MessagePackWriter writer, Vector2Int value, MessagePackSerializerOptions options)
    {
      writer.Write(value.x);
      writer.Write(value.y);
    }

    // Serialize a Vector3Int
    private void SerializeVector3Int(ref MessagePackWriter writer, Vector3Int value, MessagePackSerializerOptions options)
    {
      writer.Write(value.x);
      writer.Write(value.y);
      writer.Write(value.z);
    }

    // Serialize a Quaternion
    private void SerializeQuaternion(ref MessagePackWriter writer, Quaternion value, MessagePackSerializerOptions options)
    {
      writer.Write(value.x);
      writer.Write(value.y);
      writer.Write(value.z);
      writer.Write(value.w);
    }
    #endregion

    #region Deserialization methods
    // Deserialize a state
    public State Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
    {
      switch (reader.NextMessagePackType)
      {
        case MessagePackType.Boolean:
          return new BoolState(reader.ReadBoolean());

        case MessagePackType.Integer:
          if (reader.NextCode == MessagePackCode.Int64)
            return new LongState(reader.ReadInt64());
          else
            return new IntState(reader.ReadInt32());

        case MessagePackType.Float:
          if (reader.NextCode == MessagePackCode.Float64)
            return new DoubleState(reader.ReadDouble());
          else
            return new FloatState(reader.ReadSingle());

        case MessagePackType.String:
          return new StringState(reader.ReadString());

        case MessagePackType.Extension:
          return DeserializeExtensionType(ref reader, options);

        case MessagePackType.Array:
          return DeserializeList(ref reader, options);

        case MessagePackType.Map:
          return DeserializeObject(ref reader, options);

        default:
          throw new MessagePackSerializationException($"Unsupported type {MessagePackCode.ToFormatName(reader.NextCode)}");
      }
    }

    // Deserialize an extension type
    private State DeserializeExtensionType(ref MessagePackReader reader, MessagePackSerializerOptions options)
    {
      var header = reader.ReadExtensionFormatHeader();

      return header.TypeCode switch {
        Vector2TypeCode => new Vector2State(DeserializeVector2(ref reader, options)),
        Vector3TypeCode => new Vector3State(DeserializeVector3(ref reader, options)),
        Vector2IntTypeCode => new Vector2IntState(DeserializeVector2Int(ref reader, options)),
        Vector3IntTypeCode => new Vector3IntState(DeserializeVector3Int(ref reader, options)),
        QuaternionTypeCode => new QuaternionState(DeserializeQuaternion(ref reader, options)),
        _ => throw new MessagePackSerializationException($"Unsupported extension type {header.TypeCode}"),
      };
    }
      
    // Deserialize a list state
    private ListState DeserializeList(ref MessagePackReader reader, MessagePackSerializerOptions options)
    {
      var length = reader.ReadArrayHeader();

      var listState = new ListState();
      for (int i = 0; i < length; i++)
      {
        var state = Deserialize(ref reader, options);
        listState.Add(state);
      }
      return listState;
    }

    // Deserialize an enumerable of key-state pairs
    private ObjectState DeserializeObject(ref MessagePackReader reader, MessagePackSerializerOptions options)
    {
      var length = reader.ReadMapHeader();

      var objectState = new ObjectState();
      for (int i = 0; i < length; i++)
      {
        var name = reader.ReadString();
        var state = Deserialize(ref reader, options);
        objectState.Set(name, state);
      }
      return objectState;
    }

    // Deserialize a Vector2
    private Vector2 DeserializeVector2(ref MessagePackReader reader, MessagePackSerializerOptions options)
    {
      return new Vector2(reader.ReadSingle(), reader.ReadSingle());
    }

    // Deserialize a Vector3
    private Vector3 DeserializeVector3(ref MessagePackReader reader, MessagePackSerializerOptions options)
    {
      return new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
    }

    // Deserialize a Vector2Int
    private Vector2Int DeserializeVector2Int(ref MessagePackReader reader, MessagePackSerializerOptions options)
    {
      return new Vector2Int(reader.ReadInt32(), reader.ReadInt32());
    }

    // Deserialize a Vector3Int
    private Vector3Int DeserializeVector3Int(ref MessagePackReader reader, MessagePackSerializerOptions options)
    {
      return new Vector3Int(reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32());
    }

    // Deserialize a Quaternion
    private Quaternion DeserializeQuaternion(ref MessagePackReader reader, MessagePackSerializerOptions options)
    {
      return new Quaternion(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
    }
    #endregion
  }
}
