using System;
using Sirenix.OdinInspector;

namespace MyGameUtility {
    public interface IDefData2<T, T2> where T : IComparable<T> where T2 : BaseHealthGroupInfo<T> {
        [ShowInInspector]
        public T2 HealthGroupInfo { get; }
        
        public CustomAction<DamageValue<T>> OnApplyDamageBefore  { get; }
        public CustomAction<DamageValue<T>> OnApplyDamageAfter   { get; }
        public CustomAction<DamageValue<T>> OnBeKilledBefore     { get; }
        public CustomAction<DamageValue<T>> OnBeKilled           { get; }
        public CustomAction<DamageValue<T>> OnDamageProcessAfter { get; }
        
        public bool IsDeath { get; }
        public void ApplyDamage(DamageValue<T> damageValue);
    }
}