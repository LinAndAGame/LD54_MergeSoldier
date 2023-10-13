using DG.Tweening;
using Map;
using MyGameUtility;
using UnityEngine;

namespace Role {
    public abstract class BaseRole_Player : BaseRoleCtrl {
        public CustomAction<MapLocator> OnMoveToOtherLocator = new CustomAction<MapLocator>();
        public CustomAction<MapLocator> OnLeftLocator = new CustomAction<MapLocator>();
        
        public int   MergeCount         = 2;
        public float MoveToLocatorSpeed = 0.5f;

        private Tweener _MoveToLocatorTweener;

        public MapLocator BelongToLocator { get; private set; }

        protected override void InitInfos() {
            base.InitInfos();
            RoleCommonInfo.Damage = BaseDamage;
            float maxHp = BaseHp;
            HpInternalSystemRef.Hp = new MinMaxValueFloat(0, maxHp, maxHp);
        }

        public void SetNotPlacedOnLocator() {
            if (BelongToLocator != null) {
                OnLeftLocator.Invoke(BelongToLocator);
                BelongToLocator.CurPlacedPlayerRole = null;
                BelongToLocator                     = null;
            }
        }

        public void PlaceToLocator(MapLocator mapLocator) {
            SetNotPlacedOnLocator();

            BelongToLocator                   = mapLocator;
            BelongToLocator.CurPlacedPlayerRole = this;
            OnMoveToOtherLocator.Invoke(BelongToLocator);
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

        protected override void ClearData() {
            _MoveToLocatorTweener.Kill();
            if (BelongToLocator != null) {
                BelongToLocator.CurPlacedPlayerRole = null;
                BelongToLocator                     = null;
            }
            base.ClearData();
        }
    }
}