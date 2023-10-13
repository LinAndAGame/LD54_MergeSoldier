using System;
using System.Collections.Generic;
using System.Linq;
using MyGameExpand;
using UnityEngine;

namespace MyGameUtility {
    public class Physics2DTouchUtility : MonoSingletonSimple<Physics2DTouchUtility> {

        public CustomAction<TouchingInfo> OnMouseEnter = new CustomAction<TouchingInfo>();

        private List<TouchingInfo>    _LastTouchingInfos       = new List<TouchingInfo>();
        private List<TouchingInfo>    _CurTouchingInfos        = new List<TouchingInfo>();
        private HashSet<TouchingInfo> _CurTouchingInfosHashSet = new HashSet<TouchingInfo>(new TouchingInfo.EqualityComparer());

        public int MaxCachedHitResultCount = 10;

        private RaycastHit2D[] _Hit2Ds;

        private void Start() {
            _Hit2Ds = new RaycastHit2D[MaxCachedHitResultCount];
        }

        private void Update() {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var     hitCount      = Physics2D.RaycastNonAlloc(mouseWorldPos, Vector2.zero, _Hit2Ds);
            if (hitCount > 0) {
                _CurTouchingInfos.Clear();
                _CurTouchingInfosHashSet.Clear();
                for (int i = 0; i < hitCount; i++) {
                    var curHit2D          = _Hit2Ds[i];
                    var hitTrans          = curHit2D.transform;
                    var aliveTouchingInfo = _LastTouchingInfos.Find(data => data.HitTrans == hitTrans);
                    // 如果不存在就新创建一个触摸数据
                    if (aliveTouchingInfo == null) {
                        aliveTouchingInfo = new TouchingInfo(curHit2D);
                        OnMouseEnter.Invoke(aliveTouchingInfo);
                    }
                    
                    _CurTouchingInfos.Add(aliveTouchingInfo);
                    _CurTouchingInfosHashSet.Add(aliveTouchingInfo);
                }
                
                // 当前所有正在触摸的对象都激活Stay时间
                foreach (var curTouchingInfo in _CurTouchingInfos) {
                    curTouchingInfo.OnMouseStay.Invoke();
                }
                
                // 上一次存储的数据中，当前触摸数据已经找不到的需要触发Exit事件
                foreach (var lastTouchingInfo in _LastTouchingInfos) {
                    if (_CurTouchingInfosHashSet.Contains(lastTouchingInfo) == false) {
                        lastTouchingInfo.Exit();
                    }
                }

                _LastTouchingInfos = new List<TouchingInfo>(_CurTouchingInfos);
            }
            else {
                if (_LastTouchingInfos.IsNullOrEmpty() == false) {
                    foreach (var lastTouchingInfo in _LastTouchingInfos) {
                        lastTouchingInfo.Exit();
                    }
                    _LastTouchingInfos.Clear();
                }
            }
        }

        public class TouchingInfo {
            public CustomAction          OnMouseStay = new CustomAction();
            public CustomAction          OnMouseExit = new CustomAction();
            public CustomEventCollection CEC         = new CustomEventCollection();

            public readonly RaycastHit2D Hit2D;

            public Transform HitTrans => Hit2D.transform;

            public TouchingInfo(RaycastHit2D hit2D) {
                Hit2D = hit2D;
            }

            public void Exit() {
                OnMouseExit.Invoke();
                CEC.Clear();
            }
            
            public class EqualityComparer : IEqualityComparer<TouchingInfo> {
                public bool Equals(TouchingInfo x, TouchingInfo y) {
                    if (ReferenceEquals(x, y)) return true;
                    if (ReferenceEquals(x, null)) return false;
                    if (ReferenceEquals(y, null)) return false;
                    if (x.GetType() != y.GetType()) return false;
                    return x.HitTrans.Equals(y.HitTrans);
                }

                public int GetHashCode(TouchingInfo obj) {
                    return obj.HitTrans.GetHashCode();
                }
            }
        }
    }
}