using System;
using System.Collections.Generic;

namespace MyGameUtility {
    [Serializable]
    public abstract class BaseHealthSingleInfo<T> {
        public T Value;
        public int Index;

        public BaseHealthSingleInfo<T> Parent;
        
        public abstract T    ReduceValue(T target);

        public abstract bool IsNoRemainingValue();
        
        public void AddChild(BaseHealthSingleInfo<T> child) {
            child.Parent = this;
        }
    }
}