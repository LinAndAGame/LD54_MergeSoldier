using Bullet;
using MyGameUtility;
using UnityEngine;

namespace Role.Ability {
    public class Buff_ClusterBombHit : BaseRoleAbilityBuffWithData<Data_ClusterBomb, BulletCtrl> {
        public Buff_ClusterBombHit(Data_ClusterBomb data, BulletCtrl owner) : base(data, owner) { }
        
        protected override void InitInternal() {
            DataOwner.OnHitEnemy.AddListener(() => {
                Debug.Log($"集束炸弹攻击命中！描述：【{Data.Description}】");
            }, CEC);
        }
    }
}