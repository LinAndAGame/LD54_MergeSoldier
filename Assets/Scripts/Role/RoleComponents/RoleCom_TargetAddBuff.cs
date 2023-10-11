using System;
using System.Collections;
using System.Collections.Generic;
using Buff;
using Fight;
using MyGameExpand;
using MyGameUtility;
using UnityEngine;

namespace Role {
    public class RoleCom_TargetAddBuff : BaseRoleComponent<BaseRoleCtrl> {
        public AddTargetSelectEnum AddTargetSelectType = AddTargetSelectEnum.Random;
        public List<RoleTypeEnum>  AllReadyAddToRoleTypes;
        public AddBuffInfo         AddBuffInfoRef;
        public float               DefaultCD = 3;

        public ValueCacheFloat CD;

        public override void Init() {
            base.Init();
            CD = DefaultCD;
        }

        public override void EffectHandleInternal() {
            base.EffectHandleInternal();
            switch (AddTargetSelectType) {
                case AddTargetSelectEnum.Random:
                    var rolePlayer = FightCtrl.I.MapCtrlRef.AllAttackAreaPlayers.FindAll(data=>AllReadyAddToRoleTypes.Contains(data.RoleType)).GetRandomElement();
                    if (rolePlayer == null) {
                        return;
                    }

                    rolePlayer.BuffSystemRef.AddBuff(AddBuffInfoRef.GetBuff(rolePlayer));
                    break;
                case AddTargetSelectEnum.All:
                    var rolePlayers = FightCtrl.I.MapCtrlRef.AllAttackAreaPlayers.FindAll(data=>AllReadyAddToRoleTypes.Contains(data.RoleType));
                    foreach (var player in rolePlayers) {
                        player.BuffSystemRef.AddBuff(AddBuffInfoRef.GetBuff(player));
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            VCC_CanEffectHandle.Add(CanEffectHandle.GetCacheElement());

            StartCoroutine(delaySetCabEffect());

            IEnumerator delaySetCabEffect() {
                yield return new WaitForSeconds(CD);
                VCC_CanEffectHandle.Clear();
            }
        }
        
        public enum AddTargetSelectEnum {
            Random,
            All,
        }
    }
}