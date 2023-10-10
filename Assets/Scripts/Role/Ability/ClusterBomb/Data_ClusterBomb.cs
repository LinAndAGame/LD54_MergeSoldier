using MyGameUtility;

namespace Role.Ability {
    public class Data_ClusterBomb : BaseRoleAbilityData {
        
        
        public override BaseBuff GetRoleAbility(BaseRoleCtrl applyTo) {
            return new Buff_ClusterBomb(this, applyTo as Role_Archer);
        }
    }
}