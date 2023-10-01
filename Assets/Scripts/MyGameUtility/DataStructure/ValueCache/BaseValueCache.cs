using System;
using System.Collections.Generic;

namespace MyGameUtility {
    public abstract class BaseValueCache<T> {
        public event Action OnValueChanged;
        
        protected List<ValueCacheElement<T>> ElementCaches = new List<ValueCacheElement<T>>();

        public void RemoveElement(ValueCacheElement<T> element) {
            ElementCaches.Remove(element);
        }

        public abstract T GetValue();

        protected void OnValueChangedInvoke() {
            OnValueChanged?.Invoke();
        }
    }
}