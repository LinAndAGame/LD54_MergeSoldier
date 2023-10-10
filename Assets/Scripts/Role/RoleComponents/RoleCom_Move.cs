using System.Collections.Generic;
using Fight;
using MyGameUtility;
using UnityEngine;

namespace Role {
    public class RoleCom_Move : BaseRoleComponent<Role_Enemy> {
        public PhysicalEventTrigger PET_Self;
        
        public float MoveSpeed = 0.5f;

        public ValueCacheBool CanMove = new ValueCacheBool(true);

        private   ValueCacheCollection  _VCC_Move         = new ValueCacheCollection();
        protected HashSet<BaseRoleCtrl> AllTouchedPlayers = new HashSet<BaseRoleCtrl>();

        public override void Init() {
            base.Init();
            PET_Self.OnTriggerEnter2DAct.AddListener(other => {
                addToTouchedPlayers(other);
            });

            PET_Self.OnTriggerStay2DAct.AddListener(addToTouchedPlayers);

            PET_Self.OnTriggerExit2DAct.AddListener((other) => {
                BaseRoleCtrl enemy = other.transform.GetComponent<BaseRoleCtrl>();
                AllTouchedPlayers.Remove(enemy);
                if (AllTouchedPlayers.Count == 0) {
                    _VCC_Move.Clear();
                }
            });

            void addToTouchedPlayers(Collider2D other) {
                BaseRole_Player enemy = other.transform.GetComponent<BaseRole_Player>();
                if (enemy.BelongToLocator.IsPreviewLocator) {
                    return;
                }

                if (AllTouchedPlayers.Count == 0) {
                    _VCC_Move.Add(Owner.RoleStateInfoRef.CanMove.GetCacheElement());
                }

                AllTouchedPlayers.Add(enemy);
            }
        }

        public override void EffectHandle() {
            base.EffectHandle();
            if (CanMove.GetValue()) {
                this.transform.position = Vector3.MoveTowards(this.transform.position, this.transform.position + Vector3.down * 10, Time.fixedDeltaTime * MoveSpeed);
            }
        }
    }
}