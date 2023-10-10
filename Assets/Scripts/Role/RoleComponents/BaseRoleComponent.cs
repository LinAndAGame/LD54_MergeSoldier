using UnityEngine;

namespace Role {
    public abstract class BaseRoleComponent<T> : MonoBehaviour,IRoleCallBacks where T : BaseRoleCtrl {
        public          T    Owner;
        
        public virtual void Init(){}
        public virtual void InitOnRoleGroup(){}
        public virtual void EffectHandle(){}
        public virtual void DoOnDeath(){}
    }
}