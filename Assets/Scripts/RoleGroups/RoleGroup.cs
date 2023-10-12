using System.Collections.Generic;
using System.Linq;
using Map;
using Role;
using UnityEngine;

namespace RoleGroups {
    public class RoleGroup : MonoBehaviour {
        public List<BaseRole_Player> AllReadyPlacedRoleCtrls;

        public void Init() {
            AllReadyPlacedRoleCtrls = this.GetComponentsInChildren<BaseRole_Player>().ToList();
            Hide();
        }
        
        public void Show() {
            foreach (BaseRole_Player readyPlacedRoleCtrl in AllReadyPlacedRoleCtrls) {
                readyPlacedRoleCtrl.Show();
            }
        }
        
        public void Hide() {
            foreach (BaseRole_Player readyPlacedRoleCtrl in AllReadyPlacedRoleCtrls) {
                readyPlacedRoleCtrl.Hide();
            }
        }

        public void DestroySelf() {
            Destroy(this.gameObject);
        }
    }
}