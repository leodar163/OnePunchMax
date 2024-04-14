using System;
using UnityEngine;

namespace Environment
{
    public class CampHandler : MonoBehaviour
    {
        [SerializeField] private int _id;
        public int Id => _id;

        private void Reset()
        {
            GenerateNewId();
        }

        [ContextMenu("Generate New Id")]
        private void GenerateNewId() => _id = Guid.NewGuid().GetHashCode();
    }
}