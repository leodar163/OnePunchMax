using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Utils;

namespace Environment
{
    public class MapGenerator : MonoBehaviour
    {
        [SerializeField] private AssetReference _triggerPrefab;
        [SerializeField] private MapMemo _mapMemo;

        private void Start()
        {
            // TODO: call this in loading loop
            Generate(_mapMemo).Forget();
        }

        public async UniTask<List<SceneLoaderTrigger>> Generate(MapMemo mapMemo)
        {
            float lastAngle = 0;
            List<SceneLoaderTrigger> triggers = new ();
            for (int i = 0; i < mapMemo.Camps.Length; i++)
            {
                GetPosition(triggers, mapMemo, out Vector2 position, ref lastAngle);
                SceneLoaderTrigger trigger = await GenerateTrigger(mapMemo.Camps[i], position);
                triggers.Add(trigger);
            }
            return triggers;
        }

        private async UniTask<SceneLoaderTrigger> GenerateTrigger(AssetReference scene, Vector2 position)
        {
            GameObject obj = await _triggerPrefab.InstantiateAsync(position, Quaternion.identity, transform.root);
            SceneLoaderTrigger trigger = obj.GetComponent<SceneLoaderTrigger>();
            trigger.SceneToLoad = scene;
            return trigger;
        }

        private void GetPosition(List<SceneLoaderTrigger> triggers, MapMemo mapMemo, out Vector2 position, ref float angle)
        {
            switch (triggers.Count)
            {
                case 0:
                    position = Vector2.zero;
                    angle = 0;
                    return;

                case 1:
                    angle = Random.Range(0f, 360f);
                    Vector2 direction = Vector2.left.Rotate(angle);
                    position = direction * mapMemo.DistanceBetweenCamps;
                    return;

                default:
                    angle = (angle + mapMemo.Angle) % 360;
                    direction = Vector2.left.Rotate(angle);
                    position = (Vector2)triggers[^1].transform.position + direction * mapMemo.DistanceBetweenCamps;
                    return;
            }
        }
    }
}