using System;

namespace MyGameUtility {
    public class ValueCacheElement<T> : IValueCacheElement {
        public Action OnValueChanged;

        private T _Value;
        public T Value {
            get => _Value;
            set {
                _Value = value;
                OnValueChanged?.Invoke();
            }
        }

        private BaseValueCache<T> _FromCache;

        public ValueCacheElement(T defaultValue,BaseValueCache<T> fromCache) {
            Value      = defaultValue;
            _FromCache = fromCache;
        }

        public void Remove() {
            Value = default;
            _FromCache.RemoveElement(this);
            _FromCache = null;
        }
    }
}