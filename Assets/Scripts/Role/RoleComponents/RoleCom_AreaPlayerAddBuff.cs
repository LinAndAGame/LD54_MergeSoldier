using System;
using System.Collections.Generic;
using Buff;
using HighlightPlus;
using Map;
using MyGameUtility;

namespace Role {
    public class RoleCom_AreaPlayerAddBuff : BaseRoleComponent<BaseRole_Player> {
        public List<AddBuffInfo> AllAddBuffInfos = new List<AddBuffInfo>();
        public HighlightProfile  AreaHighlightProfile;

        private List<MapLocator> _HighLightedAreas = new List<MapLocator>();

        private Physics2DTouchUtility.TouchingInfo _CurTouchingInfo;

        public override void Init() {
            base.Init();
            addBuffToLocator(Owner.BelongToLocator);
            Owner.OnLeftLocator.AddListener(data => {
                BC.Clear();
            }, CEC);
            Owner.OnMoveToOtherLocator.AddListener(addBuffToLocator, CEC);

            Physics2DTouchUtility.I.OnMouseEnter.AddListener(data => {
                if (_CurTouchingInfo != null || data.HitTrans != this.transform) {
                    return;
                }
                if (Owner.BelongToLocator != null && Owner.BelongToLocator.IsPreviewLocator) {
                    return;
                }

                _CurTouchingInfo = data;
                _HighLightedAreas.AddRange(Owner.BelongToLocator.AroundCanAttackLocators);
                foreach (var highLightedArea in _HighLightedAreas) {
                    highLightedArea.PlayHighLightEffect(AreaHighlightProfile);
                }
                
                data.OnMouseExit.AddListener(() => {
                    foreach (var highLightedArea in _HighLightedAreas) {
                        highLightedArea.CloseHighLightEffect();
                    }
                    _HighLightedAreas.Clear();
                    _CurTouchingInfo = null;
                }, data.CEC);
            }, CEC);

            void addBuffToLocator(MapLocator curBelongToLocator) {
                foreach (var aroundLocator in curBelongToLocator.AroundCanAttackLocators) {
                    var mapLocatorBuff = new Buff_MapLocatorAddBuffToPlayer(aroundLocator, AllAddBuffInfos);
                     aroundLocator.BuffSystemRef.AddBuff(mapLocatorBuff,BC);
                }
            }
        }
    }
}