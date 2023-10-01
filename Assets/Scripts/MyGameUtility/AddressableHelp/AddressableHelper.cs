using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Pool;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace MyGameUtility {
    public static class AddressableHelper {
        private static Dictionary<PoolKey, IObjectPool<GameObject>> _Pool = new Dictionary<PoolKey, IObjectPool<GameObject>>(new PoolKey.EqualCompare());

        public static T LoadAssetSync<T>(AssetReferenceT<T> assetRef) where T : UnityEngine.Object {
            if (assetRef.OperationHandle.IsValid()) {
                var handle = assetRef.OperationHandle.Convert<T>();
                if (handle.IsDone) {
                    return handle.Result;
                }
                else {
                    handle.WaitForCompletion();
                    return handle.Result;
                }
            }
            else {
                var handle = assetRef.LoadAssetAsync<T>();
                handle.WaitForCompletion();
                return handle.Result;
            }
        }
        
        public static void LoadAssetAsync<T>(AssetReferenceT<T> assetRef, Action<AsyncOperationHandle<T>> onCompleted) where T : UnityEngine.Object {
            if (assetRef.OperationHandle.IsValid()) {
                var handle = assetRef.OperationHandle.Convert<T>();
                if (handle.IsDone) {
                    onCompleted?.Invoke(handle);
                }
                else {
                    handle.Completed += onCompleted;
                }
            }
            else {
                var handle = assetRef.LoadAssetAsync<T>();
                handle.Completed += onCompleted;
            }
        }

        public static void PreloadAsync() {
            
        }

        public static void PreloadSync() {
            
        }


        // public static T InsObjectSyncFromPool<T>(AssetReference assetRef, ICustomPoolFunctions<GameObject> poolFunctions = null) where T : ICanUsePool {
        //     ICustomPoolFunctions<GameObject> usedPoolFunctions = poolFunctions;
        //     if (usedPoolFunctions == null) {
        //         usedPoolFunctions = DefaultAssetReferenceCustomPoolFunctions.GetPoolFunctions(assetRef);
        //     }
        //
        //     PoolKey poolKey = new PoolKey(assetRef, usedPoolFunctions);
        //     if (_Pool.ContainsKey(poolKey) == false) {
        //         _Pool.Add(poolKey, new ObjectPool<GameObject>(usedPoolFunctions.CreateFunc,usedPoolFunctions.GetAction,usedPoolFunctions.ReleaseAction,usedPoolFunctions.DestroyAction));
        //     }
        //
        //     var result = _Pool[poolKey].Get(out GameObject obj);
        //     var com    = obj.GetComponent<T>();
        //     com.PooledObject = result;
        //     return com;
        // }
        //
        // public static void ReleasePooledObjectToPool(ICanUsePool canUsePool) {
        //     if (canUsePool.PooledObject.HasValue) {
        //         ((IDisposable) canUsePool.PooledObject).Dispose();
        //         canUsePool.PooledObject = null;
        //     }
        // }
        
        public struct PoolKey : IEquatable<PoolKey> {
            public AssetReference                   AssetReferenceData;
            public ICustomPoolFunctions<GameObject> PoolFunctions;

            public PoolKey(AssetReference assetReferenceData, ICustomPoolFunctions<GameObject> poolFunctions) {
                AssetReferenceData = assetReferenceData;
                PoolFunctions      = poolFunctions;
            }

            public bool Equals(PoolKey other) {
                return Equals(PoolFunctions, other.PoolFunctions) && Equals(AssetReferenceData, other.AssetReferenceData);
            }

            public override bool Equals(object obj) {
                return obj is PoolKey other && Equals(other);
            }

            public override int GetHashCode() {
                return HashCode.Combine(PoolFunctions, AssetReferenceData);
            }

            public class EqualCompare : EqualityComparer<PoolKey> {
                public override bool Equals(PoolKey x, PoolKey y) {
                    return x.PoolFunctions == y.PoolFunctions && x.AssetReferenceData == y.AssetReferenceData;
                }

                public override int GetHashCode(PoolKey obj) {
                    return HashCode.Combine(obj.PoolFunctions.GetHashCode(), obj.AssetReferenceData.GetHashCode());
                }
            }
        }
    }
}