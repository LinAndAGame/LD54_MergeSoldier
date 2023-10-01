using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace MyGameUtility {
    public class DefaultAssetReferenceCustomPoolFunctions : BaseCustomPoolFunctions<AssetReference,DefaultAssetReferenceCustomPoolFunctions,GameObject> {
        public DefaultAssetReferenceCustomPoolFunctions(AssetReference dataFrom) : base(dataFrom) { }
        
        public override GameObject CreateFunc() {
            Debug.Log($"{GetType()}同步加载产生一个缓存");
            var handle = DataFrom.InstantiateAsync();
            handle.WaitForCompletion();
            return handle.Result;
        }

        public override Task<GameObject> AsyncCreateFunc() {
            return DataFrom.InstantiateAsync().Task;
        }

        public override void GetAction(GameObject cache) {
            Debug.Log($"{GetType()}从缓存池中获取一个缓存");
            cache.SetActive(true);
        }

        public override void ReleaseAction(GameObject cache) {
            Debug.Log($"{GetType()}释放一个缓存");
            cache.SetActive(false);
        }

        public override void DestroyAction(GameObject cache) {
            UnityEngine.Object.Destroy(cache);
            DataFrom.ReleaseInstance(cache);
        }
    }
}