using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace MyGameUtility {
    public class MyComponentPoolByAssetRef<TCache> : MyCustomAssetRefPool<TCache,GameObject,AssetReference> where TCache : Component {
        public MyComponentPoolByAssetRef(AssetReference dataFrom) : base(dataFrom) { }
        
        protected override TCache DoOnCreate(out AsyncOperationHandle<GameObject> cacheHandle) {
            cacheHandle = DataFrom.InstantiateAsync();
            if (cacheHandle.IsValid() == false || cacheHandle.IsDone == false) {
                cacheHandle.WaitForCompletion(); 
            }
            return cacheHandle.Result.GetComponent<TCache>();
        }

        protected override void DoOnGet(TCache cache) {
            cache.gameObject.SetActive(true);
        }

        protected override void DoOnRelease(TCache cache) {
            cache.gameObject.SetActive(false);
        }

        protected override void DoOnDestroy(TCache cache) {
            UnityEngine.Object.Destroy(cache.gameObject);
        }
    }
}