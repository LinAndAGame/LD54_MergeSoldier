using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Pool;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace MyGameUtility {
    public class MyDefaultCustomPoolWithAssetRef<T> : MyCustomAssetRefPool<T,GameObject,AssetReference> where T : Component,ICanUsePool {
        public MyDefaultCustomPoolWithAssetRef(AssetReference dataFrom) : base(dataFrom) { }

        protected override void DoOnGet(PooledObject<T> pooledObject, T cache) {
            cache.gameObject.SetActive(true);
            cache.PooledObject = pooledObject;
        }

        protected override T DoOnCreate(out AsyncOperationHandle<GameObject> cacheHandle) {
            cacheHandle = DataFrom.InstantiateAsync();
            if (cacheHandle.IsValid() == false || cacheHandle.IsDone == false) {
                cacheHandle.WaitForCompletion(); 
            }

            return cacheHandle.Result.GetComponent<T>();
        }

        protected override void DoOnGet(T cache) {
            cache.gameObject.SetActive(true);
        }

        protected override void DoOnRelease(T cache) {
            cache.gameObject.SetActive(false);
        }

        protected override void DoOnDestroy(T cache) {
            UnityEngine.Object.Destroy(cache.gameObject);
        }
    }
}