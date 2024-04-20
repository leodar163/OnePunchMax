using System;
using UnityEngine;
using UnityEngine.Events;

namespace Behaviors
{
    [Serializable]
    public class WaterContainer
    {
        [SerializeField] private float _quantity;
        [SerializeField] private float _minQuantity;
        [SerializeField] private float _maxQuantity = 100;
        [Space]
        [SerializeField] private UnityEvent<float> _onQuantityChanged;
        
        public float MinQuantity
        {
            get => _minQuantity;
            set => _minQuantity = value;
        }

        public float MaxQuantity
        {
            get => _maxQuantity;
            set => _maxQuantity = value;
        }

        public float Quantity
        {
            get => _quantity;
            set
            {
                float oldQuantity = _quantity;
                _quantity = value;
                _quantity = Mathf.Clamp(_quantity, _minQuantity, _maxQuantity);
                _onQuantityChanged.Invoke(_quantity - oldQuantity);
            }
        }
        
        public UnityEvent<float> OnQuantityChanged => _onQuantityChanged;
    }
}