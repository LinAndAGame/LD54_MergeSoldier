namespace Role.Ability {
    public class Buff_ClusterBomb : BaseRoleAbilityBuffWithData<Data_ClusterBomb, Role_Archer> {
        public Buff_ClusterBomb(Data_ClusterBomb data, Role_Archer owner) : base(data, owner) { }
        
        public override void Init() {
            base.Init();
            Owner.OnBulletInitAfter.AddListener(bulletCtrl => {
                Owner.BuffSystemRef.AddBuff(new Buff_ClusterBombHit(Data, bulletCtrl));
            } );
        }
    }
}