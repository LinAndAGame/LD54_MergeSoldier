using MyGameUtility;
using UnityEngine;

namespace Role.Ability {
    [CreateAssetMenu(fileName = "闪电链Data", menuName = "纯数据资源/RoleAbility/闪电链Data", order = 0)]
    public class Data_LightingChain : BaseRoleAbilityData {
        public GameObject EffectPrefab;

        public override BaseBuff GetRoleAbility(BaseRoleCtrl applyTo) {
            return new Buff_LightingChain(this, applyTo as Role_Archer);
        }
    }
}