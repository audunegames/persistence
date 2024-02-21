using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Audune.Persistence.Editor
{
  // Class that defines a property drawer for a file reference
  [CustomPropertyDrawer(typeof(FileReference))]
  public class FileReferencePropertyDrawer : PropertyDrawer
  {
    // Draw the property
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
      var adapterName = property.FindPropertyRelative("_adapterName");
      var path = property.FindPropertyRelative("_path");

      EditorGUI.BeginProperty(position, label, property);

      var linePosition = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
      var fieldPosition = EditorGUI.PrefixLabel(linePosition, label);

      var adapterNameWidth = Mathf.Max(fieldPosition.width * 0.3f, 80.0f);
      var adapterNamePosition = new Rect(fieldPosition.x, fieldPosition.y, adapterNameWidth, fieldPosition.height);
      adapterName.stringValue = EditorGUI.TextField(adapterNamePosition, adapterName.stringValue);

      var separatorWidth = 10.0f;
      var separatorPosition = new Rect(fieldPosition.x + adapterNameWidth + EditorGUIUtility.standardVerticalSpacing, fieldPosition.y, separatorWidth, fieldPosition.height);
      EditorGUI.LabelField(separatorPosition, ":/");

      var pathPosition = new Rect(fieldPosition.x + adapterNameWidth + separatorWidth + EditorGUIUtility.standardVerticalSpacing * 2.0f, fieldPosition.y, fieldPosition.width - adapterNameWidth - separatorWidth - EditorGUIUtility.standardVerticalSpacing * 2.0f, fieldPosition.height);
      path.stringValue = EditorGUI.TextField(pathPosition, path.stringValue);

      var adapter = Object.FindObjectsByType<Adapter>(FindObjectsInactive.Include, FindObjectsSortMode.None).Where(a => a.adapterName == adapterName.stringValue).FirstOrDefault();
      if (adapter != null)
      {
        linePosition = new Rect(linePosition.x, linePosition.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing, linePosition.width, EditorGUIUtility.singleLineHeight);
        EditorGUI.HelpBox(linePosition, $"Resolved to adapter \"{adapter.adapterName}\" of type {adapter.GetType()}", MessageType.None);
      }

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
