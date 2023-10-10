using Bullet;
using UnityEngine.AddressableAssets;

namespace Role {
    public class Role_Archer : BaseRole_Player {
        public CustomAction<BulletCtrl> OnBulletInitAfter = new CustomAction<BulletCtrl>();
        public RoleCom_FireBulletAttack RoleComFireBulletAttackRef;
        
        public AssetReference AssetRef_Bullet;

        protected override void Editor_SetProperties() {
            base.Editor_SetProperties();
            Editor_InitRoleCom<RoleCom_FireBulletAttack,BaseRoleCtrl>(ref RoleComFireBulletAttackRef);
        }
    }
}