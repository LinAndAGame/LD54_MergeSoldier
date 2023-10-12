using System;
using System.Collections.Generic;
using Buff;
using Map;

namespace Role {
    public class RoleCom_AreaPlayerAddBuff : BaseRoleComponent<BaseRole_Player> {
        public List<AddBuffInfo> AllAddBuffInfos = new List<AddBuffInfo>();
        
        public override void Init() {
            base.Init();
            addBuffToLocator(Owner.BelongToLocator);
            Owner.OnLeftLocator.AddListener(data => {
                BC.Clear();
            }, CEC);
            Owner.OnMoveToOtherLocator.AddListener(addBuffToLocator, CEC);

            void addBuffToLocator(MapLocator curBelongToLocator) {
                foreach (var aroundLocator in curBelongToLocator.AroundCanAttackLocators) {
                    var mapLocatorBuff = new Buff_MapLocatorAddBuffToPlayer(aroundLocator, AllAddBuffInfos);
                     aroundLocator.BuffSystemRef.AddBuff(mapLocatorBuff,BC);
                }
            }
        }
    }
}