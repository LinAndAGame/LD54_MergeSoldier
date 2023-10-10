using MyGameUtility;

namespace Role.Ability {
    public class BaseRoleAbilityBuffWithData<TData, TOwner> : BaseBuffWithOwner<TOwner> 
        where TData : BaseRoleAbilityData {
        public TData Data;

        public BaseRoleAbilityBuffWithData(TData data, TOwner owner) : base(owner) {
            Data = data;
        }
    }
}