using Role;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Fight {
    public class Panel_PlayerRoleDetailInfo : MonoBehaviour {
        public TextMeshProUGUI TMP_BaseHp;
        public TextMeshProUGUI TMP_BaseAtk;
        public TextMeshProUGUI TMP_RoleType;
        public Image           Img_Role;

        public void Init(BaseRoleCtrl roleCtrl) {
            TMP_RoleType.text = roleCtrl.RoleType.ToString();
            TMP_BaseHp.text   = roleCtrl.BaseHp.ToString();
            TMP_BaseAtk.text  = roleCtrl.BaseDamage.ToString();
            Img_Role.sprite   = roleCtrl.RoleComVfxRef.SR_Self.sprite;
            Img_Role.color    = roleCtrl.RoleComVfxRef.SR_Self.color;
        }
    }
}