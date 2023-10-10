using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DamageProcess;
using MyGameUtility;
using UnityEngine;

namespace Role {
    public class RoleCom_CloseAttackEnemy : BaseRoleCom_TouchOthers<BaseRoleCtrl> {
        [SerializeField]
        private float DefaultAttackCD;

        public ValueCacheFloat AttackCD { get; set; }

        protected ValueCacheCollection  VCC_AttackCD      = new ValueCacheCollection();
        protected HashSet<BaseRoleCtrl> AllTouchedPlayers = new HashSet<BaseRoleCtrl>();

        public override void Init() {
            base.Init();
            AttackCD = new ValueCacheFloat(DefaultAttackCD);
        }

        public override void EffectHandle() {
            if (CanAttack() == false) {
                return;
            }

            HitEnemiesHandle();
            AttackCDHandle();
            OtherSystemAttackEffectHandle();
        }

        protected virtual bool CanAttack() {
            return Owner.RoleStateInfoRef.CanAttack.GetValue() && AllTouchedPlayers.Count != 0;
        }

        protected virtual void OtherSystemAttackEffectHandle() {
            Owner.RoleAnimationSystemRef.PlayAttackAnimation();
        }

        protected virtual void HitEnemiesHandle() {
            DamageInfo damageInfo        = GetDamageInfo();
            var        touchedPlayerList = AllTouchedPlayers.ToList();
            for (int i = touchedPlayerList.Count - 1; i >= 0; i--) {
                BaseRoleCtrl touchedEnemy = touchedPlayerList[i];
                touchedEnemy.BeHit(damageInfo);
            }
        }

        protected virtual void AttackCDHandle() {
            VCC_AttackCD.Add(Owner.RoleStateInfoRef.CanAttack.GetCacheElement());

            Owner.StartCoroutine(delaySetCanAttack());

            IEnumerator delaySetCanAttack() {
                yield return new WaitForSeconds(AttackCD.GetValue());
                VCC_AttackCD.Clear();
            }
        }

        protected virtual DamageInfo GetDamageInfo() {
            DamageInfo damageInfo = new DamageInfo();
            damageInfo.DamageFrom = Owner;
            damageInfo.Damage     = Owner.RoleCommonInfo.Damage.GetValue();
            return damageInfo;
        }
    }
}