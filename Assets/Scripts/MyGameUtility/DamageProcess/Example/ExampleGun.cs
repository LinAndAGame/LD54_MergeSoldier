using MyGameExpand;
using UnityEngine;

namespace MyGameUtility {
    public class ExampleGun : MonoBehaviour {
        public int           Damage;
        public ExampleBullet BulletPrefab;
        public KeyCode       FireKeyCode;

        private void Start() {
            MyPoolSimpleComponent.Preload(10, BulletPrefab);
        }

        private void Update() {
            if (Input.GetKeyDown(FireKeyCode)) {
                var dir    = Camera.main.ScreenToWorldPoint(Input.mousePosition).SetZ(0) - this.transform.position;
                var bullet = MyPoolSimpleComponent.Get(BulletPrefab);
                bullet.Init(this.transform.position, dir, this);
            }
        }
    }
}