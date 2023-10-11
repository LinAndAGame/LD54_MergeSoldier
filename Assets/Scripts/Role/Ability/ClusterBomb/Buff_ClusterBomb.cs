using MyGameUtility;

namespace Role.Ability {
    public class Buff_ClusterBomb : BaseRoleAbilityBuffWithData<Data_ClusterBomb, Role_Archer> {
        public Buff_ClusterBomb(Data_ClusterBomb data, Role_Archer owner) : base(data, owner) { }
        
        protected override void InitInternal() {
            DataOwner.OnBulletInitAfter.AddListener(bulletCtrl => {
                DataOwner.BuffSystemRef.AddBuff(new Buff_ClusterBombHit(Data, bulletCtrl));
            } );
        }
    }
}