using System.Collections.Generic;

namespace MyGameUtility {
    public class ValueCacheCollection {
        private List<IValueCacheElement> _AllValueCacheElements = new List<IValueCacheElement>();

        public void Add(IValueCacheElement valueCacheElement) {
            _AllValueCacheElements.Add(valueCacheElement);
        }

        public void Clear() {
            foreach (IValueCacheElement valueCacheElement in _AllValueCacheElements) {
                valueCacheElement.Remove();
            }
            _AllValueCacheElements.Clear();
        }
    }
}