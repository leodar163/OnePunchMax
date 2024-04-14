using UnityEngine;

namespace Interactions
{
    public class ObjectHolder : MonoBehaviour, IHolder
    {
        [SerializeField] private FixedJoint2D _anchorPoint;

        public FixedJoint2D AnchorPoint => _anchorPoint;
        IPickable IHolder.HoldObject { get; set; }

        public IHolder HolderSelf => this;
    }
}