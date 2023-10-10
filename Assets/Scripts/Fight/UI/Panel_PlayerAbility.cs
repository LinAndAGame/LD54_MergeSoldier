using Player;
using Role;
using Role.Ability;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Fight {
    public class Panel_PlayerAbility : MonoBehaviour {
        public Button          BtnSelf;
        public TextMeshProUGUI TMP_Description;

        private BaseRoleAbilityData _CurAbilityAsset;
        private BaseRoleCtrl                      _Target;
        
        public void Init() {
            BtnSelf.onClick.AddListener(() => {
                _Target.BuffSystemRef.AddBuff(_CurAbilityAsset.GetRoleAbility(_Target));
                FightCtrl.I.ContinueGame();
                FightCtrl.I.FightUIRef.HidePlayerAbilityChoosePanel();
                FightCtrl.I.Data.CurTimeScale.Current = 1;
            });
        }

        public void ShowAbility(BaseRoleCtrl target, BaseRoleAbilityData abilityAsset) {
            this.gameObject.SetActive(true);
            _Target              = target;
            TMP_Description.text = abilityAsset.Description;
            _CurAbilityAsset     = abilityAsset;
        }

        public void Hide() {
            this.gameObject.SetActive(false);
        }
    }
}