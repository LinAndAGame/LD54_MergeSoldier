using MyGameUtility;

namespace Role {
    public class RoleStateInfo : BaseRoleInfo {
        public ValueCacheBool CanBeHit = true;
        public ValueCacheBool CanDeath = true;
        public ValueCacheBool CanAttack = true;
        public ValueCacheBool CanMove = true;

        public bool IsDeath;
        
        public RoleStateInfo(BaseRoleCtrl owner) : base(owner) { }
    }
}