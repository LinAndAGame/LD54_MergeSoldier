using System;
using DG.Tweening;
using MyGameExpand;
using MyGameUtility;
using TMPro;
using UnityEngine;

namespace Effects {
    public class TextEffect : MonoBehaviour {
        public TextMeshPro TMP_Value;
        public float       ShowDuration = 0.3f;
        public float       HideDuration = 0.4f;
        public Vector3     RandomMinSpeed;
        public Vector3     RandomMaxSpeed;
        public Vector3     Grave;

        private Vector3 _CurSpeed;

        public void PlayEffect(string value, Vector3 position) {
            _CurSpeed                 = new Vector3(UnityEngine.Random.Range(RandomMinSpeed.x, RandomMaxSpeed.x), UnityEngine.Random.Range(RandomMinSpeed.y, RandomMaxSpeed.y));
            TMP_Value.text            = value;
            TMP_Value.color           = TMP_Value.color.SetA(1);
            this.transform.position   = position;
            this.transform.localScale = Vector3.zero;
            Sequence sequence = DOTween.Sequence();
            sequence.Append(this.transform.DOScale(Vector3.one, ShowDuration));
            sequence.Append(TMP_Value.DOFade(0, HideDuration));

            sequence.onComplete += () => {
                MyPoolSimpleComponent.Release(this);
            };
        }

        private void Update() {
            this.transform.position += _CurSpeed * Time.deltaTime + 0.5f  * Grave * Mathf.Pow(Time.deltaTime, 2);
            _CurSpeed               = _CurSpeed                  + Grave * Time.deltaTime;
        }
    }
}