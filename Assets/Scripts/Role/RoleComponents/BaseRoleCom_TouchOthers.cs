using System.Collections.Generic;
using MyGameUtility;
using UnityEngine;

namespace Role {
    public class BaseRoleCom_TouchOthers<T> : BaseRoleComponent<T> where T : BaseRoleCtrl {
        [SerializeField]
        private PhysicalEventTrigger PET_Self;
        
        protected HashSet<BaseRoleCtrl> AllTouchedPlayers = new HashSet<BaseRoleCtrl>();

        public override void Init() {
            base.Init();
            PET_Self.OnTriggerEnter2DAct.AddListener(addToTouchedPlayers);
            PET_Self.OnTriggerStay2DAct.AddListener(addToTouchedPlayers);

            PET_Self.OnTriggerExit2DAct.AddListener(other => {
                if (CanRemoveFromTouched(other) == false) {
                    return;
                }
                
                BaseRoleCtrl enemy = other.transform.GetComponent<BaseRoleCtrl>();
                AllTouchedPlayers.Remove(enemy);
            });

            void addToTouchedPlayers(Collider2D other) {
                if (CanAddToTouched(other) == false) {
                    return;
                }
                
                BaseRoleCtrl enemy = other.transform.GetComponent<BaseRoleCtrl>();
                AllTouchedPlayers.Add(enemy);
            }
        }

        protected override bool CanEffectHandleInternal() {
            return CanEffectHandle && Owner.RoleStateInfoRef.CanAttack.GetValue() && AllTouchedPlayers.Count != 0;
        }

        protected virtual bool CanAddToTouched(Collider2D other) {
            return true;
        }

        protected virtual bool CanRemoveFromTouched(Collider2D other) {
            return true;
        }
    }
}