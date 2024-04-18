using System;
using UnityEditor;
using UnityEngine;

namespace Detections.Editor
{
    [CustomEditor(typeof(ColliderDetector))]
    public class ColliderDetectorEditor : UnityEditor.Editor
    {
        

        public override void OnInspectorGUI()
        {
            if (target is not ColliderDetector detector) return;
            
            EditorGUILayout.LabelField("Geometry");
            detector.GeometryType = (DetectionGeometryType)EditorGUILayout.EnumPopup("Geometry Type", detector.GeometryType);

            serializedObject.Update();
            
            switch (detector.GeometryType)
            {
                case DetectionGeometryType.circle:
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("_circle"), true);
                    break;
                case DetectionGeometryType.cone:
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("_cone"), true);
                    break;
                case DetectionGeometryType.none:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            serializedObject.ApplyModifiedProperties();
            GUILayout.Space(10f);
            base.OnInspectorGUI();
        }
    }
}