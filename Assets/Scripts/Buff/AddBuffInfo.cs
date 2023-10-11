using System;
using MyGameUtility;
using Role;

namespace Buff {
    [Serializable]
    public class AddBuffInfo {
        public BuffTypeEnum AddBuffType;
        public int          Layer;

        public BaseBuff GetBuff(BaseRoleCtrl dataOwner) {
            switch (AddBuffType) {
                case BuffTypeEnum.Buff_AddDamage:
                    return new Buff_AddDamage(dataOwner, Layer);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}