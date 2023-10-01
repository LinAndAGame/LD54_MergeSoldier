using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Fight {
    public class Panel_PlayerAbility : MonoBehaviour {
        public Button          BtnSelf;
        public TextMeshProUGUI TMP_Description;

        private BasePlayerAbilityAsset _CurAbilityAsset;
        
        public void Init() {
            BtnSelf.onClick.AddListener(() => {
                _CurAbilityAsset.ApplyAbility();
                _CurAbilityAsset = null;
                FightCtrl.I.ContinueGame();
                FightCtrl.I.FightUIRef.HidePlayerAbilityChoosePanel();
            });
        }

        public void ShowAbility(BasePlayerAbilityAsset abilityAsset) {
            TMP_Description.text = abilityAsset.Description;
            _CurAbilityAsset     = abilityAsset;
        }
    }
}