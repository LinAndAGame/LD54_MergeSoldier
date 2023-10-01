using UnityEngine;
using UnityEngine.Pool;

namespace MyGameUtility {
    public class MyCustomPoolByInterface<T> : MyCustomPool<T,T> where T : Component, ICanUsePool {
        public MyCustomPoolByInterface(T      dataFrom) : base(dataFrom) { }
        
        protected override T Create(T dataFrom) {
            return UnityEngine.Object.Instantiate(dataFrom);
        }

        protected override void DoOnGet(PooledObject<T> pooledObject, T cache) {
            cache.PooledObject = pooledObject;
            cache.gameObject.SetActive(true);
        }

        protected override void DoOnRelease(T cache) {
            cache.PooledObject = null;
            cache.gameObject.SetActive(false);
        }

        protected override void DoOnDestroy(T cache) {
            UnityEngine.Object.Destroy(cache.gameObject);
        }
    }
}