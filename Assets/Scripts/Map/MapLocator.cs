using System;
using System.Collections.Generic;
using Fight;
using HighlightPlus;
using MyGameExpand;
using Role;
using UnityEngine;

namespace Map {
    public class MapLocator : MonoBehaviour {
        public Vector2Int      Pos;
        public HighlightEffect HighlightEffectRef;
        public SpriteRenderer  SR_Self;

        public BaseRole_Player CurPlacedRoleCtrl { get; set; }

        public bool       HasRoleData  => CurPlacedRoleCtrl != null;
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