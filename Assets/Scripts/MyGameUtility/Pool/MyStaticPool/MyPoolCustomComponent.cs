using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace MyGameUtility {
    public static class MyPoolCustomComponent {
        private static Dictionary<Component, object>      PoolCache1 = new Dictionary<Component, object>();
        private static Dictionary<AssetReference, object> PoolCache2 = new Dictionary<AssetReference, object>(new AssetReferenceEquipCompare());

#region Preload

        public static void Preload<T>(int preloadCount, AssetReference assetRef, MyDefaultCustomPoolWithAssetRef<T> poolFunctions = null) where T : Component, ICanUsePool {
            GetDefaultPoolWithAssetRefByInterface(assetRef,poolFunctions).Preload(preloadCount);
        }
        public static void Preload<T>(int preloadCount, T prefab, MyCustomPool<T,T> poolFunctions = null) where T : Component, ICanUsePool {
            GetDefaultPoolRefByInterface(prefab, poolFunctions).Preload(preloadCount);
        }

#endregion

#region AsyncPreload
        
        public static void AsyncPreload<T>(int preloadCount, AssetReference assetRef, MyDefaultCustomPoolWithAssetRef<T> poolFunctions = null) where T : Component, ICanUsePool {
            GetDefaultPoolWithAssetRefByInterface(assetRef,poolFunctions).Preload(preloadCount);
        }

#endregion

#region Get

        public static T Get<T>(AssetReference assetRef, MyDefaultCustomPoolWithAssetRef<T> poolFunctions = null) where T : Component, ICanUsePool {
            var pool         = GetDefaultPoolWithAssetRefByInterface(assetRef, poolFunctions);
            var pooledObject = pool.Get(out T result);
            result.PooledObject = pooledObject;
            return result;
        }

        public static T Get<T>(T prefab, MyCustomPool<T,T> poolFunctions = null) where T : Component, ICanUsePool {
            var pool         = GetDefaultPoolRefByInterface(prefab, poolFunctions);
            var pooledObject = pool.Get(out T result);
            result.PooledObject = pooledObject;
            return result;
        }

#endregion

#region Release
        
        public static void Release(ICanUsePool canUsePool) {
            if (canUsePool.PooledObject != null) {
                 canUsePool.PooledObject.Dispose();
                canUsePool.PooledObject = null;
            }
        }

#endregion

        private static MyDefaultCustomPoolWithAssetRef<T> GetDefaultPoolWithAssetRefByInterface<T>(AssetReference assetRef, MyDefaultCustomPoolWithAssetRef<T> poolFunctions)where T : Component, ICanUsePool {
            MyDefaultCustomPoolWithAssetRef<T> usedPoolFunctions = poolFunctions;
            if (usedPoolFunctions == null) {
                if (PoolCache2.ContainsKey(assetRef) == false) {
                    PoolCache2.Add(assetRef,new MyDefaultCustomPoolWithAssetRef<T>(assetRef));
                }
                usedPoolFunctions = (MyDefaultCustomPoolWithAssetRef<T>) PoolCache2[assetRef];
            }

            return usedPoolFunctions;
        }
        private static MyCustomPool<T,T> GetDefaultPoolRefByInterface<T>(T prefab, MyCustomPool<T,T> poolFunctions)where T : Component, ICanUsePool {
            MyCustomPool<T,T> usedPoolFunctions = poolFunctions;
            if (usedPoolFunctions == null) {
                if (PoolCache1.ContainsKey(prefab) == false) {
                    PoolCache1.Add(prefab,new MyCustomPoolByInterface<T>(prefab));
                }
                usedPoolFunctions = (MyCustomPoolByInterface<T>) PoolCache1[prefab];
            }

            return usedPoolFunctions;
        }
    }
}