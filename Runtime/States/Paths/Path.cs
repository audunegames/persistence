using System;

namespace Audune.Persistence
{
  // Class that defines a path
  public abstract class Path
  {
    // Evaluate the path as a getteron the specified state
    internal abstract State EvaluateGetter(State state);

    // Evaluate the path as a setter on the specified state
    internal abstract void EvaluateSetter(State state, State value);


    // Parse a path
    public static Path Parse(string input)
    {
      var path = default(Path);

      var scanner = new Scanner(input);
      while (!scanner.AtEnd)
      {
        if (scanner.Index > 0 && scanner.Match('['))
        {
          var index = scanner.ReadInt();
          path = new Index(path, index);
          scanner.Consume(']');
        }
        else if (scanner.Index == 0 || scanner.Match('.'))
        {
          var name = scanner.ReadString();
          path = new Access(path, name);
        }
        else
        {
          throw new FormatException($"Found invalid character '{scanner.Next}' at position {scanner.Index}");
        }
      }

      return path;
    }

    // Parse a path using an implicit operator
    public static implicit operator Path(string input) 
    { 
      return Path.Parse(input);
    }


    // Class that defines an indexer path
    internal sealed class Index : Path
    {
      // The parent of the path
      private readonly Path _parent;

      // The index of the path
      private readonly int _index;


      // Constructor
      internal Index(Path parent, int index)
      {
        _parent = parent;
        _index = index;
      }


      // Evaluate the path as a getter on the specified state
      internal override State EvaluateGetter(State state)
      {
        var parentState = _parent?.EvaluateGetter(state) ?? state;

        if (parentState is not ListState listState)
          throw new PathEvaluationException($"Expected state of type {typeof(ObjectState)} but found {state.GetType()}");
        else if (!listState.TryGet<State>(_index, out var itemState))
          throw new PathEvaluationException($"Undefined item with index {_index}");
        else
          return itemState;
      }

      // Evaluate the path as a setter on the specified state
      internal override void EvaluateSetter(State state, State value)
      {
        var parentState = _parent?.EvaluateGetter(state) ?? state;

        if (parentState is not ListState listState)
          throw new PathEvaluationException($"Expected state of type {typeof(ObjectState)} but found {state.GetType()}");
        else
          listState.Set(_index, value);
      }
    }


    // Class that defines an accessor path
    internal sealed class Access : Path
    {
      // The parent of the path
      private readonly Path _parent;

      // The name of the path
      private readonly string _name;


      // Constructor
      internal Access(Path parent, string name)
      {
        _parent = parent;
        _name = name;
      }


      // Evaluate the path as a getter on the specified state
      internal override State EvaluateGetter(State state)
      {
        var parentState = _parent?.EvaluateGetter(state) ?? state;

        if (parentState is not ObjectState objectState)
          throw new PathEvaluationException($"Expected state of type {typeof(ObjectState)} but found {state.GetType()}");
        else if (!objectState.TryGet<State>(_name, out var fieldState))
          throw new PathEvaluationException($"Undefined field with name {_name}");
        else 
          return fieldState;
      }

      // Evaluate the path as a setter on the specified state
      internal override void EvaluateSetter(State state, State value)
      {
        var parentState = _parent?.EvaluateGetter(state) ?? state;

        if (parentState is not ObjectState objectState)
          throw new PathEvaluationException($"Expected state of type {typeof(ObjectState)} but found {state.GetType()}");
        else
          objectState.Set(_name, value);
      }
    }
  }
}