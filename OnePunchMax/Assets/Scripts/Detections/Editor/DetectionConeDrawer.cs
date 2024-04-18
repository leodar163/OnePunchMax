using UnityEditor;
using UnityEngine;

namespace Detections.Editor
{
    [CustomPropertyDrawer(typeof(DetectionCone))]
    public class DetectionConeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            GUILayout.Label(label);
            
            SerializedProperty viewAngle = property.FindPropertyRelative("_viewAngle");
            SerializedProperty viewOffset = property.FindPropertyRelative("_viewOffset");
            SerializedProperty viewRadius = property.FindPropertyRelative("_viewRadius");

            viewAngle.floatValue = EditorGUILayout.Slider( viewAngle.displayName, viewAngle.floatValue, 0, 360);
            viewOffset.floatValue = EditorGUILayout.Slider(viewOffset.displayName, viewOffset.floatValue, -180, 180);
            viewRadius.floatValue = Mathf.Max(0, EditorGUILayout.FloatField(viewRadius.displayName, viewRadius.floatValue));
        }
    }
}