using Detections;
using UnityEngine;

namespace Behaviors.Attack
{
    public class AttackReceiver : MonoBehaviour, IPositionnable
    {
        public Vector3 Position => transform.position;
        
        public virtual void ReceiveAttack(AttackData attack)
        {
            
        }
    }
}