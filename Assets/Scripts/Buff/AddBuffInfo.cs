using System;
using MyGameUtility;
using Role;

namespace Buff {
    [Serializable]
    public class AddBuffInfo {
        public BuffTypeEnum AddBuffType;
        public int          Layer;

        public BaseBuff GetBuff(BaseRoleCtrl dataOwner, Func<int, int> getLayerFunc = null) {
            switch (AddBuffType) {
                case BuffTypeEnum.Buff_AddDamage:
                    return new Buff_AddDamage(dataOwner, getLayerFunc == null ? Layer : getLayerFunc.Invoke(Layer));
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}