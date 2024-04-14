using UnityEngine;

namespace Interactions
{
    public interface IHolder
    {
        public FixedJoint2D AnchorPoint { get; }
        public IPickable HoldObject { get; protected set; }
        
        public IHolder HolderSelf { get; }

        public void Pick(IPickable pickable)
        {
            if (HoldObject != null) return;

            HoldObject = pickable;
            AnchorPoint.enabled = true;
            AnchorPoint.connectedBody = pickable.RigidBody;
            pickable.OnPicked(this);
        }

        public void Drop()
        {
            if (HoldObject == null) return;

            AnchorPoint.enabled = false;
            AnchorPoint.connectedBody = null;
            HoldObject.OnDropped(this);
            HoldObject = null;
        }

        public void Switch(IPickable pickable)
        {
            Drop();
            Pick(pickable);
        }
    }
}