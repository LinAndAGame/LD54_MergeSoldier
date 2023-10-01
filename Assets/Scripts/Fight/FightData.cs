using MyGameUtility;
using UnityEngine;

namespace Fight {
    public class FightData {
        public MinMaxValueInt CurEnemyLv = new MinMaxValueInt(1, 100, 1); 
        public MinMaxValueInt TotalHp    = new MinMaxValueInt(0, 5, 5);
        public MinMaxValueInt FightProcess    = new MinMaxValueInt(0, 10, 0);
        public MinMaxValueFloat CurTimeScale    = new MinMaxValueFloat(1,5,1);
    }
}