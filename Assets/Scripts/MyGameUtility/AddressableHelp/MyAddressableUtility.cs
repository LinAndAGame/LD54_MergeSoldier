using System.Collections.Generic;
using UnityEngine.AddressableAssets;

namespace MyGameUtility {
    public class AssetReferenceEquipCompare : IEqualityComparer<AssetReference> {
        public bool Equals(AssetReference x, AssetReference y) {
            bool flag1 = x == null;
            bool flag2 = y == null;

            if (flag1 ^ flag2) {
                return false;
            }
            else {
                if (flag1 & flag2) {
                    return true;
                }
                else {
                    return x.RuntimeKey.Equals(y.RuntimeKey);
                }
            }
            
        }

        public int GetHashCode(AssetReference obj) {
            return obj.RuntimeKey.GetHashCode();
        }
    }
}