using UnityEngine;

namespace MyGameUtility {
    public class MyDefaultComponentPool<T> : MyCustomPool<T, T> where T : Component {
        public MyDefaultComponentPool(T dataFrom) : base(dataFrom) { }

        protected override T Create(T dataFrom) {
            return UnityEngine.Object.Instantiate(dataFrom);
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