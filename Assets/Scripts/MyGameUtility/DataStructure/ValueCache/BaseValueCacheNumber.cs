namespace MyGameUtility {
    public abstract class BaseValueCacheNumber<T> : BaseValueCache<T> {
        protected T _OriginalValue;

        public T OriginalValue {
            get => _OriginalValue;
            set {
                _OriginalValue = value;
                OnValueChangedInvoke();
            }
        }


        protected BaseValueCacheNumber(T originalValue) {
            _OriginalValue = originalValue;
        }

        public ValueCacheElement<T> GetCacheElement(T defaultValue) {
            ValueCacheElement<T> element = GenElement(defaultValue);
            ElementCaches.Add(element);
            OnValueChangedInvoke();
            return element;
        }
        protected abstract ValueCacheElement<T> GenElement(T defaultValue);
    }
}