using System.Collections;
using Bullet;
using MyGameUtility;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Role {
    public class RoleCom_FireBulletAttack : BaseRoleCom_TouchOthers<BaseRoleCtrl> {
        public AssetReference AssetRef_Bullet;
        public float          DefaultCD = 0.5f;

        public ValueCacheFloat CD;

        public override void Init() {
            base.Init();
            CD = DefaultCD;
        }

        public override void EffectHandleInternal() {
            base.EffectHandleInternal();
            var ins = MyPoolSimpleComponent.Get<BulletCtrl>(AssetRef_Bullet);
            ins.Init(Owner);
            
            VCC_CanEffectHandle.Add(CanEffectHandle.GetCacheElement());

            StartCoroutine(delaySetCabEffect());

            IEnumerator delaySetCabEffect() {
                yield return new WaitForSeconds(CD);
                VCC_CanEffectHandle.Clear();
            }
        }
    }
}