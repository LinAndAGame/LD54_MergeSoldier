using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace MyGameUtility {
    public static class MyPoolSimpleGameObject {
        private static readonly Dictionary<GameObject, MyDefaultGameObjectPool>        PoolCache1 = new Dictionary<GameObject, MyDefaultGameObjectPool>();
        private static readonly Dictionary<AssetReference, MyGameObjectPoolByAssetRef> PoolCache2 = new Dictionary<AssetReference, MyGameObjectPoolByAssetRef>(new AssetReferenceEquipCompare());

        public static void Preload(int count, GameObject dataFrom) {
            GetPoolCacheFrom1(dataFrom).Preload(count);
        }
        public static void Preload(int count, AssetReference dataFrom) {
            GetPoolCacheFrom2(dataFrom).Preload(count);
        }

        public static void AsyncPreload(int count, AssetReference dataFrom) {
            GetPoolCacheFrom2(dataFrom).Preload(count);
        }

        public static GameObject Get(GameObject dataFrom) {
            return GetPoolCacheFrom1(dataFrom).Get();
        }
        public static GameObject Get(AssetReference dataFrom) {
            return GetPoolCacheFrom2(dataFrom).Get();
        }

        public static void Release(GameObject cache) {
            foreach (MyDefaultGameObjectPool pool in PoolCache1.Values) {
                pool.Release(cache);
            }
            foreach (MyGameObjectPoolByAssetRef pool in PoolCache2.Values) {
                pool.Release(cache);
            }
        }

        private static MyDefaultGameObjectPool GetPoolCacheFrom1(GameObject dataFrom) {
            if (PoolCache1.ContainsKey(dataFrom) == false) {
                PoolCache1.Add(dataFrom, new MyDefaultGameObjectPool(dataFrom));
            }

            return PoolCache1[dataFrom];
        }
        private static MyGameObjectPoolByAssetRef GetPoolCacheFrom2(AssetReference dataFrom) {
            if (PoolCache2.ContainsKey(dataFrom) == false) {
                PoolCache2.Add(dataFrom, new MyGameObjectPoolByAssetRef(dataFrom));
            }

            return PoolCache2[dataFrom];
        }
    }
}