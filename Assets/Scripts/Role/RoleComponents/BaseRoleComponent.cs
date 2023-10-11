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
            if (CanEffectHandle == false) {
                return;
            }
            
            EffectHandleInternal();
        }

        public virtual void Init()                 {}
        public virtual void EffectHandleInternal() {}
        public virtual void DoOnDeath()            {}
    }
}