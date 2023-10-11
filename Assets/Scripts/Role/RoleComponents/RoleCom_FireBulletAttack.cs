using Bullet;
using MyGameUtility;
using UnityEngine.AddressableAssets;

namespace Role {
    public class RoleCom_FireBulletAttack : BaseRoleCom_TouchOthers<BaseRoleCtrl> {
        public AssetReference AssetRef_Bullet;

        public override void EffectHandleInternal() {
            base.EffectHandleInternal();
            var ins = MyPoolSimpleComponent.Get<BulletCtrl>(AssetRef_Bullet);
            ins.Init(Owner);
        }
    }
}