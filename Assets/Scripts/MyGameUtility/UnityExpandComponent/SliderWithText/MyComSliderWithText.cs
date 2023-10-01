using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MyGameUtility {
    public class MyComSliderWithText : MonoBehaviour {
        // 血条组件
        public Slider          SldRef;
        public TextMeshProUGUI TMP_CurValue;
        public TextMeshProUGUI TMP_MaxValue;

        public float Value {
            get => SldRef.value;
            set {
                SldRef.value      = value;
                TMP_CurValue.text = value.ToString();
            }
        }

        public float MaxValue {
            get => SldRef.maxValue;
            set {
                SldRef.maxValue   = value;
                TMP_MaxValue.text = value.ToString();
            }
        }

        public void Init(float value, float maxValue) {
            this.Value    = value;
            this.MaxValue = maxValue;
        }
    }
}