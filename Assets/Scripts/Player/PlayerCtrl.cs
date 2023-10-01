using System;
using System.Linq;
using Fight;
using Map;
using UnityEngine;

namespace Player {
    public class PlayerCtrl : MonoBehaviour {
        private MapLocator _LastTouchedMapLocator;

        public MapLocator LastTouchedMapLocator {
            get => _LastTouchedMapLocator;
            set {
                if (_LastTouchedMapLocator != null) {
                    _LastTouchedMapLocator.HighlightEffectRef.highlighted = false;
                }

                _LastTouchedMapLocator                                = value;
                _LastTouchedMapLocator.HighlightEffectRef.highlighted = true;

                if (FightCtrl.I.MapCtrlRef.AllPlayerPreviewMapLocators.Contains(_LastTouchedMapLocator)) {
                    LastTouchedCanPlacedMapLocator = value;
                }
            }
        }

        public bool IsTouchingCanPlacedMapLocator => FightCtrl.I.MapCtrlRef.AllPlayerPreviewMapLocators.Contains(LastTouchedMapLocator);

        private MapLocator _LastTouchedCanPlacedMapLocator;

        public MapLocator LastTouchedCanPlacedMapLocator {
            get => _LastTouchedCanPlacedMapLocator;
            private set {
                if (_LastTouchedCanPlacedMapLocator != null) {
                    _LastTouchedCanPlacedMapLocator.HighlightEffectRef.highlighted = false;
                }

                _LastTouchedCanPlacedMapLocator                                = value;
                _LastTouchedCanPlacedMapLocator.HighlightEffectRef.highlighted = true;

                FightCtrl.I.RoleGroupCreator.PreviewRoleGroupToCurTouchedMapLocator();
            }
        }

        private RaycastHit2D[] _Hit2Ds = new RaycastHit2D[10];

        public void Init() {
            LastTouchedMapLocator = FightCtrl.I.MapCtrlRef.AllPlayerPreviewMapLocators[0];
        }

        private void Update() {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var hitCount = Physics2D.RaycastNonAlloc(mouseWorldPos, Vector2.zero, _Hit2Ds);
            if (hitCount > 0) {
                for (int i = 0; i < hitCount; i++) {
                    var curHit2D = _Hit2Ds[i];
                    if (curHit2D.transform.CompareTag("MapLocator")) {
                        var curLocator = curHit2D.transform.GetComponent<MapLocator>();
                        if (curLocator != LastTouchedMapLocator) {
                            LastTouchedMapLocator = curLocator;
                        }
                    }
                }
            }
        }
    }
}