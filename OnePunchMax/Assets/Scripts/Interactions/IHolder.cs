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
            if (HoldObject != null || pickable == null) return;

            Transform pickableTransform = pickable.RigidBody.transform;
            Transform anchorTransform = AnchorPoint.transform;

            Vector3 anchorPosition = anchorTransform.position + anchorTransform.rotation * AnchorPoint.connectedAnchor;
            
            HoldObject = pickable;

            pickableTransform.position = anchorPosition;
            pickableTransform.right = anchorTransform.right;
            
            AnchorPoint.connectedBody = pickable.RigidBody;
            AnchorPoint.enabled = true;
            
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