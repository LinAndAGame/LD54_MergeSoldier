using System.Collections.Generic;
using Role;
using UnityEngine;

namespace Fight {
    public class RoleCreatorCtrl : MonoBehaviour {
        public List<BaseRoleCtrl> AllRolePrefabs;
        
        public List<BaseRoleCtrl> AllPlayerRolePrefabs => AllRolePrefabs.FindAll(data=>data.CompareTag("Player"));

        public BaseRoleCtrl GetRoleByType(RoleTypeEnum roleType) {
            return AllRolePrefabs.Find(data => data.RoleType == roleType);
        }
    }
}