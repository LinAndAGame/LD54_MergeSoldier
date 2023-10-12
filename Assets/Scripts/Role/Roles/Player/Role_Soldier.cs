using MyGameUtility;

namespace Role {
    public class Role_Soldier : BaseRole_Player {
        public RoleCom_CloseAttackEnemy CloseAttackEnemyCom;

        protected override void Editor_SetProperties() {
            base.Editor_SetProperties();
            Editor_InitRoleCom<RoleCom_CloseAttackEnemy, BaseRoleCtrl>(ref CloseAttackEnemyCom);
        }
    }
}