using System;
using UnityEngine;

namespace Utils.Debug
{
    [Serializable]
    public class DebugData
    {
        [SerializeField] public bool activateVisualDebug = true;
        [SerializeField] public bool activateTextDebug = false;
        [SerializeField] public Color gizmoColor = Color.green;
        [SerializeField] public Color debugColor = Color.white;
    }
}