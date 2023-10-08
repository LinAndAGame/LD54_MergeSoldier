using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace MyGameUtility {
    public class MyPoolSimpleComponent {
        private static Dictionary<Component, object>      _AllPoolCache1 = new Dictionary<Component, object>();
        private static Dictionary<AssetReference, object> _AllPoolCache2 = new Dictionary<AssetReference, object>(new AssetReferenceEquipCompare());

        #region Preload
        
        public static void Preload<T>(int preloadCount, T prefab) where T : Component {
            GetDefaultComponentPool(prefab).Preload(preloadCount);
        }
        public static void Preload<T>(int preloadCount, AssetReference assetRef) where T : Component {
            GetComponentPoolByAssetRef<T>(assetRef).Preload(preloadCount);
        }

        #endregion

        #region AsyncPreload

        public static void AsyncPreload<T>(int preloadCount, AssetReference assetRef) where T : Component {
            GetComponentPoolByAssetRef<T>(assetRef).Preload(preloadCount);
        }

        #endregion

        #region Get
        
        public static T Get<T>(T prefab) where T : Component {
            return GetDefaultComponentPool(prefab).Get();
        }
        public static T Get<T>(AssetReference assetRef) where T : Component {
            return GetComponentPoolByAssetRef<T>(assetRef).Get();
        }

        #endregion

        #region Release

        public static void Release<T>(T cache) where T : Component {
            foreach (object value in _AllPoolCache1.Values) {
                if (value is MyDefaultComponentPool<T> pool) {
                    pool.Release(cache);
                }
            }
            foreach (object value in _AllPoolCache2.Values) {
                if (value is MyComponentPoolByAssetRef<T> pool) {
                    pool.Release(cache);
                }
            }
        }

        #endregion

        private static MyDefaultComponentPool<T> GetDefaultComponentPool<T>(T prefab)  where T : Component {
            if (_AllPoolCache1.ContainsKey(prefab) == false) {
                _AllPoolCache1.Add(prefab,new MyDefaultComponentPool<T>(prefab));
            }

            return (MyDefaultComponentPool<T>) _AllPoolCache1[prefab];
        }
        private static MyComponentPoolByAssetRef<T> GetComponentPoolByAssetRef<T>(AssetReference assetRef)  where T : Component {
            if (_AllPoolCache2.ContainsKey(assetRef) == false) {
                _AllPoolCache2.Add(assetRef,new MyComponentPoolByAssetRef<T>(assetRef));
            }

            return (MyComponentPoolByAssetRef<T>) _AllPoolCache2[assetRef];
        }
    }
}