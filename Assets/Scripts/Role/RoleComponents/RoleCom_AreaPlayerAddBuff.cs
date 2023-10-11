using System;
using System.Collections.Generic;
using Buff;

namespace Role {
    public class RoleCom_AreaPlayerAddBuff : BaseRoleComponent<BaseRole_Player> {
        public List<AddBuffInfo> AllAddBuffInfos = new List<AddBuffInfo>();
        
        public override void Init() {
            base.Init();
            Owner.OnLeftLocator.AddListener(data => {
                BC.Clear();
            }, CEC);
            Owner.OnMoveToOtherLocator.AddListener(data => {
                foreach (var aroundLocator in data.AroundLocators) {
                    var mapLocatorBuff = new Buff_MapLocatorAddBuffToPlayer(aroundLocator, AllAddBuffInfos);
                    aroundLocator.BuffSystemRef.AddBuff(mapLocatorBuff,BC);
                }
            }, CEC);
        }
    }
}