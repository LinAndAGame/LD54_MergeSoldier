using DamageProcess;
using MyGameUtility;
using UnityEngine;

namespace Role {
    public class RoleCommonInfo : BaseRoleInfo {
        public ValueCacheFloat Damage;

        private int _Lv = 1;

        public int Lv {
            get => _Lv;
            set {
                _Lv = value;
                Owner.RoleUISystemRef.RefreshLv(_Lv);
                // _Lv = Mathf.Clamp(_Lv, 1, 100);
            }
        }

        public RoleCommonInfo(RoleCtrl owner) : base(owner) {
            Damage = new ValueCacheFloat(Owner.BaseDamage);
        }
    }
}