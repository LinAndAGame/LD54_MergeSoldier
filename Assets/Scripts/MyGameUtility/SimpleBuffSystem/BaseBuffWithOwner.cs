using System;
using System.Collections.Generic;

namespace MyGameUtility {
    public class BaseBuffWithOwner<T> : BaseBuff {
        protected T DataOwner;

        public BaseBuffWithOwner(T dataOwner, int layer) : base(layer) {
            DataOwner = dataOwner;
        }
    }
}