using System;
using UnityEngine;

namespace Interactions
{
    public class ObjectHolder : MonoBehaviour, IHolder
    {
        [SerializeField] private FixedJoint2D _anchorPoint;
        [SerializeField] private Vector2 _holdObjectOffset = new Vector2(0.7f, 0);

        public FixedJoint2D AnchorPoint => _anchorPoint;
        IPickable IHolder.HoldObject { get; set; }

        public IHolder HolderSelf => this;

        private void Update()
        {
            _anchorPoint.anchor = transform.rotation * _holdObjectOffset;
        }
    }
}