using System;
using UnityEngine;

namespace Role {
    public class BaseRoleSystem : MonoBehaviour{
        public RoleCtrl Owner;

        public virtual void Init()            { }
        public virtual void InitOnRoleGroup() { }
        
        public virtual void DoOnDeath(){ }
    }
}