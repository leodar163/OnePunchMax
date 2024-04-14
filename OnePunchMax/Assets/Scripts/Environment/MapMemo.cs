using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Environment
{
    [CreateAssetMenu(fileName = "Mm_Map", menuName ="Memo/Map")]
    public class MapMemo : ScriptableObject
    {
        [SerializeField] private float _minDistanceBetweenCamps;
        [SerializeField] private float _maxDistanceBetweenCamps;
        [Space]
        [Range(0f, 90)]
        [SerializeField] private float _angleRange;
        [Space]
        [SerializeField] private AssetReference[] _camps;

        public float DistanceBetweenCamps => Random.Range(_minDistanceBetweenCamps, _maxDistanceBetweenCamps);
        public float Angle => Random.Range(-_angleRange, _angleRange);
        public AssetReference[] Camps => _camps;

        private void OnValidate()
        {
            UpdateValue(ref _minDistanceBetweenCamps, ref _maxDistanceBetweenCamps, "Distance Between Camps");

            void UpdateValue(ref float minValue, ref float maxValue, string valueName)
            {
                if (minValue > maxValue)
                {
                    if (maxValue == 0)
                    {
                        maxValue = minValue;
                        Debug.LogWarning($"Min {valueName} superior to Max {valueName}:\nMax {valueName} increased to Min {valueName}");
                    }
                    else
                    {
                        minValue = maxValue;
                        Debug.LogWarning($"Min {valueName} superior to Max {valueName}:\nMin {valueName} decreased to Max {valueName}");
                    }
                }
            }
        }
    }
}