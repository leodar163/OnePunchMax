using System;
using UnityEngine;

namespace Utils
{
    [Serializable]
    public class UseLimiter
    {
        [SerializeField] private int _maxUses = 0;

        private int _uses = 0;

        public bool Use()
        {
            if (_maxUses == 0) return true;
            return ++_uses <= _maxUses;
        }
    }
}