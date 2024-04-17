using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Random = UnityEngine.Random;

namespace Environment
{
    [CreateAssetMenu(fileName = "Mm_Map", menuName ="Memo/Map")]
    public class MapMemo : ScriptableObject
    {
        [Serializable]
        public struct Camp
        {
            [SerializeField] private float _minDistanceFromLastCamp;
            [SerializeField] private float _maxDistanceFromLastCamp;
            [Space]
            [SerializeField] private AssetReference _campTrigger;
            [SerializeField] private AssetReference _campScene;

            public float DistanceFromLastCamp => Random.Range(_minDistanceFromLastCamp, _maxDistanceFromLastCamp);
            public AssetReference CampTrigger => _campTrigger;
            public AssetReference CampScene => _campScene;
        }

        [Range(0f, 90f)]
        [SerializeField] private float _angleRange;
        [Space]
        [SerializeField] private Camp[] _camps;

        public float Angle => Random.Range(-_angleRange, _angleRange);
        public Camp[] Camps => _camps;
    }
}