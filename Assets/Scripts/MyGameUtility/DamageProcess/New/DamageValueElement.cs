using System;
using System.Collections.Generic;

namespace MyGameUtility {
    [Serializable]
    public class DamageValueElement<T> {
        public DamageLabel                   DamageLabel;
        public T                             TotalDamageValue;
        public BaseHealthSingleInfo<T>       Start;
        public BaseHealthSingleInfo<T>       Ends;
        public List<BaseHealthSingleInfo<T>> ForcePath;

        public DamageValueElement( T totalDamageValue) {
            TotalDamageValue           = totalDamageValue;
        }

        public DamageValueElement<T> SetStart(BaseHealthSingleInfo<T> start) {
            Start = start;
            return this;
        }
        public DamageValueElement<T> SetEnd(BaseHealthSingleInfo<T> end) {
            Ends = end;
            return this;
        }
        public DamageValueElement<T> SetForcePath(List<BaseHealthSingleInfo<T>> forcePath) {
            ForcePath = new List<BaseHealthSingleInfo<T>>(forcePath);
            return this;
        }
    }
}