using System;
using UnityEngine;

namespace MyGameUtility {
    [Serializable]
    public class MinMaxValueInt {
        public CustomAction OnAnyValueChangedAfter     = new CustomAction();
        public CustomAction OnCurrentValueChangedAfter = new CustomAction();
        public CustomAction OnMinValueChangedAfter     = new CustomAction();
        public CustomAction OnMaxValueChangedAfter     = new CustomAction();
        public CustomAction OnCurValueEqualsMin        = new CustomAction();
        public CustomAction OnCurValueEqualsMax        = new CustomAction();

        private int _Min;
        public int Min {
            get => _Min;
            set {
                _Min    = value;
                Current = Current;
                OnMinValueChangedAfter.Invoke();
                OnAnyValueChangedAfter.Invoke();
            }
        }

        private int _Max;
        public int Max {
            get => _Max;
            set {
                _Max    = value;
                Current = Current;
                OnMaxValueChangedAfter.Invoke();
                OnAnyValueChangedAfter.Invoke();
            }
        }
        
        private int _Current;
        public int Current {
            get => _Current;
            set {
                _Current = Mathf.Clamp(value, Min, Max);
                OnCurrentValueChangedAfter.Invoke();
                OnAnyValueChangedAfter.Invoke();
                if (IsEqualMin) {
                    OnCurValueEqualsMin.Invoke();
                }else if (IsEqualMax) {
                    OnCurValueEqualsMax.Invoke();
                }
            }
        }

        public MinMaxValueInt(int min,int max,int defaultValue) {
            Min     = min;
            Max     = max;
            Current = defaultValue;
        }

        public bool IsEqualMin => Current == Min;
        public bool IsEqualMax => Current == Max;

        public void SetCurrentToMin() {
            Current = Min;
        }

        public void SetCurrentToMax() {
            Current = Max;
        }
    }
}