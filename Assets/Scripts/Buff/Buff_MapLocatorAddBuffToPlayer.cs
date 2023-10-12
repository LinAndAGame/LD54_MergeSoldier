using System.Collections.Generic;
using Map;
using MyGameUtility;
using Role;

namespace Buff {
    public class Buff_MapLocatorAddBuffToPlayer : BaseBuffWithOwner<MapLocator> {
        private List<AddBuffInfo> _AllReadyAddedBuffInfos;
        
        public Buff_MapLocatorAddBuffToPlayer(MapLocator dataOwner, List<AddBuffInfo> allReadyAddedBuffs) : base(dataOwner, 1) {
            _AllReadyAddedBuffInfos = new List<AddBuffInfo>(allReadyAddedBuffs);
        }

        protected override void InitInternal() {
            base.InitInternal();

            resetBuff();
            DataOwner.OnPlayerLeft.AddListener(data => {
                BC.Clear();
            }, CEC);
            DataOwner.OnPlayerPlaced.AddListener(addBuff, CEC);
            OnLayerChanged.AddListener(resetBuff, CEC);

            void resetBuff() {
                BC.Clear();
                if (DataOwner.HasRoleData) {
                    addBuff(DataOwner.CurPlacedPlayerRole);
                }
            }

            void addBuff(BaseRole_Player rolePlayer) {
                foreach (var addBuffInfo in _AllReadyAddedBuffInfos) {
                    var buff = addBuffInfo.GetBuff(rolePlayer);
                    buff.Layer *= this.Layer;
                    rolePlayer.BuffSystemRef.AddBuff(buff, BC);
                }
            }
        }
    }
}