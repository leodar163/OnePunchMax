using System;
using System.Collections.Generic;
using UnityEngine;
using Utils.Debug;

namespace Detections
{
    [Serializable]
    public abstract class DetectionGeometry
    {
        protected readonly RaycastHit2D[] _occlusionHits = new RaycastHit2D[1];
        
        public virtual void DrawGizmo(Vector3 center, Quaternion rotation) {}

        public virtual List<Collider2D> DetectColliders(Vector3 center, Quaternion rotation, ContactFilter2D filter,
            Collider2D[] unFilteredColliders, bool activateOcclusion = false, ContactFilter2D occlusionFilter = default,
            DebugData debugData = null)
        {
            return null;
        }
    }
}