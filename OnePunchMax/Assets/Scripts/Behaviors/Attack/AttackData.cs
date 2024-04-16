using System;
using UnityEngine;

namespace Behaviors.Attack
{
    [Serializable]
    public class AttackData
    {
        [SerializeField] public bool canSetOnFire;
        [SerializeField] public bool canMakeThingsExplode;
        [SerializeField] public bool canBreakBreakable;
        [SerializeField] public float knockBackForce;

        public AttackBehavior attacker;
    }
}