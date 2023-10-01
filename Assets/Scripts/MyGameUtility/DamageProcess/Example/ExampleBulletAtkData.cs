using System.Collections.Generic;

namespace MyGameUtility {
    public class ExampleBulletAtkData : BaseAtkData<int, ExampleHealthGroupInfo> {
        private int Damage;

        public ExampleBulletAtkData(int damage) {
            Damage = damage;
        }

        public override DamageValue<int> InitData(ExampleHealthGroupInfo healthGroupInfo) {
            DamageValue<int> damageValue = new DamageValue<int>();
            damageValue.AllDamageValueElements.Add(new DamageValueElement<int>(Damage).SetStart(healthGroupInfo.Shield).SetEnd(healthGroupInfo.Hp));
            return damageValue;
        }
    }
}