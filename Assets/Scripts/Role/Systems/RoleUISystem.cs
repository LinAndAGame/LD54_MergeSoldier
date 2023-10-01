using MyGameUtility;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Role {
    public class RoleUISystem : BaseRoleSystem {
        public Canvas          CanvasRef;
        public TextMeshProUGUI TMP_CurHp;
        public TextMeshProUGUI TMP_MaxHp;
        public Slider          Sld_Hp;
        public TextMeshProUGUI TMP_Lv;
        
        public override void InitOnRoleGroup() {
            base.InitOnRoleGroup();
            CanvasRef.gameObject.SetActive(false);
        }

        public override void Init() {
            CanvasRef.gameObject.SetActive(true);
            CanvasRef.worldCamera = Camera.main;
        }

        public void RefreshHp(MinMaxValueFloat hp) {
            TMP_CurHp.text = hp.Current.ToString();
            TMP_MaxHp.text = hp.Max.ToString();
            Sld_Hp.value   = hp.Ratio;
        }

        public void RefreshLv(int lv) {
            TMP_Lv.text = lv.ToString();
        }
    }
}