using System.Collections.Generic;
using Map;
using MyGameUtility;

namespace Buff {
    public class Buff_MapLocatorAddBuffToPlayer : BaseBuffWithOwner<MapLocator> {
        private List<AddBuffInfo> _AllReadyAddedBuffInfos;
        
        public Buff_MapLocatorAddBuffToPlayer(MapLocator dataOwner, List<AddBuffInfo> allReadyAddedBuffs) : base(dataOwner, 1) {
            _AllReadyAddedBuffInfos = new List<AddBuffInfo>(allReadyAddedBuffs);
        }

        protected override void InitInternal() {
            base.InitInternal();
            DataOwner.OnPlayerLeft.AddListener(data => {
                BC.Clear();
            }, CEC);
            DataOwner.OnPlayerPlaced.AddListener(rolePlayer => {
                foreach (var addBuffInfo in _AllReadyAddedBuffInfos) {
                    BC.Add(addBuffInfo.GetBuff(rolePlayer));
                }
            }, CEC);
        }
    }
}