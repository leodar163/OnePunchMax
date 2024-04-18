using System.Collections;
using UnityEngine;

namespace Interactions
{
    public interface IHolder
    {
        public FixedJoint2D AnchorPoint { get; }
        public IPickable HoldObject { get; protected set; }
        
        public IHolder HolderSelf { get; }


        //protected IEnumerator _grabRoutine { get; set; }
        
        public void Pick(IPickable pickable)
        {
            if (HoldObject != null || pickable == null) return;

            Transform pickableTransform = pickable.RigidBody.transform;
            Transform anchorTransform = AnchorPoint.transform;

            Vector3 anchorPosition = anchorTransform.position + anchorTransform.rotation * AnchorPoint.connectedAnchor;
            
            HoldObject = pickable;

            HoldObject.RigidBody.position = anchorPosition;
            pickableTransform.right = anchorTransform.right;

            if (AnchorPoint.TryGetComponent(out MonoBehaviour behaviour))
            {
                behaviour.StartCoroutine(GrabRoutine());
            }
            else
            {
                AnchorPoint.connectedBody = HoldObject.RigidBody;
                AnchorPoint.enabled = true;
                HoldObject.OnPicked(this);
            }
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

        private IEnumerator GrabRoutine()
        {
            yield return new WaitForFixedUpdate();
            AnchorPoint.connectedBody = HoldObject.RigidBody;
            AnchorPoint.enabled = true;
            HoldObject.OnPicked(this);
        }
    }
}