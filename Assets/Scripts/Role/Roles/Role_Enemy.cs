using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using DamageProcess;
using Fight;
using Map;
using MyGameExpand;
using MyGameUtility;
using Unity.VisualScripting;
using UnityEngine;

namespace Role {
    public class Role_Enemy : RoleCtrl {
        public int   ColumnIndex;
        public float MoveSpeed = 2;

        private ValueCacheCollection _VCC_Move          = new ValueCacheCollection();
        private HashSet<Role_Player>    _AllTouchedPlayers = new HashSet<Role_Player>();

        private void OnTriggerEnter2D(Collider2D other) {
            if (FightCtrl.I.IsGameOver == false && other.CompareTag("PlayerTotalHp")) {
                FightCtrl.I.Data.TotalHp.Current--;
                Death();
            }
            AddTouchedPlayer(other);
        }
        private void OnTriggerStay2D(Collider2D other) {
            AddTouchedPlayer(other);
        }

        private void AddTouchedPlayer(Collider2D other) {
            if (other.CompareTag("Player")) {
                Role_Player player = other.transform.GetComponent<Role_Player>();
                if (player.BelongToLocator.IsPreviewLocator) {
                    return;
                }
                
                if (_AllTouchedPlayers.Count == 0) {
                    _VCC_Move.Add(RoleStateInfoRef.CanMove.GetCacheElement());
                }
                _AllTouchedPlayers.Add(player);
            }
        }

        private void OnTriggerExit2D(Collider2D other) {
            if (other.CompareTag("Player")) {
                Role_Player enemy = other.transform.GetComponent<Role_Player>();
                _AllTouchedPlayers.Remove(enemy);
                if (_AllTouchedPlayers.Count == 0) {
                    _VCC_Move.RemoveAll();
                }
            }
        }

        public void Init(MapLocator mapLocator) {
            RoleEffectSystemRef.SR_Self.color = RoleEffectSystemRef.SR_Self.color.SetA(1);
            ColumnIndex                       = mapLocator.Pos.x;
            this.transform.position           = mapLocator.transform.position;
            this.transform.SetParent(FightCtrl.I.EnemyCreatorCtrlRef.transform);
            
            // 属性数值更新
            
            RoleCommonInfo        = new RoleCommonInfo(this);
            RoleCommonInfo.Damage = new ValueCacheFloat(1);
            RoleCommonInfo.Lv     = FightCtrl.I.Data.CurEnemyLv.Current;
            
            RoleStateInfoRef = new RoleStateInfo(this);
            RoleEventRef     = new RoleEvent(this);

            float maxHp = StandardLevelHp_V1.FindEntity(data => data.f_Lv == RoleCommonInfo.Lv).f_Hp;
            RoleHpSystemRef.Hp = new MinMaxValueFloat(0, maxHp, maxHp);
            foreach (BaseRoleSystem roleSystem in AllRoleSystems) {
                roleSystem.Init();
            }
            
            RoleEventRef.OnDeathSucceed.AddListener(() => {
                FightCtrl.I.Data.FightProcess.Current++;
            });
            _HasInit = true;
        }

        protected override void AdditionalFixedUpdate() {
            base.AdditionalFixedUpdate();
            if (RoleStateInfoRef.CanMove.GetValue()) {
                this.transform.position = Vector3.MoveTowards(this.transform.position, this.transform.position + Vector3.down * 10, Time.fixedDeltaTime * MoveSpeed);
            }
        }

        protected override bool CanAttack() {
            return _AllTouchedPlayers.Count != 0;
        }

        protected override void Attack() {
            DamageInfo damageInfo = new DamageInfo();
            damageInfo.DamageFrom = this;
            damageInfo.Damage     = this.RoleCommonInfo.Damage.GetValue();
            var touchedPlayerList = _AllTouchedPlayers.ToList();
            for (int i = touchedPlayerList.Count - 1; i >= 0; i--) {
                Role_Player touchedEnemy = touchedPlayerList[i];
                touchedEnemy.BeHit(damageInfo);
            }
        }

        public override void DestroySelf() {
            Destroy(this.gameObject);
        }
    }
}