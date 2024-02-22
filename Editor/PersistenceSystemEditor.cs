using Audune.Utils.UnityEditor.Editor;
using System;
using UnityEditor;
using UnityEngine;

namespace Audune.Persistence.Editor
{
  // Class that defines an editor for the persistence system
  [CustomEditor(typeof(PersistenceSystem))]
  public class PersistenceSystemEditor : UnityEditor.Editor
  {
    // Return the target object of the editor
    public new PersistenceSystem target => serializedObject.targetObject as PersistenceSystem;


    // Draw the inspector GUI
    public override void OnInspectorGUI()
    {
      base.OnInspectorGUI();

      EditorGUILayout.Space();
      EditorGUILayout.LabelField("Components", EditorStyles.boldLabel);

      var addAdapterPosition = EditorGUILayout.GetControlRect();
      addAdapterPosition = EditorGUI.PrefixLabel(addAdapterPosition, new GUIContent("Add Adapter"));
      EditorGUIExtensions.GenericMenuDropdown(addAdapterPosition, new GUIContent("(select)"), typeof(Adapter).CreateGenericMenuForChildTypes(Utils.UnityEditor.TypeDisplayOptions.None, null, AddAdapterComponent));
    }


    // Add a new adapter component to the persistence system
    private void AddAdapterComponent(Type adapterType)
    {
      target.gameObject.AddComponent(adapterType);
    }
  }
}