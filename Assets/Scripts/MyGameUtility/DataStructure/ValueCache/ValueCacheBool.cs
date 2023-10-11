namespace MyGameUtility {
    public class ValueCacheBool : BaseValueCache<bool> {
        private bool _OriginalValue;
        
        private ValueCacheBool(bool originalValue) {
            _OriginalValue = originalValue;
        }
        
        public ValueCacheElement<bool> GetCacheElement() {
            ValueCacheElement<bool> element = new ValueCacheElement<bool>(!_OriginalValue,this);
            ElementCaches.Add(element);
            OnValueChangedInvoke();
            return element;
        }

        public override bool GetValue() {
            return ElementCaches.Count == 0 ? _OriginalValue : !_OriginalValue;
        }

        public static implicit operator bool(ValueCacheBool a) {
            return a.GetValue();
        }

        public static implicit operator ValueCacheBool(bool data) {
            return new ValueCacheBool(data);
        }
    }
}