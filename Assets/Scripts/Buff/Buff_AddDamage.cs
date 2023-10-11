using MyGameUtility;
using Role;

namespace Buff {
    public class Buff_AddDamage : BaseBuffWithOwner<BaseRoleCtrl> {
        public Buff_AddDamage(BaseRoleCtrl dataOwner, int layer) : base(dataOwner, layer) { }

        protected override void InitInternal() {
            addDamage();
            OnMerged.AddListener(() => {
                addDamage();
            }, CEC);

            void addDamage() {
                VCC.Clear();
                VCC.Add(DataOwner.RoleCommonInfo.Damage.GetCacheElement(Layer));
            }
        }
    }
}