using System;
using UnityEngine;

    [System.Serializable]
    public struct InterfaceRef<T> : ISerializationCallbackReceiver
    {
        [SerializeField] private UnityEngine.Object reference;

        private UnityEngine.Object _oldreference;
        
        public T Ref => reference != null ? (T)(object)reference : default;

        public void OnBeforeSerialize()
        {
            Validate();
        }

        public void OnAfterDeserialize()
        { }

        private void Validate()
        {
            if (reference == default)
            {
                return;
            }

            if (reference is GameObject gameObject
                && gameObject.TryGetComponent(out T target)){
                reference = target as UnityEngine.Object;
            }   
            if (reference != null && reference is not T)
            {
                Debug.LogError($"{reference.GetType().Name} does not implement {typeof(T)}");
                reference = _oldreference;
            }
            else
            {
                _oldreference = reference;
            }
        }
    }