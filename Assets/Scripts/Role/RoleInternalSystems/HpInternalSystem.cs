using DamageProcess;
using MyGameUtility;

namespace Role {
    public class HpInternalSystem : BaseRoleInternalSystem<BaseRoleCtrl> {
        public MinMaxValueFloat Hp = new MinMaxValueFloat(0, 10, 10);

        public HpInternalSystem(BaseRoleCtrl owner) : base(owner) { }

        public override void Init() {
            Owner.RoleUISystemRef.RefreshHp(Hp);
            Hp.OnAnyValueChangedAfter.AddListener(() => {
                Owner.RoleUISystemRef.RefreshHp(Hp);
            });
            Hp.OnCurValueEqualsMin.AddListener(() => {
                Owner.Death();
            });
        }

        public void RunDamageProcess(DamageInfo damageInfo) {
            Hp.Current -= damageInfo.Damage;
            Owner.RoleComVfxRef.CreateDamageEffect(damageInfo.Damage, Owner.transform.position);
        }
    }
}