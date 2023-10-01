using Unity.VisualScripting;
using UnityEngine;

namespace Role {
    public class RolePhysicalSystem : BaseRoleSystem {
        public BoxCollider2D BoxCollider2DRef;

        public override void Init() {
            SetEnable(true);
        }

        public override void InitOnRoleGroup() {
            SetEnable(false);
        }

        public override void DoOnDeath() {
            SetEnable(false);
        }

        public void SetEnable(bool enable) {
            BoxCollider2DRef.enabled = enable;
        }
    }
}