using System;
using System.Collections.Generic;

namespace Audune.Persistence
{
  // Class that defines an accessor property
  internal sealed class StateProperty
  {
    // The getter and setter of the property
    private readonly Func<State> _getter;
    private readonly Action<State> _setter;


    // Return and set the value of the property
    public State value { get => _getter(); set => _setter?.Invoke(value); }


    // Constructor
    public StateProperty(Func<State> getter, Action<State> setter = null)
    {
      _getter = getter;
      _setter = setter;
    }
  }
}