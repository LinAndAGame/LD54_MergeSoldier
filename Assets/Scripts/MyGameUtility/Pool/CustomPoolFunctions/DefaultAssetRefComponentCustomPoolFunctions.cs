using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace MyGameUtility {
    public class DefaultAssetRefComponentCustomPoolFunctions<T> : BaseCustomPoolFunctions<AssetReference,DefaultAssetRefComponentCustomPoolFunctions<T>,T> where T : Component {
        public DefaultAssetRefComponentCustomPoolFunctions(AssetReference dataFrom) : base(dataFrom) { }
        
        public override T CreateFunc() {
            Debug.Log("同步加载产生一个缓存");
            var handle = DataFrom.InstantiateAsync();
            handle.WaitForCompletion();
            return handle.Result.GetComponent<T>();
        }

        public override async Task<T> AsyncCreateFunc() {
            Debug.Log("异步加载产生一个缓存");
            GameObject result = await DataFrom.InstantiateAsync().Task;
            return result.GetComponent<T>();
        }

        public override void GetAction(T cache) {
            cache.gameObject.SetActive(true);
        }

        public override void ReleaseAction(T cache) {
            cache.gameObject.SetActive(false);
        }

        public override void DestroyAction(T cache) {
            UnityEngine.Object.Destroy(cache);
            DataFrom.ReleaseInstance(cache.gameObject);
        }
    }
}