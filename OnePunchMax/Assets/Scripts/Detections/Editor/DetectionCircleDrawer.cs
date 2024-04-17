using UnityEditor;
using UnityEngine;

namespace Detections.Editor
{
    [CustomPropertyDrawer(typeof(DetectionCircle))]
    public class DetectionCircleDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            GUILayout.Label(label);
            
            SerializedProperty radius = property.FindPropertyRelative("_radius");
            SerializedProperty offset = property.FindPropertyRelative("_offset");

            radius.floatValue = Mathf.Max(0, EditorGUILayout.FloatField(radius.displayName, radius.floatValue));
            offset.floatValue = Mathf.Max(0, EditorGUILayout.FloatField(offset.displayName, offset.floatValue));
        }
    }
}