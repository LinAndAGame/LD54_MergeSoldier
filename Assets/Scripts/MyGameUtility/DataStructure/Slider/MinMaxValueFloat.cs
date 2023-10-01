using System;
using UnityEngine;

namespace MyGameUtility {
    [Serializable]
    public class MinMaxValueFloat {
        public CustomAction OnAnyValueChangedAfter     = new CustomAction();
        public CustomAction OnCurrentValueChangedAfter = new CustomAction();
        public CustomAction OnMinValueChangedAfter     = new CustomAction();
        public CustomAction OnMaxValueChangedAfter     = new CustomAction();
        public CustomAction OnCurValueEqualsMin        = new CustomAction();
        public CustomAction OnCurValueEqualsMax        = new CustomAction();

        private float _Min;
        public float Min {
            get => _Min;
            set {
                _Min    = value;
                Current = Current;
                OnMinValueChangedAfter.Invoke();
                OnAnyValueChangedAfter.Invoke();
            }
        }

        private float _Max;
        public float Max {
            get => _Max;
            set {
                _Max    = value;
                Current = Current;
                OnMaxValueChangedAfter.Invoke();
                OnAnyValueChangedAfter.Invoke();
            }
        }
        
        private float _Current;
        public float Current {
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

        public float Ratio => Current / Max;

        public MinMaxValueFloat(float min, float max, float defaultValue) {
            Min     = min;
            Max     = max;
            Current = defaultValue;
        }

        public bool IsEqualMin => Mathf.Approximately(Current, Min);
        public bool IsEqualMax => Mathf.Approximately(Current, Max);

        public void SetCurrentToMin() {
            Current = Min;
        }

        public void SetCurrentToMax() {
            Current = Max;
        }
    }
}