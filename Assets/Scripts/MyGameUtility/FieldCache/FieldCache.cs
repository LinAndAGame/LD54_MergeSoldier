/*
 * 功能 ： 一个实例中的变量需要初始化操作，在调用时只会进行一次初始化，做一个缓存，应用场景 ： 各种Find函数
 */

using System;

namespace MyGameUtility.FieldCache {
    public class FieldCache<T> {
        private T       _Cache;
        private bool    _HasInit;
        private Func<T> _FuncGetCache;

        public T Cache {
            get {
                if (_HasInit == false) {
                    _HasInit = true;
                    _Cache   = _FuncGetCache.Invoke();
                }

                return _Cache;
            }
        }

        public FieldCache(Func<T> funcGetCache) {
            _FuncGetCache = funcGetCache;
        }

        public void ClearCache() {
            _HasInit = false;
        }
    }
}