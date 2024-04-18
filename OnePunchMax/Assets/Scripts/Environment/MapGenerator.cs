using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Utils;

namespace Environment
{
    public class MapGenerator : MonoBehaviour
    {
        [SerializeField] private MapMemo _mapMemo;

        private void Start()
        {
            // TODO: call this in loading loop
            Async().Forget();
            return;

            async UniTaskVoid Async() => EnvironmentManager.LoaderTriggers = await Generate(_mapMemo);
        }

        public async UniTask<List<SceneLoaderTrigger>> Generate(MapMemo mapMemo)
        {
            float lastAngle = 0;
            List<SceneLoaderTrigger> triggers = new ();
            for (int i = 0; i < mapMemo.Camps.Length; i++)
            {
                MapMemo.Camp camp = mapMemo.Camps[i];
                Vector2 position = GetPosition(triggers, mapMemo, camp.DistanceFromLastCamp, ref lastAngle);
                SceneLoaderTrigger trigger = await GenerateTrigger(camp, position);
                triggers.Add(trigger);
            }
            return triggers;
        }

        private async UniTask<SceneLoaderTrigger> GenerateTrigger(MapMemo.Camp camp, Vector2 position)
        {
            GameObject obj = await camp.CampTrigger.InstantiateAsync(position, Quaternion.identity, transform.root);
            SceneLoaderTrigger trigger = obj.GetComponent<SceneLoaderTrigger>();
            trigger.SceneToLoad = camp.CampScene;
            return trigger;
        }

        private Vector2 GetPosition(List<SceneLoaderTrigger> triggers, MapMemo mapMemo, float distance, ref float angle)
        {
            switch (triggers.Count)
            {
                case 0:
                    angle = 0;
                    return Vector2.zero;

                case 1:
                    angle = Random.Range(0f, 360f);
                    Vector2 direction = Vector2.left.Rotate(angle);
                    return direction * distance;
                    

                default:
                    angle = (angle + mapMemo.Angle) % 360;
                    direction = Vector2.left.Rotate(angle);
                    return (Vector2)triggers[^1].transform.position + direction * distance;
            }
        }
    }
}