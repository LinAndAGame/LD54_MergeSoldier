using System.Collections.Generic;
using System.Linq;
using Map;
using Role;
using UnityEngine;

namespace RoleGroups {
    public class RoleGroup : MonoBehaviour {
        public List<Role_Player> AllReadyPlacedRoleCtrls;

        public void Init() {
            AllReadyPlacedRoleCtrls = this.GetComponentsInChildren<Role_Player>().ToList();
            foreach (Role_Player readyPlacedRoleCtrl in AllReadyPlacedRoleCtrls) {
                readyPlacedRoleCtrl.InitOnRoleGroup();
            }
            Hide();
        }
        
        public void Show() {
            foreach (Role_Player readyPlacedRoleCtrl in AllReadyPlacedRoleCtrls) {
                readyPlacedRoleCtrl.Show();
            }
        }
        
        public void Hide() {
            foreach (Role_Player readyPlacedRoleCtrl in AllReadyPlacedRoleCtrls) {
                readyPlacedRoleCtrl.Hide();
            }
        }

        public void DestroySelf() {
            Destroy(this.gameObject);
        }
    }
}