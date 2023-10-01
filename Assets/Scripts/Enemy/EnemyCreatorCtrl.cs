using System.Collections;
using System.Collections.Generic;
using Fight;
using Map;
using MyGameExpand;
using MyGameUtility;
using Role;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Enemy {
    public class EnemyCreatorCtrl : MonoBehaviour {
        public List<Role_Enemy> AssetRef_AllEnemies;
        public int                  CountOnce = 6;
        public int                  Interval  = 1;

        public List<Role_Enemy> _AllEnemies { get; private set; }

        public void Init() {
            _AllEnemies = new List<Role_Enemy>();
            StartGenEnemies();
        }
        
        public void StartGenEnemies() {
            StartCoroutine(genEnemies());

            IEnumerator genEnemies() {
                while (true) {
                    var allRandomLocators = FightCtrl.I.MapCtrlRef.AllEnemyMapLocators.GetRandomList(CountOnce);
                    foreach (MapLocator randomLocator in allRandomLocators) {
                        var enemyIns = Instantiate(AssetRef_AllEnemies.GetRandomElement());
                        enemyIns.Init(randomLocator);
                        _AllEnemies.Add(enemyIns);
                    }
                    yield return new WaitForSeconds(Interval);
                }
            }
        }

        public List<Role_Enemy> GetEnemiesByColumn(int columnIndex) {
            return _AllEnemies.FindAll(data => data.ColumnIndex == columnIndex);
        }
    }
}