using MyGameUtility;

namespace Role {
    public class Role_Soldier : BaseRole_Player {
        public RoleCom_CloseAttackEnemy AttackEnemyCom;

        public override void Init() {
            float maxHp = BaseHp;
            HpInternalSystemRef.Hp = new MinMaxValueFloat(0, maxHp, maxHp);
            base.Init();
        }

        protected override void Editor_SetProperties() {
            base.Editor_SetProperties();
            Editor_InitRoleCom<RoleCom_CloseAttackEnemy, BaseRoleCtrl>(ref AttackEnemyCom);
        }
    }
}