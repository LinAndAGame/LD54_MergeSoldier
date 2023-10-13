using MyGameUtility;
using UnityEngine;

namespace Role {
    public abstract class BaseRoleComponent<TOwner> : MonoBehaviour,IRoleCallBacks where TOwner : BaseRoleCtrl {
        public TOwner Owner;

        protected ValueCacheBool       CanEffectHandle     = true;
        protected ValueCacheCollection VCC_CanEffectHandle = new ValueCacheCollection();

        protected ValueCacheCollection  VCC = new ValueCacheCollection();
        protected CustomEventCollection CEC = new CustomEventCollection();
        protected BuffCollection        BC  = new BuffCollection();


        public void EffectHandle() {
            if (CanEffectHandleInternal() == false) {
                return;
            }
            
            EffectHandleInternal();
        }

        protected virtual bool CanEffectHandleInternal() {
            return CanEffectHandle;
        }

        public virtual void Init()                 {}
        public virtual void EffectHandleInternal() {}

        public virtual void DoOnMouseEnter() { }
        public virtual void DoOnMouseExit()  { }

        public virtual void DoOnDeath() {
            VCC.Clear();
            CEC.Clear();
            BC.Clear();
        }
    }
}