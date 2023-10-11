using System;
using System.Collections.Generic;
using Fight;
using HighlightPlus;
using MyGameExpand;
using MyGameUtility;
using Role;
using UnityEngine;

namespace Map {
    public class MapLocator : MonoBehaviour {
        public CustomAction<BaseRole_Player>                  OnPlayerPlaced   = new CustomAction<BaseRole_Player>();
        public CustomAction<BaseRole_Player>                  OnPlayerLeft   = new CustomAction<BaseRole_Player>();

        public Vector2Int      Pos;
        public HighlightEffect HighlightEffectRef;
        public SpriteRenderer  SR_Self;

        private BaseRole_Player _CurPlacedPlayerRole;
        public BaseRole_Player CurPlacedPlayerRole {
            get => _CurPlacedPlayerRole;
            set {
                if (_CurPlacedPlayerRole != null) {
                    OnPlayerLeft.Invoke(_CurPlacedPlayerRole);
                }
                _CurPlacedPlayerRole = value;
                if (_CurPlacedPlayerRole != null) {
                    OnPlayerPlaced.Invoke(_CurPlacedPlayerRole);
                }
            }
        }

        public BaseBuffSystem BuffSystemRef { get; } = new BuffSystemDefault();

        public bool       HasRoleData  => CurPlacedPlayerRole != null;
        public MapLocator UpLocator    => FightCtrl.I.MapCtrlRef.GetUpAroundLocator(this);
        public MapLocator DownLocator  => FightCtrl.I.MapCtrlRef.GetDownAroundLocator(this);
        public MapLocator LeftLocator  => FightCtrl.I.MapCtrlRef.GetLeftAroundLocator(this);
        public MapLocator RightLocator => FightCtrl.I.MapCtrlRef.GetRightAroundLocator(this);

        public bool IsCanAttackLocator => FightCtrl.I.MapCtrlRef.IsLocatorAtAttackArea(this);
        public bool IsPreviewLocator   => FightCtrl.I.MapCtrlRef.IsLocatorAtPreviewArea(this);

        public List<MapLocator> AroundLocators {
            get {
                List<MapLocator> result = new List<MapLocator>();
                result.AddExceptNull(UpLocator);
                result.AddExceptNull(DownLocator);
                result.AddExceptNull(LeftLocator);
                result.AddExceptNull(RightLocator);

                return result;
            }
        }

        public void DestroySelf() {
            if (Application.isPlaying) {
                Destroy(this.gameObject);
            }
            else {
                DestroyImmediate(this.gameObject);
            }
        }

        public void Editor_Init(Vector2Int pos) {
            Pos       =  pos;
            this.name += $"_{pos.x}_{pos.y}";
        }
    }
}