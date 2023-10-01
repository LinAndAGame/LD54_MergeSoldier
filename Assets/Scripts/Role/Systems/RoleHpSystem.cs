using DamageProcess;
using MyGameUtility;

namespace Role {
    public class RoleHpSystem : BaseRoleSystem {
        public MinMaxValueFloat Hp = new MinMaxValueFloat(0, 10, 10);

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
            Owner.RoleEffectSystemRef.CreateDamageEffect(damageInfo.Damage, this.transform.position);
        }
    }
}