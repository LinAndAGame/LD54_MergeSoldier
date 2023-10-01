using MyGameUtility;

namespace Role {
    public class RoleStateInfo : BaseRoleInfo {
        public ValueCacheBool CanBeHit = new ValueCacheBool(true);
        public ValueCacheBool CanDeath = new ValueCacheBool(true);
        public ValueCacheBool CanAttack = new ValueCacheBool(true);
        public ValueCacheBool CanMove = new ValueCacheBool(true);

        public bool IsDeath;
        
        public RoleStateInfo(RoleCtrl owner) : base(owner) { }
    }
}