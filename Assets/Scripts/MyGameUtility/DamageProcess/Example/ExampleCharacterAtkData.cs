using System;
using System.Collections.Generic;

namespace MyGameUtility {
    [Serializable]
    public class ExampleCharacterAtkData : BaseAtkData<int, ExampleHealthGroupInfo> {
        public int CharacterDamage = 10;
        
        public override DamageValue<int> InitData(ExampleHealthGroupInfo healthGroupInfo) {
            DamageValue<int> damageValue = new DamageValue<int>();
            damageValue.AllDamageValueElements.Add(new DamageValueElement<int>(CharacterDamage).SetStart(healthGroupInfo.Shield).SetEnd(healthGroupInfo.Hp));
            return damageValue;
        }
    }
}