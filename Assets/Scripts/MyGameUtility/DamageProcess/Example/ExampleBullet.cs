using System;
using UnityEngine;

namespace MyGameUtility {
    public class ExampleBullet : MonoBehaviour {
        public int                         Damage;
        public ExampleBulletGiveDamageUnit GiveDamageUnitRef;
        public BoxCollider2D               BoxCollider2DRef;
        
        public float MoveSpeed       = 5;
        public float MaxMoveDuration = 5;

        private bool            _HasInit;
        private Vector3         _Dir;
        private ITimingTaskNode _TimingTaskNode;
        private bool            _CanMove;
        
        public void Init(Vector3 startPos, Vector3 dir, ExampleGun gun) {
            this.transform.position = startPos;
            _Dir                    = dir.normalized;
            _TimingTaskNode         = TimerHelp.AddTimingTask(TimeSpan.FromSeconds(MaxMoveDuration), Recycle);
            _HasInit                = true;
            _CanMove                = true;
            this.gameObject.SetActive(true);
            GiveDamageUnitRef.SetAttackData(new ExampleBulletAtkData(gun.Damage + Damage));
            GiveDamageUnitRef.OpenDetection();
            BoxCollider2DRef.enabled = true;
        }

        private void FixedUpdate() {
            if (_HasInit == false) {
                return;
            }

            if (_CanMove == false) {
                return;
            }
            
            this.transform.position += _Dir * MoveSpeed * Time.fixedDeltaTime;
        }

        private void OnTriggerEnter2D(Collider2D other) {
            this.transform.SetParent(other.transform);
            _CanMove                 = false;
            BoxCollider2DRef.enabled = false;
        }

        private void Recycle() {
            _HasInit = false;
            this.gameObject.SetActive(false);
            _TimingTaskNode.Remove();
            _TimingTaskNode = null;
            GiveDamageUnitRef.CloseDetection();
            _CanMove                 = false;
            BoxCollider2DRef.enabled = false;
            MyPoolSimpleComponent.Release(this);
        }
    }
}