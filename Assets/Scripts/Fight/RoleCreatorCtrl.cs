using System.Collections.Generic;
using Role;
using UnityEngine;

namespace Fight {
    public class RoleCreatorCtrl : MonoBehaviour {
        public List<RoleCtrl> AllRolePrefabs;
        
        public List<RoleCtrl> AllPlayerRolePrefabs => AllRolePrefabs.FindAll(data=>data.CompareTag("Player"));

        public RoleCtrl GetRoleByType(RoleTypes roleType) {
            return AllRolePrefabs.Find(data => data.RoleType == roleType);
        }
    }
}