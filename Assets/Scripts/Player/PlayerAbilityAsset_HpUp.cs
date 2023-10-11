using System.Collections.Generic;
using Fight;
using Map;
using Role;
using UnityEngine;

namespace Player {
    [CreateAssetMenu(fileName = "HpUp", menuName = "编辑器资源/HpUp", order = 0)]
    public class PlayerAbilityAsset_HpUp : BasePlayerAbilityAsset {
        public List<RoleTypeEnum> AllRoleTypes = new List<RoleTypeEnum>();
        public float           UpValue      = 2;

        public override void ApplyAbility() {
            base.ApplyAbility();
            var temp = FightCtrl.I.MapCtrlRef.AllCanAttackMapLocators.FindAll(data => data.HasRoleData);
            foreach (MapLocator mapLocator in temp) {
                if (AllRoleTypes.Contains(mapLocator.CurPlacedPlayerRole.RoleType)) {
                    mapLocator.CurPlacedPlayerRole.HpInternalSystemRef.Hp.Max += UpValue;
                    mapLocator.CurPlacedPlayerRole.HpInternalSystemRef.Hp.Current += UpValue;
                }
            }
        }
    }
}