using System.Collections.Generic;
using MyGameUtility;
using UnityEngine;

namespace Role.Ability {
    public abstract class BaseRoleAbilityData : ScriptableObject{
        public string             Description;
        public List<RoleTypeEnum> AllowedRoleTypes;

        public abstract BaseBuff GetRoleAbility(BaseRoleCtrl applyTo);
    }
}