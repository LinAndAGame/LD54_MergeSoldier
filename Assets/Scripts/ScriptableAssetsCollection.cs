using System.Collections.Generic;
using Role.Ability;
using UnityEngine;

[CreateAssetMenu(fileName = "资源集合", menuName = "纯数据资源/资源集合", order = 0)]
public class ScriptableAssetsCollection : ScriptableObject {
    private static ScriptableAssetsCollection _I;

    public static ScriptableAssetsCollection I {
        get {
            if (_I == null) {
                _I = Resources.Load<ScriptableAssetsCollection>("资源集合");
            }

            return _I;
        }
    }

    public List<BaseRoleAbilityData>     AllRoleAbilityDatas;
    public Data_LightingChain LightingChainData;
}