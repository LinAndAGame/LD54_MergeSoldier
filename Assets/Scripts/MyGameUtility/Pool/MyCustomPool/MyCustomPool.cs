using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.Pool;

namespace MyGameUtility {
    public abstract class MyCustomPool<TCache, TFrom> where TCache : class where TFrom : class {
        public readonly    TFrom              DataFrom;
        public readonly    HashSet<TCache>    AllCurGetCaches = new HashSet<TCache>();
        protected readonly ObjectPool<TCache> Pool;

        public MyCustomPool(TFrom dataFrom) {
            DataFrom = dataFrom;
            Pool     = new ObjectPool<TCache>(() => this.Create(DataFrom), this.DoOnGet, this.DoOnRelease, this.DoOnDestroy);
        }

        public void Preload(int preloadCount) {
            for (int i = 0; i < preloadCount; i++) {
                Pool.Release(this.Get());
            }
        }

        public TCache Get() {
            TCache cache = Pool.Get();
            DoOnGet(cache);
            AllCurGetCaches.Add(cache);
            return cache;
        }

        public PooledObject<TCache> Get(out TCache cache) {
            PooledObject<TCache> pooledObject = Pool.Get(out cache);
            DoOnGet(cache);
            AllCurGetCaches.Add(cache);
            return pooledObject;
        }

        public void Release(TCache cache) {
            if (AllCurGetCaches.Contains(cache) == false) {
                return;
            }

            Pool.Release(cache);
            DoOnRelease(cache);
            AllCurGetCaches.Remove(cache);
        }

        public virtual void Clear() {
            Pool.Clear();
        }

        protected abstract TCache Create(TFrom dataFrom);

        protected virtual void DoOnGet(TCache               cache)                      { }
        protected virtual void DoOnGet(PooledObject<TCache> pooledObject, TCache cache) { }

        protected abstract void DoOnRelease(TCache cache);
        protected abstract void DoOnDestroy(TCache cache);
    }
}