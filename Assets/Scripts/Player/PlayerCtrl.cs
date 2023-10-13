using System;
using System.Linq;
using Fight;
using HighlightPlus;
using Map;
using MyGameUtility;
using Role;
using UnityEngine;

namespace Player {
    public class PlayerCtrl : MonoBehaviour {
        public HighlightProfile HighlightProfile_MouseTouch;

        private MapLocator _LastTouchedMapLocator;

        public MapLocator LastTouchedMapLocator {
            get => _LastTouchedMapLocator;
            set {
                if (_LastTouchedMapLocator != null) {
                    _LastTouchedMapLocator.CloseHighLightEffect();
                }

                _LastTouchedMapLocator = value;
                if (_LastTouchedMapLocator != null) {
                    _LastTouchedMapLocator.PlayHighLightEffect(HighlightProfile_MouseTouch);
                }

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
                _LastTouchedCanPlacedMapLocator = value;
                FightCtrl.I.RoleGroupCreator.PreviewRoleGroupToCurTouchedMapLocator();
            }
        }

        public void Init() {
            LastTouchedMapLocator = FightCtrl.I.MapCtrlRef.AllPlayerPreviewMapLocators[0];
            
            Physics2DTouchUtility.I.OnMouseEnter.AddListener(data => {
                if (data.HitTrans.CompareTag("MapLocator")) {
                    var curLocator = data.HitTrans.GetComponent<MapLocator>();
                    if (curLocator != LastTouchedMapLocator) {
                        LastTouchedMapLocator = curLocator;
                    }

                    data.OnMouseExit.AddListener(() => {
                        LastTouchedMapLocator = null;
                    }, data.CEC);
                }
            });
        }
    }
}