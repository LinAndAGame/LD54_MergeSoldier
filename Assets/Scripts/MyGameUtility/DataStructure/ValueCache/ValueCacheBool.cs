namespace MyGameUtility {
    public class ValueCacheBool : BaseValueCache<bool> {
        private bool _OriginalValue;
        
        public ValueCacheBool(bool originalValue) {
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
    }
}