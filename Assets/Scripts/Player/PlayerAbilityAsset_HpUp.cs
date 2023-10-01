using System.Collections.Generic;
using Fight;
using Map;
using Role;
using UnityEngine;

namespace Player {
    [CreateAssetMenu(fileName = "HpUp", menuName = "编辑器资源/HpUp", order = 0)]
    public class PlayerAbilityAsset_HpUp : BasePlayerAbilityAsset {
        public List<RoleTypes> AllRoleTypes = new List<RoleTypes>();
        public float           UpValue      = 2;

        public override void ApplyAbility() {
            base.ApplyAbility();
            var temp = FightCtrl.I.MapCtrlRef.AllPlayerCanAttackMapLocators.FindAll(data => data.HasRoleData);
            foreach (MapLocator mapLocator in temp) {
                if (AllRoleTypes.Contains(mapLocator.CurPlacedRoleCtrl.RoleType)) {
                    mapLocator.CurPlacedRoleCtrl.RoleHpSystemRef.Hp.Max += UpValue;
                    mapLocator.CurPlacedRoleCtrl.RoleHpSystemRef.Hp.Current += UpValue;
                }
            }
        }
    }
}