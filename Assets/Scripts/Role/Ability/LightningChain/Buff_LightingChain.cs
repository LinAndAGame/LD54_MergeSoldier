namespace Role.Ability {
    public class Buff_LightingChain : BaseRoleAbilityBuffWithData<Data_LightingChain, Role_Archer> {
        public Buff_LightingChain(Data_LightingChain data, Role_Archer owner) : base(data, owner) { }

        public override void Init() {
            base.Init();
            Owner.OnBulletInitAfter.AddListener(bullet => {
                bullet.BuffSystemRef.AddBuff(new Buff_LightingChainHit(Data, bullet));
            }, CEC);
        }
    }
}