﻿using MyGameUtility;

namespace Role.Ability {
    public class Buff_LightingChain : BaseRoleAbilityBuffWithData<Data_LightingChain, Role_Archer> {
        public Buff_LightingChain(Data_LightingChain data, Role_Archer owner) : base(data, owner) { }

        protected override void InitInternal() {
            DataOwner.OnBulletInitAfter.AddListener(bullet => {
                bullet.BuffSystemRef.AddBuff(new Buff_LightingChainHit(Data, bullet));
            }, CEC);
        }
    }
}