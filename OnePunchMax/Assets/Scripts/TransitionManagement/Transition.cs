using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace TransitionManagement
{
    public abstract class Transition : ScriptableObject
    {
        public static Canvas Canvas
        {
            get
            {
                if (_canvas == null)
                {
                    GameObject container = new GameObject("Canvas - Transition", typeof(GraphicRaycaster));
                    DontDestroyOnLoad(container);

                    _canvas = container.GetComponent<Canvas>();
                    _canvas.sortingOrder = 20;
                    _canvas.renderMode = RenderMode.ScreenSpaceOverlay;

                    CanvasScaler scaler = container.AddComponent<CanvasScaler>();
                    if (scaler)
                    {
                        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                        scaler.referenceResolution = new Vector2(1080, 1920);
                    }
                }
                return _canvas;
            }
        }
        private static Canvas _canvas;

        public static Image Image
        {
            get
            {
                if (_image == null)
                {
                    _image = new GameObject("Image").AddComponent<Image>();
                    _image.color = Color.clear;
                    RectTransform rectTransform = _image.rectTransform;
                    rectTransform.SetParent(Canvas.transform, false);
                    rectTransform.localScale = Vector3.one;
                    rectTransform.anchorMin = Vector2.zero;
                    rectTransform.anchorMax = Vector2.one;
                    rectTransform.offsetMin = Vector2.zero;
                    rectTransform.offsetMax = Vector2.zero;
                }
                return _image;
            }
        }
        private static Image _image;

        public abstract UniTask Play(TransitionMode mode = TransitionMode.Complete);

        public abstract float Duration(TransitionMode mode = TransitionMode.Complete);
    }
}