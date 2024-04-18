using Behaviors.Attack;
using UnityEngine;

namespace Interactions.Tests
{
    public class TargetTest : MonoBehaviour, ITarget
    {
        public void ReceiveAttack(AttackData data)
        {
            Destroy(gameObject);
        }

        public Vector3 Position => transform.position;
    }
}