using System.Linq;
using DamageProcess;

namespace Role {
    public class Role_Soldier : Role_Player {
        protected override void Attack() {
            DamageInfo damageInfo = new DamageInfo();
            damageInfo.DamageFrom = this;
            damageInfo.Damage     = this.RoleCommonInfo.Damage.GetValue();
            var touchedEnemiesList = _AllTouchedEnemies.ToList();
            for (int i = touchedEnemiesList.Count - 1; i >= 0; i--) {
                Role_Enemy touchedEnemy = touchedEnemiesList[i];
                touchedEnemy.BeHit(damageInfo);
            }
        }
    }
}