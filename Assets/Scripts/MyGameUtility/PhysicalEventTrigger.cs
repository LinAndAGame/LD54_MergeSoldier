/*
 * 功能 : 如果物理检测方法与调用方需要分离，则此脚本
 */

using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;

#if UNITY_EDITOR
using UnityEditorInternal;
#endif
using UnityEngine;

namespace MyGameUtility {
    public class PhysicalEventTrigger : MonoBehaviour {
        public List<Transform> IgnoreTrans;
        public List<string>    IgnoreTags;
        public bool            UseOnlyCheck;
        [ValueDropdown("AllGameTags",IsUniqueList = true)]
        public List<string> OnlyCheckTags;
 
        public Collider2D Collider2DSelf;
        public Collider   ColliderSelf;
        
        public CustomAction<Collider2D> OnTriggerEnter2DAct;
        public CustomAction<Collider2D> OnTriggerStay2DAct;
        public CustomAction<Collider2D> OnTriggerExit2DAct;
        
        public CustomAction<Collision2D> OnCollisionEnter2DAct;
        public CustomAction<Collision2D> OnCollisionStay2DAct;
        public CustomAction<Collision2D> OnCollisionExit2DAct;
        
        public CustomAction<Collider>   OnTriggerEnterAct;
        public CustomAction<Collider>   OnTriggerStayAct;
        public CustomAction<Collider>   OnTriggerExitAct;
        
        public CustomAction<Collision> OnCollisionEnterAct;
        public CustomAction<Collision> OnCollisionStayAct;
        public CustomAction<Collision> OnCollisionExitAct;
        
#if UNITY_EDITOR
        private List<string> AllGameTags => InternalEditorUtility.tags.ToList();
#endif

        private void Awake() {
            CustomEventUtility.CreateCustomEventInClass(this);
        }

        private void OnValidate() {
            if (ColliderSelf == null) {
                ColliderSelf = this.GetComponent<Collider>();
            }

            if (Collider2DSelf == null) {
                Collider2DSelf = this.GetComponent<Collider2D>();
            }
        }

#region InvokeFunctions
        
        private void InvokeAction<T>(T other, CustomAction<T> action) where T : Component {
            InvokeCollisionInternal(other, other.transform, other.gameObject.tag, action);
        }
        private void InvokeCollision2D(Collision2D other, CustomAction<Collision2D> action) {
            InvokeCollisionInternal(other, other.transform, other.gameObject.tag, action);
        }
        private void InvokeCollision(Collision other, CustomAction<Collision> action) {
            InvokeCollisionInternal(other, other.transform, other.gameObject.tag, action);
        }
        private void InvokeCollisionInternal<T>(T other, Transform otherTrans, string otherTag, CustomAction<T> action) {
            if (IgnoreTrans.Contains(otherTrans)) {
                return;
            }
            if (IgnoreTags.Contains(otherTag)) {
                return;
            }

            if (UseOnlyCheck && OnlyCheckTags.Contains(otherTag) == false) {
                return;
            }
            action?.Invoke(other);
        }

#endregion

        private void OnTriggerEnter2D(Collider2D other) {
            InvokeAction(other,OnTriggerEnter2DAct);
        }

        private void OnTriggerStay2D(Collider2D other) {
            InvokeAction(other,OnTriggerStay2DAct);
        }

        private void OnTriggerExit2D(Collider2D other) {
              InvokeAction(other,OnTriggerExit2DAct);
        }

        private void OnCollisionEnter2D(Collision2D other) {
            InvokeCollision2D(other,OnCollisionEnter2DAct);
        }

        private void OnCollisionStay2D(Collision2D other) {
            InvokeCollision2D(other,OnCollisionStay2DAct);
        }

        private void OnCollisionExit2D(Collision2D other) {
            InvokeCollision2D(other,OnCollisionExit2DAct);
        }

        private void OnTriggerEnter(Collider other) {
            InvokeAction(other,OnTriggerEnterAct);
        }

        private void OnTriggerStay(Collider other) {
            InvokeAction(other,OnTriggerStayAct);
        }

        private void OnTriggerExit(Collider other) {
            InvokeAction(other,OnTriggerExitAct);
        }

        private void OnCollisionEnter(Collision other) {
            InvokeCollision(other,OnCollisionEnterAct);
        }

        private void OnCollisionStay(Collision other) {
            InvokeCollision(other,OnCollisionStayAct);
        }

        private void OnCollisionExit(Collision other) {
            InvokeCollision(other,OnCollisionExitAct);
        }

        public void SetEnableOfCollider(bool enable) {
            if (ColliderSelf != null) {
                ColliderSelf.enabled = enable;
            }

            if (Collider2DSelf != null) {
                Collider2DSelf.enabled = enable;
            }
        }
    }
}