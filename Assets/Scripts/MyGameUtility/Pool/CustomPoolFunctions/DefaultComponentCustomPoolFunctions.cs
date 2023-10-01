using UnityEngine;

namespace MyGameUtility {
    public class DefaultComponentCustomPoolFunctions<T> : BaseCustomPoolFunctions<T,DefaultComponentCustomPoolFunctions<T>,T> where T : Component {
        public DefaultComponentCustomPoolFunctions(T dataFrom) : base(dataFrom) { }
        
        public override T CreateFunc() {
            Debug.Log("同步加载产生一个缓存");
            return UnityEngine.Object.Instantiate(DataFrom);
        }

        public override void GetAction(T cache) {
            cache.gameObject.SetActive(true);
        }

        public override void ReleaseAction(T cache) {
            cache.gameObject.SetActive(false);
        }

        public override void DestroyAction(T cache) {
            UnityEngine.Object.Destroy(cache);
        }
    }
}