using System.Collections;
using Bullet;
using Fight;
using MyGameExpand;
using MyGameUtility;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Role {
    public class Role_Archer : Role_Player {
        public AssetReference AssetRef_Bullet;

        protected override void Attack() {
            var bulletIns = MyPoolSimpleComponent.Get<BulletCtrl>(AssetRef_Bullet);
            bulletIns.Init(this);
        }
    }
}