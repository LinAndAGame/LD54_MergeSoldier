using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace MyGameUtility {
    public class MyGameObjectPoolByAssetRef : MyCustomAssetRefPool<GameObject,GameObject,AssetReference> {
        public MyGameObjectPoolByAssetRef(AssetReference dataFrom) : base(dataFrom) { }

        protected override GameObject DoOnCreate(out AsyncOperationHandle<GameObject> cacheHandle) {
            cacheHandle = DataFrom.InstantiateAsync();
            if (cacheHandle.IsValid() == false || cacheHandle.IsDone == false) {
                cacheHandle.WaitForCompletion(); 
            }

            return cacheHandle.Result;
        }

        protected override void DoOnGet(GameObject cache) {
            cache.gameObject.SetActive(true);
        }

        protected override void DoOnRelease(GameObject cache) {
            cache.gameObject.SetActive(false);
        }

        protected override void DoOnDestroy(GameObject cache) {
            UnityEngine.Object.Destroy(cache);
        }
    }
}