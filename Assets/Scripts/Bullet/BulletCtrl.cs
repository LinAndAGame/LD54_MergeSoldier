using System;
using DamageProcess;
using MyGameUtility;
using Role;
using UnityEngine;

namespace Bullet {
    public class BulletCtrl : MonoBehaviour {
        public CustomAction OnHitEnemy = new CustomAction();
        
        public float        MoveSpeed  = 5;
        
        private DamageInfo _DamageInfo;
        
        public BuffSystem BuffSystemRef { get; set; }
        
        public void Init(BaseRoleCtrl roleCtrl) {
            _DamageInfo             = new DamageInfo();
            _DamageInfo.DamageFrom  = roleCtrl;
            _DamageInfo.Damage      = roleCtrl.RoleCommonInfo.Damage.GetValue();
            this.transform.position = roleCtrl.transform.position;
            BuffSystemRef           = new BuffSystem();
        }

        private void Update() {
            this.transform.position = Vector3.MoveTowards(this.transform.position, this.transform.position + Vector3.up * 10, Time.deltaTime * MoveSpeed);
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if (other.CompareTag("Enemy")) {
                Role_Enemy enemy = other.transform.GetComponent<Role_Enemy>();
                enemy.BeHit(_DamageInfo);
                OnHitEnemy.Invoke();
                DestroySelf();
            }
            else if (other.CompareTag("Boundary")) {
                DestroySelf();
            }
        }

        private void DestroySelf() {
            MyPoolSimpleComponent.Release(this);
        }
    }
}