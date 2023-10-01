using System;
using System.Collections;
using System.Collections.Generic;
using DamageProcess;
using MyGameUtility;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Role {
    public abstract class RoleCtrl : MonoBehaviour {
        public RoleTypes            RoleType;
        public float                AttackCD           = 0.3f;
        public float                BaseDamage         = 5;
        public float                BaseHp             = 10;
        public float                MoveToLocatorSpeed = 0.5f;
        public RoleHpSystem         RoleHpSystemRef;
        public RoleAnimationSystem  RoleAnimationSystemRef;
        public RoleEffectSystem     RoleEffectSystemRef;
        public RolePhysicalSystem   RolePhysicalSystemRef;
        public RoleUISystem         RoleUISystemRef;
        public List<BaseRoleSystem> AllRoleSystems;

        public RoleCommonInfo RoleCommonInfo;
        public RoleStateInfo  RoleStateInfoRef;
        public RoleEvent      RoleEventRef;

        protected bool _HasInit;

        private ValueCacheCollection _VCC_AttackCD = new ValueCacheCollection();

        protected virtual void FixedUpdate() {
            if (_HasInit == false) {
                return;
            }
            if (RoleStateInfoRef.IsDeath) {
                return;
            }

            if (RoleStateInfoRef.CanAttack.GetValue() && CanAttack()) {
                this.RoleAnimationSystemRef.PlayAttackAnimation();
                Attack();
                _VCC_AttackCD.Add(RoleStateInfoRef.CanAttack.GetCacheElement());

                StartCoroutine(delaySetCanAttack());

                IEnumerator delaySetCanAttack() {
                    yield return new WaitForSeconds(AttackCD);
                    _VCC_AttackCD.RemoveAll();
                }
            }

            AdditionalFixedUpdate();
        }

        protected virtual  void AdditionalFixedUpdate() { }
        protected abstract bool CanAttack();
        protected abstract void Attack();

        public void Show() {
            this.gameObject.SetActive(true);
        }

        public void Hide() {
            this.gameObject.SetActive(false);
        }

        public void Death() {
            if (RoleStateInfoRef.IsDeath) {
                return;
            }
            
            RoleEventRef.OnReadyDeath.Invoke();
            if (RoleStateInfoRef.CanDeath.GetValue() == false) {
                RoleEventRef.OnDeathFailure.Invoke();
                return;
            }

            RoleStateInfoRef.IsDeath = true;
            foreach (BaseRoleSystem roleSystem in AllRoleSystems) {
                roleSystem.DoOnDeath();
            }
            RoleEventRef.OnDeathSucceed.Invoke();
        }

        public void BeHit(DamageInfo damageInfo) {
            if (RoleStateInfoRef.IsDeath) {
                return;
            }
            
            RoleEventRef.OnReadyBeHit.Invoke();

            if (RoleStateInfoRef.CanBeHit.GetValue() == false) {
                RoleEventRef.OnBeHitFailure.Invoke();
                return;
            }

            RoleAnimationSystemRef.PlayBeHitAnimation();
            RoleEffectSystemRef.PlayBeHitEffect();
            RoleHpSystemRef.RunDamageProcess(damageInfo);
            RoleEventRef.OnBeHitSucceed.Invoke();
        }
        
        public virtual void DestroySelf() { }

#if UNITY_EDITOR
        [Button]
        private void Editor_SetProperties() {
            AllRoleSystems.Clear();
            initSystem(ref RoleHpSystemRef);
            initSystem(ref RoleAnimationSystemRef);
            initSystem(ref RoleEffectSystemRef);
            initSystem(ref RolePhysicalSystemRef);
            initSystem(ref RoleUISystemRef);

            void initSystem<T>(ref T property) where T : BaseRoleSystem {
                if (this.GetComponent<T>() == null) {
                    this.gameObject.AddComponent<T>();
                }

                property       = this.GetComponent<T>();
                property.Owner = this;
                AllRoleSystems.Add(property);
            }
        }
#endif
    }
}