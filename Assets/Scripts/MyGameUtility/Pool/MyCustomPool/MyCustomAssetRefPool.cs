using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Pool;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace MyGameUtility {
    public class MyCustomAssetRefPool<TCache,TLoad, TFrom>
        where TCache : UnityEngine.Object
        where TLoad : UnityEngine.Object
        where TFrom : AssetReference {
        public readonly    TFrom                                               DataFrom;
        public readonly    Dictionary<TCache,AsyncOperationHandle<GameObject>> AllInstantiateCaches = new Dictionary<TCache, AsyncOperationHandle<GameObject>>();
        public readonly    HashSet<TCache>                                     AllCurGetCaches      = new HashSet<TCache>();
        protected readonly ObjectPool<TCache>                                  Pool;
        private            AsyncOperationHandle<TLoad>                         _AssetRefLoadHandle;

        public MyCustomAssetRefPool(TFrom dataFrom) {
            DataFrom            = dataFrom;
            Pool                = new ObjectPool<TCache>(this.CreateCache, this.DoOnGet, this.DoOnRelease, this.DestroyCache);
            _AssetRefLoadHandle = dataFrom.LoadAssetAsync<TLoad>();
        }


        public void Preload(int preloadCount) {
            for (int i = 0; i < preloadCount; i++) {
                Pool.Release(this.Get());
            }
        }

        public PooledObject<TCache> Get(out TCache cache) {
            var pooledObject = this.Pool.Get(out cache);
            DoOnGet(pooledObject,cache);
            AllCurGetCaches.Add(cache);
            return pooledObject;
        }
        public TCache Get() {
            var cache = this.Pool.Get();
            DoOnGet(cache);
            AllCurGetCaches.Add(cache);
            return cache;
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
            AllInstantiateCaches.Clear();
            AllCurGetCaches.Clear();
            Addressables.Release(_AssetRefLoadHandle);
        }
        

        private TCache CreateCache() {
            if (_AssetRefLoadHandle.IsDone == false) {
                _AssetRefLoadHandle.WaitForCompletion();
            }

            TCache cache = DoOnCreate(out AsyncOperationHandle<GameObject> cacheHandle);
            AllInstantiateCaches.Add(cache,cacheHandle);
            return cache;
        }
        
        
        private void DestroyCache(TCache cache) {
            if (AllInstantiateCaches.ContainsKey(cache) == false) {
                return;
            }

            DoOnDestroy(cache);
            Addressables.ReleaseInstance(AllInstantiateCaches[cache]);
        }

        protected virtual void DoOnGet(PooledObject<TCache> pooledObject, TCache cache) { }

        protected virtual TCache DoOnCreate(out AsyncOperationHandle<GameObject> cacheHandle) {
            cacheHandle = default;
            return null;
        }

        protected virtual void DoOnGet(TCache cache) { }

        protected virtual void DoOnRelease(TCache cache) { }

        protected virtual void DoOnDestroy(TCache cache) { }
    }
}