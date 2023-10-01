using System;

namespace MyGameUtility {
    [Serializable]
    public class ExampleCharacterDefData : BaseDefData<int, ExampleHealthGroupInfo> {
        public int Def = 1;

        public override bool IsDeath => HealthGroupInfo.Hp.Value <= 0;

        public override void ApplyDamage(DamageValue<int> damageValue) {
            foreach (var damageValueElement in damageValue.AllDamageValueElements) {
                HealthGroupInfo.ReduceValue(damageValueElement.Start, damageValueElement.Ends, damageValueElement.TotalDamageValue - Def);
            }
        }

        public ExampleCharacterDefData(ExampleHealthGroupInfo healthGroupInfo) : base(healthGroupInfo) { }
    }
}