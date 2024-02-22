using Audune.Utils.UnityEditor.Editor;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Audune.Persistence.Editor
{
  // Class that defines a property drawer for a file reference
  [CustomPropertyDrawer(typeof(FileReference))]
  public class FileReferenceDrawer : PropertyDrawer
  {
    // Draw the property
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
      var adapterName = property.FindPropertyRelative("_adapterName");
      var path = property.FindPropertyRelative("_path");

      EditorGUI.BeginProperty(position, label, property);

      var linePosition = position.AlignTop(EditorGUIUtility.singleLineHeight, EditorGUIUtility.standardVerticalSpacing, out position);
      var fieldPosition = EditorGUI.PrefixLabel(linePosition, label);

      var adapterNamePosition = fieldPosition.AlignLeft(Mathf.Max(fieldPosition.width * 0.3f, 80.0f), EditorGUIUtility.standardVerticalSpacing, out fieldPosition);
      adapterName.stringValue = EditorGUI.TextField(adapterNamePosition, adapterName.stringValue);

      var separatorPosition = fieldPosition.AlignLeft(10.0f, EditorGUIUtility.standardVerticalSpacing, out fieldPosition);
      EditorGUI.LabelField(separatorPosition, ":/");

      path.stringValue = EditorGUI.TextField(fieldPosition, path.stringValue);

      var adapter = Object.FindObjectsByType<Adapter>(FindObjectsInactive.Include, FindObjectsSortMode.None).Where(a => a.adapterName == adapterName.stringValue).FirstOrDefault();
      if (adapter != null)
        EditorGUI.HelpBox(position, $"Resolved to adapter \"{adapter.adapterName}\" of type {adapter.GetType()}", MessageType.None);

      EditorGUI.EndProperty();
    }

    // Return the height of the property
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
      var adapterName = property.FindPropertyRelative("_adapterName");

      var adapter = Object.FindObjectsByType<Adapter>(FindObjectsInactive.Include, FindObjectsSortMode.None).Where(a => a.adapterName == adapterName.stringValue).FirstOrDefault();
      if (adapter != null)
        return EditorGUIUtility.singleLineHeight * 2.0f + EditorGUIUtility.standardVerticalSpacing;
      else
        return EditorGUIUtility.singleLineHeight;
    }
  }
}
