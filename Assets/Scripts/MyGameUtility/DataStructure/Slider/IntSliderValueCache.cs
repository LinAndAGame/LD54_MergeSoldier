using System;
using UnityEngine;

namespace MyGameUtility {
    public class IntSliderValueCache {
        public event Action OnAnyValueChanged;
        public event Action OnMinChanged;
        public event Action OnMaxChanged;
        public event Action OnCurrentValueChanged;
        
        public readonly ValueCacheInt _MinCache;
        public readonly ValueCacheInt _MaxCache;
        public readonly ValueCacheInt _CurrentCache;

        public int  Min        => _MinCache.GetValue();
        public int  Max        => _MaxCache.GetValue();
        public int  Current    => _CurrentCache.GetValue();
        public bool IsEqualMin => Current == Min;
        public bool IsEqualMax => Current == Max;

        public IntSliderValueCache(int min, int max, int defaultValue) {
            _MinCache     = min;
            _MaxCache     = max;
            _CurrentCache = defaultValue;

            _MinCache.OnValueChanged += ()=> {
                OnMinChanged?.Invoke();
                OnAnyValueChanged?.Invoke();
            };
            _MaxCache.OnValueChanged += ()=> {
                OnMaxChanged?.Invoke();
                OnAnyValueChanged?.Invoke();
            };
            _CurrentCache.OnValueChanged += ()=> {
                OnCurrentValueChanged?.Invoke();
                OnAnyValueChanged?.Invoke();
            };

            OnAnyValueChanged += () => {
                int offsetValue = Mathf.Clamp(Current, Min, Max) - Current;
                if (offsetValue != 0) {
                    _CurrentCache.GetCacheElement(offsetValue);
                }
            };
        }

        public void SetToMin() {
            // Current = Max;
        }
    }
}