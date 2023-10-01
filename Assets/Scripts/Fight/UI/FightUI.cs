using System.Collections.Generic;
using DG.Tweening;
using MyGameUtility;
using Player;
using Role;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Fight {
    public class FightUI : MonoBehaviour {
        public Button        BtnStartGame;
        public RectTransform RectTrans_StartPanel;

        public Slider          Sld_EnemyLevelUpProcess;
        public TextMeshProUGUI TMP_CurEnemyLevelUpProcess;
        public TextMeshProUGUI TMP_MaxEnemyLevelUpProcess;

        public Slider          Sld_PlayerCurTotalHp;
        public TextMeshProUGUI TMP_PlayerCurTotalHp;
        public TextMeshProUGUI TMP_PlayerMaxTotalHp;

        public RectTransform             RectTrans_AbilityPanel;
        public List<Panel_PlayerAbility> AllPlayerAbilityPanels;

        public RectTransform   RectTrans_PlayEnd;
        public Button          Btn_Replay;
        public Button          Btn_Quit;
        public TextMeshProUGUI TMP_EnemyLv;

        public RectTransform              RectTrans_PlayerRoleDetailInfos;
        public Panel_PlayerRoleDetailInfo PlayerRoleDetailInfoPrefab;

        public TextMeshProUGUI TMP_CurCachedRoleGroup;

        public Slider          Sld_TimeScale;
        public TextMeshProUGUI TMP_CurTimeScale;

        public TextMeshProUGUI TMP_CurEnemyLv;

        private Tweener _PanelShowTweener;

        public void Init() {
            foreach (Panel_PlayerAbility playerAbilityPanel in AllPlayerAbilityPanels) {
                playerAbilityPanel.Init();
            }

            Sld_EnemyLevelUpProcess.maxValue = FightCtrl.I.Data.FightProcess.Max;
            RefreshFightProcess(FightCtrl.I.Data.FightProcess);
            FightCtrl.I.Data.FightProcess.OnAnyValueChangedAfter.AddListener(() => {
                RefreshFightProcess(FightCtrl.I.Data.FightProcess);
            });

            Sld_PlayerCurTotalHp.maxValue = FightCtrl.I.Data.TotalHp.Max;
            RefreshTotalHp(FightCtrl.I.Data.TotalHp);
            FightCtrl.I.Data.TotalHp.OnAnyValueChangedAfter.AddListener(() => { RefreshTotalHp(FightCtrl.I.Data.TotalHp); });

            foreach (RoleCtrl playerRolePrefab in FightCtrl.I.RoleCreatorCtrlRef.AllPlayerRolePrefabs) {
                var ins = Instantiate(PlayerRoleDetailInfoPrefab, RectTrans_PlayerRoleDetailInfos);
                ins.Init(playerRolePrefab);
            }
            
            FightCtrl.I.RoleGroupCreator.OnCachedRoleGroupsChanged.AddListener(() => {
                TMP_CurCachedRoleGroup.text = FightCtrl.I.RoleGroupCreator.CurCachedRoleGroupCount.ToString();
            });
            
            Sld_TimeScale.onValueChanged.AddListener(data => {
                FightCtrl.I.Data.CurTimeScale.Current = data;
                TMP_CurTimeScale.text         = data.ToString();
            });
            
            FightCtrl.I.Data.CurEnemyLv.OnAnyValueChangedAfter.AddListener(() => {
                TMP_CurEnemyLv.text = FightCtrl.I.Data.CurEnemyLv.Current.ToString();
            });
        }

        public void ShowStartPanel() {
            RectTrans_StartPanel.gameObject.SetActive(true);
            BtnStartGame.onClick.AddListener(() => {
                RectTrans_StartPanel.gameObject.SetActive(false);
                FightCtrl.I.StartGame();
            });
        }

        public void ShowEndPanel() {
            RectTrans_PlayEnd.gameObject.SetActive(true);
            Btn_Replay.onClick.AddListener(() => {
                SceneManager.LoadScene(1);
                SceneManager.LoadScene(0);
            });
            Btn_Quit.onClick.AddListener(() => {
                Application.Quit();
            });
            TMP_EnemyLv.text = FightCtrl.I.Data.CurEnemyLv.Current.ToString();
        }

        private void RefreshTotalHp(MinMaxValueInt hp) {
            Sld_PlayerCurTotalHp.value = hp.Current;
            TMP_PlayerCurTotalHp.text  = hp.Current.ToString();
            TMP_PlayerMaxTotalHp.text  = hp.Max.ToString();
        }

        private void RefreshFightProcess(MinMaxValueInt fightProcess) {
            Sld_EnemyLevelUpProcess.value = fightProcess.Current;
            TMP_CurEnemyLevelUpProcess.text  = fightProcess.Current.ToString();
            TMP_MaxEnemyLevelUpProcess.text  = fightProcess.Max.ToString();
        }

        public void OpenPlayerAbilityChoosePanel(List<BasePlayerAbilityAsset> allAbilities) {
            _PanelShowTweener?.Kill(true);
            RectTrans_AbilityPanel.gameObject.SetActive(true);
            RectTrans_AbilityPanel.localScale = Vector3.zero;
            _PanelShowTweener                 = RectTrans_AbilityPanel.DOScale(Vector3.one, 0.5f).SetEase(Ease.InSine).SetUpdate(true);
            for (var i = 0; i < allAbilities.Count; i++) {
                AllPlayerAbilityPanels[i].ShowAbility(allAbilities[i]);
            }
        }

        public void HidePlayerAbilityChoosePanel() {
            _PanelShowTweener?.Kill(true);
            RectTrans_AbilityPanel.localScale =  Vector3.zero;
            _PanelShowTweener                 =  RectTrans_AbilityPanel.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InSine).SetUpdate(true);
            _PanelShowTweener.onComplete      += () => { RectTrans_AbilityPanel.gameObject.SetActive(false); };
        }
    }
}