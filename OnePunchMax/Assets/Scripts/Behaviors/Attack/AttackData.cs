using System;
using UnityEngine;

namespace Behaviors.Attack
{
    [Serializable]
    public class AttackData
    {
        [SerializeField] public bool canBurn;
        [SerializeField] public bool canExplode;
        [SerializeField] public bool canBreak;
        [SerializeField] public float knockBackForce;
    }
}