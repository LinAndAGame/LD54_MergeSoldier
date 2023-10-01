using UnityEngine;

namespace MyGameUtility {
    public class MyDefaultGameObjectPool : MyCustomPool<GameObject,GameObject> {
        public MyDefaultGameObjectPool(GameObject     dataFrom) : base(dataFrom) { }

        protected override GameObject Create(GameObject dataFrom) {
            return UnityEngine.Object.Instantiate(dataFrom);
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