using System.Collections.Generic;
using DG.Tweening;
using Map;
using MyGameExpand;
using MyGameUtility;
using Unity.VisualScripting;
using UnityEngine;

namespace Role {
    public abstract class Role_Player : RoleCtrl {
        public PhysicalEventTrigger PET_CanAttack;
        public int                  MergeCount = 2;

        private Tweener _MoveToLocatorTweener;

        public MapLocator BelongToLocator { get; private set; }

        protected HashSet<Role_Enemy> _AllTouchedEnemies = new HashSet<Role_Enemy>();

        protected override bool CanAttack() {
            return _AllTouchedEnemies.Count != 0;
        }

        public void InitOnRoleGroup() {
            RoleCommonInfo   = new RoleCommonInfo(this);
            RoleStateInfoRef = new RoleStateInfo(this);
            RoleEventRef     = new RoleEvent(this);
            foreach (BaseRoleSystem roleSystem in AllRoleSystems) {
                roleSystem.InitOnRoleGroup();
            }
        }

        public void Init() {
            float maxHp = BaseHp;
            RoleHpSystemRef.Hp = new MinMaxValueFloat(0, maxHp, maxHp);
            foreach (BaseRoleSystem roleSystem in AllRoleSystems) {
                roleSystem.Init();
            }

            _HasInit = true;
            
            PET_CanAttack.OnTriggerEnter2DAct.AddListener((other) => {
                Role_Enemy enemy = other.transform.GetComponent<Role_Enemy>();
                _AllTouchedEnemies.Add(enemy);
            });
            
            PET_CanAttack.OnTriggerStay2DAct.AddListener((other) => {
                Role_Enemy enemy = other.transform.GetComponent<Role_Enemy>();
                _AllTouchedEnemies.Add(enemy);
            });
            PET_CanAttack.OnTriggerExit2DAct.AddListener((other) => {
                Role_Enemy enemy = other.transform.GetComponent<Role_Enemy>();
                _AllTouchedEnemies.Remove(enemy);
            });
        }

        protected override void FixedUpdate() {
            if (_HasInit == false) {
                return;
            }

            if (RoleStateInfoRef.IsDeath) {
                return;
            }

            if (this.BelongToLocator.IsCanAttackLocator) {
                base.FixedUpdate();
            }
        }

        public void PlaceToLocator(MapLocator mapLocator) {
            if (BelongToLocator != null) {
                BelongToLocator.CurPlacedRoleCtrl = null;
                BelongToLocator                   = null;
            }

            BelongToLocator                   = mapLocator;
            BelongToLocator.CurPlacedRoleCtrl = this;
        }

        public void MoveToLocator(MapLocator mapLocator) {
            PlaceToLocator(mapLocator);
            MoveToBelongToMapLocator();
        }

        private void ForceMoveToMapLocator(MapLocator mapLocator) {
            if (_MoveToLocatorTweener != null) {
                _MoveToLocatorTweener.Kill(false);
            }

            this.transform.SetParent(mapLocator.transform);
            _MoveToLocatorTweener = this.transform.DOLocalMove(Vector3.zero, MoveToLocatorSpeed);
        }

        public void MoveToBelongToMapLocator() {
            ForceMoveToMapLocator(BelongToLocator);
        }

        public void LevelUp(int lv) {
            RoleCommonInfo.Lv += lv;
        }

        public override void DestroySelf() {
            _MoveToLocatorTweener.Kill();
            BelongToLocator.CurPlacedRoleCtrl = null;
            BelongToLocator                   = null;
            Destroy(this.GameObject());
        }
    }
}