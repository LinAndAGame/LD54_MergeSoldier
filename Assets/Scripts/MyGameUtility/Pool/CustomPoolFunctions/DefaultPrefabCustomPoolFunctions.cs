using UnityEngine;

namespace MyGameUtility {
    public class DefaultPrefabCustomPoolFunctions : BaseCustomPoolFunctions<GameObject,DefaultPrefabCustomPoolFunctions,GameObject> {
        public DefaultPrefabCustomPoolFunctions(GameObject dataFrom) : base(dataFrom) { }
        
        public override GameObject CreateFunc() {
            Debug.Log($"{GetType()}创建一个缓存");
            return UnityEngine.Object.Instantiate(DataFrom);
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
        }
    }
}