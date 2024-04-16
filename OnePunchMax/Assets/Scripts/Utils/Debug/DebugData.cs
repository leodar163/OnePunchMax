using System;
using UnityEngine;

namespace Utils.Debug
{
    [Serializable]
    public class DebugData
    {
        [SerializeField] public bool activateDebug = true;
        [SerializeField] public Color gizmoColor = Color.green;
        [SerializeField] public Color debugColor = Color.white;
    }
}