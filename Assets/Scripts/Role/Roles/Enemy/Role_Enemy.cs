using Fight;
using MyGameExpand;
using MyGameUtility;

namespace Role {
    public class Role_Enemy : BaseRoleCtrl {
        public RoleCom_CloseAttackEnemy AttackEnemyCom;
        public RoleCom_Move        MoveCom;

        public override void Init() {
            this.transform.SetParent(FightCtrl.I.EnemyCreatorCtrlRef.transform);
            base.Init();

            // 属性数值更新
            RoleCommonInfo.Damage = BaseDamage;
            RoleCommonInfo.Lv     = FightCtrl.I.Data.CurEnemyLv.Current;
            RoleEventRef.OnDeathSucceed.AddListener(() => { FightCtrl.I.Data.FightProcess.Current++; });

            float maxHp = StandardLevelHp_V1.FindEntity(data => data.f_Lv == RoleCommonInfo.Lv).f_Hp;
            HpInternalSystemRef.Hp = new MinMaxValueFloat(0, maxHp, maxHp);
        }

        protected override void Editor_SetProperties() {
            base.Editor_SetProperties();
            Editor_InitRoleCom<RoleCom_CloseAttackEnemy, BaseRoleCtrl>(ref AttackEnemyCom);
            Editor_InitRoleCom<RoleCom_Move, Role_Enemy>(ref MoveCom);
        }
    }
}