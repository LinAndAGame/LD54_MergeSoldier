using Bullet;
using UnityEngine;

namespace Role.Ability {
    public class Buff_LightingChainHit : BaseRoleAbilityBuffWithData<Data_LightingChain, BulletCtrl> {
        public Buff_LightingChainHit(Data_LightingChain data, BulletCtrl owner) : base(data, owner) { }
        
        public override void Init() {
            base.Init();
            Owner.OnHitEnemy.AddListener(() => {
                Debug.Log("闪电链能力激活！");
            }, CEC);
        }
    }
}