using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace MyGameUtility {
    public abstract class BaseCustomPoolFunctions<TDataFrom, KChild, TCacheType> : ICustomPoolFunctions<TCacheType>
        where TDataFrom : class
        where TCacheType : class
        where KChild : BaseCustomPoolFunctions<TDataFrom, KChild, TCacheType> {
        private static Dictionary<TDataFrom, ICustomPoolFunctions<TCacheType>> _Cache = new Dictionary<TDataFrom, ICustomPoolFunctions<TCacheType>>();

        public static ICustomPoolFunctions<TCacheType> GetPoolFunctions(TDataFrom dataFrom) {
            if (_Cache.ContainsKey(dataFrom) == false) {
                _Cache.Add(dataFrom, Activator.CreateInstance(typeof(KChild), dataFrom) as KChild);
            }

            return _Cache[dataFrom];
        }

        protected TDataFrom DataFrom;

        protected BaseCustomPoolFunctions(TDataFrom dataFrom) {
            DataFrom = dataFrom;
        }

        public abstract TCacheType CreateFunc();
        public abstract void       GetAction(TCacheType     cache);
        public abstract void       ReleaseAction(TCacheType cache);
        public abstract void       DestroyAction(TCacheType cache);

        public virtual async Task<TCacheType> AsyncCreateFunc() {
            return await new Task<TCacheType>(CreateFunc);
        }
    }
}