using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Test {
    public class TestAddressable : MonoBehaviour {
        public List<AssetReference> AllAssetRefs;

        [Button]
        private void OutputHashCode() {
            Debug.Log($"AllAssetRefs is Equips = {AllAssetRefs.All(data=>AllAssetRefs.All(data2=>data2.Equals(data)))}");
            foreach (var assetReference in AllAssetRefs) {
                Debug.Log(assetReference.GetHashCode());
            }
        }
    }
}