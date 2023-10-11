using Bullet;
using MyGameUtility;

namespace Role.Ability {
    public class BaseRoleAbilityBuffWithData<TData, TOwner> : BaseBuffWithOwner<TOwner> 
        where TData : BaseRoleAbilityData {
        public TData Data;

        public BaseRoleAbilityBuffWithData(TData data, TOwner owner, int layer = 1) : base(owner,layer) {
            Data = data;
        }
    }
}