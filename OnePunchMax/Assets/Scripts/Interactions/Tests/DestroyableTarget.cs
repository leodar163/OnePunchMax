using Behaviors.Attack;
using UnityEngine;

namespace Interactions
{
    public class DestroyableTarget : MonoBehaviour, ITarget
    {
        public void ReceiveAttack(AttackData data)
        {
            gameObject.SetActive(false);
        }

        public Vector3 Position => transform.position;
    }
}