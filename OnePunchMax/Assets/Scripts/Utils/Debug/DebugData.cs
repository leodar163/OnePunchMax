using System;
using UnityEngine;

namespace Utils.Debug
{
    [Serializable]
    public struct DebugData
    {
        [SerializeField] public bool activateDebug;
        [SerializeField] public Color gizmoColor;
        [SerializeField] public Color debugColor;
    }
}