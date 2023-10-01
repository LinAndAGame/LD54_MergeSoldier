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

        public Role_Player CurPlacedRoleCtrl { get; set; }

        public bool       HasRoleData  => CurPlacedRoleCtrl != null;
        public MapLocator UpLocator    => FightCtrl.I.MapCtrlRef.AllMapLocators.Find(data => data.Pos == this.Pos + Vector2Int.up);
        public MapLocator DownLocator  => FightCtrl.I.MapCtrlRef.AllMapLocators.Find(data => data.Pos == this.Pos + Vector2Int.down);
        public MapLocator LeftLocator  => FightCtrl.I.MapCtrlRef.AllMapLocators.Find(data => data.Pos == this.Pos + Vector2Int.left);
        public MapLocator RightLocator => FightCtrl.I.MapCtrlRef.AllMapLocators.Find(data => data.Pos == this.Pos + Vector2Int.right);

        public bool IsCanAttackLocator => FightCtrl.I.MapCtrlRef.AllPlayerCanAttackMapLocators.Contains(this);
        public bool IsPreviewLocator   => FightCtrl.I.MapCtrlRef.AllPlayerPreviewMapLocators.Contains(this);

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

        // private void OnMouseEnter() {
        //     FightCtrl.I.PlayerCtrlRef.LastTouchedMapLocator = this;
        // }

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