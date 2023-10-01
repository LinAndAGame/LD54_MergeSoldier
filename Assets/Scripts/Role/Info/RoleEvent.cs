namespace Role {
    public class RoleEvent : BaseRoleInfo {
        public CustomAction OnReadyDeath   = new CustomAction();
        public CustomAction OnDeathFailure = new CustomAction();
        public CustomAction OnDeathSucceed = new CustomAction();
        
        public CustomAction OnReadyBeHit   = new CustomAction();
        public CustomAction OnBeHitFailure = new CustomAction();
        public CustomAction OnBeHitSucceed = new CustomAction();
        
        public RoleEvent(RoleCtrl owner) : base(owner) { }
    }
}