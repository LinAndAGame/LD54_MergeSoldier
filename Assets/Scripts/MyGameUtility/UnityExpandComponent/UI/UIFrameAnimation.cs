using System;
using System.Collections.Generic;
using MyGameExpand;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace MyGameUtility {
    public class UIFrameAnimation : MonoBehaviour {
        public Image        ImgRef;
        [Required]
        public List<Sprite> AllSprites;
        public bool         Loop = true;

        [SerializeField]
        private int _FPS = 60;

        private float _IntervalTime;
        private float _NextFrameTime;
        private int   _CurFrameIndex;

        public int FPS {
            get => _FPS;
            set {
                _FPS          = value;
                _IntervalTime = 1f / _FPS;
            }
        }

        private void Awake() {
            FPS = _FPS;
        }

        private void Update() {
            float curTime = Time.time;
            if (curTime >= _NextFrameTime) {
                if (Loop) {
                    if (_CurFrameIndex >= AllSprites.Count) {
                        _CurFrameIndex = 0;
                    }
                }

                if (_CurFrameIndex >= AllSprites.Count) {
                    return;
                }

                ImgRef.sprite = AllSprites[_CurFrameIndex];
                ++_CurFrameIndex;
                _NextFrameTime = curTime + _IntervalTime;
            }
        }

        private void OnValidate() {
            FPS = _FPS;

            if (Application.isPlaying == false) {
            
                if (ImgRef == null) {
                    ImgRef = this.GetComponent<Image>();
                }

                if (AllSprites.IsNullOrEmpty() == false && ImgRef != null) {
                    ImgRef.sprite = AllSprites[0];
                }
            }
        }
    }
}