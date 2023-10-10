namespace Role {
    public abstract class BaseRoleInternalSystem<T> : IRoleCallBacks where T : BaseRoleCtrl {
        protected T Owner;

        protected BaseRoleInternalSystem(T owner) {
            Owner = owner;
        }

        public virtual void Init() {
        }

        public virtual void EffectHandle() {
        }

        public virtual void DoOnDeath() {
        }
    }
}