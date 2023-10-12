using System.Collections.Generic;
using System.Linq;
using DamageProcess;
using MyGameUtility;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Role {
    public abstract class BaseRoleCtrl : MonoBehaviour {
        public RoleTypeEnum            RoleType;
        public float                BaseDamage         = 5;
        public float                BaseHp             = 10;
        
        public RoleCom_Animation  RoleAnimationSystemRef;
        public RoleCom_Vfx     RoleComVfxRef;
        public RoleCom_Physical   RolePhysicalSystemRef;
        public RoleCom_UI         RoleUISystemRef;
        public List<IRoleCallBacks> AllRoleSystems;

        public HpInternalSystem HpInternalSystemRef;
        public RoleCommonInfo   RoleCommonInfo;
        public RoleStateInfo    RoleStateInfoRef;
        public RoleEvent        RoleEventRef;
        [ShowInInspector, ReadOnly]
        public BaseBuffSystem       BuffSystemRef = new BuffSystemDefault();

        protected bool _HasInit;

        public virtual void Init() {
            _HasInit = true;
            InitInfos();
            InitCallBacks();
        }

        protected virtual void InitInfos() {
            RoleCommonInfo        = new RoleCommonInfo(this);
            RoleStateInfoRef      = new RoleStateInfo(this);
            RoleEventRef          = new RoleEvent(this);
            HpInternalSystemRef   = new HpInternalSystem(this);
        }

        protected virtual void InitCallBacks() {
            AllRoleSystems = this.GetComponents<IRoleCallBacks>().ToList();
            foreach (var roleSystem in AllRoleSystems) {
                roleSystem.Init();
            }
        }

        private void Update() {
            if (_HasInit == false) {
                return;
            }
            if (RoleStateInfoRef.IsDeath) {
                return;
            }

            foreach (var roleSystem in AllRoleSystems) {
                roleSystem.EffectHandle();
            }
        }

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
            foreach (var roleSystem in AllRoleSystems) {
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
            RoleComVfxRef.PlayBeHitEffect();
            HpInternalSystemRef.RunDamageProcess(damageInfo);
            RoleEventRef.OnBeHitSucceed.Invoke();
        }

        public virtual void DestroySelf() {
            BuffSystemRef.Clear();
            Destroy(this.gameObject);
        }

#if UNITY_EDITOR
        [Button]
        protected virtual void Editor_SetProperties() {
            // 优化
            Editor_InitRoleCom<RoleCom_Animation,BaseRoleCtrl>(ref RoleAnimationSystemRef);
            Editor_InitRoleCom<RoleCom_Vfx,BaseRoleCtrl>(ref RoleComVfxRef);
            Editor_InitRoleCom<RoleCom_Physical,BaseRoleCtrl>(ref RolePhysicalSystemRef);
            Editor_InitRoleCom<RoleCom_UI,BaseRoleCtrl>(ref RoleUISystemRef);
        }

        protected void Editor_InitRoleCom<TCom,TComOwner>(ref TCom property) 
            where TComOwner : BaseRoleCtrl
            where TCom : BaseRoleComponent<TComOwner> {
            
            if (this.GetComponent<TCom>() == null) {
                this.gameObject.AddComponent<TCom>();
            }

            property       = this.GetComponent<TCom>();
            property.Owner = this.GetComponent<TComOwner>();
        }
#endif
    }
}