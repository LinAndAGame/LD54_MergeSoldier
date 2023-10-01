using System;
using System.Collections.Generic;
using Enemy;
using Map;
using MyGameExpand;
using MyGameUtility;
using Player;
using RoleGroups;
using UnityEngine;

namespace Fight {
    public class FightCtrl : MonoSingletonSimple<FightCtrl> {
        public int                          MaxFightEnemyCount = 10;
        public PlayerCtrl                   PlayerCtrlRef;
        public MapCtrl                      MapCtrlRef;
        public EnemyCreatorCtrl             EnemyCreatorCtrlRef;
        public RoleGroupCreator             RoleGroupCreator;
        public RoleCreatorCtrl              RoleCreatorCtrlRef;
        public FightUI                      FightUIRef;
        public List<BasePlayerAbilityAsset> AllPlayerAbilityAssets;

        public FightData Data;
        
        public bool IsGameOver { get; set; }

        private void Start() {
            Data              = new FightData();
            Data.FightProcess = new MinMaxValueInt(0, MaxFightEnemyCount, 0);
            FightUIRef.ShowStartPanel();
            
            Data.FightProcess.OnCurValueEqualsMax.AddListener(() => {
                StopGame();
                Data.CurEnemyLv.Current++;
                AllPlayerAbilityAssets.RandomList();
                FightUIRef.OpenPlayerAbilityChoosePanel(AllPlayerAbilityAssets.GetRange(0,3));
                Data.FightProcess.Current = 0;
            });
            
            Data.TotalHp.OnCurValueEqualsMin.AddListener(() => {
                GameOver();
                IsGameOver = true;
            });
            
            Data.CurTimeScale.OnAnyValueChangedAfter.AddListener(() => {
                Time.timeScale = Data.CurTimeScale.Current;
            });
        }

        public void StartGame() {
            FightUIRef.Init();
            PlayerCtrlRef.Init();
            EnemyCreatorCtrlRef.Init();
            RoleGroupCreator.Init();
        }

        public void StopGame() {
            Time.timeScale = 0;
        }

        public void ContinueGame() {
            Time.timeScale = Data.CurTimeScale.Current;
        }

        public void GameOver() {
            FightUIRef.ShowEndPanel();
        }
    }
}