using System;

namespace MyGameUtility {
    public abstract class BaseDefData<T, T2> where T : IComparable<T> where T2 : BaseHealthGroupInfo<T> {
        public T2                           HealthGroupInfo;
        public CustomAction<DamageValue<T>> OnApplyDamageBefore;
        public CustomAction<DamageValue<T>> OnApplyDamageAfter;
        public CustomAction<DamageValue<T>> OnBeKilledBefore;
        public CustomAction<DamageValue<T>> OnBeKilled;
        public CustomAction<DamageValue<T>> OnDamageProcessAfter;

        public BaseDefData(T2 healthGroupInfo) {
            CustomEventUtility.CreateCustomEventInClass(this);
            HealthGroupInfo = healthGroupInfo;
        }

        public abstract bool IsDeath { get; }
        public abstract void ApplyDamage(DamageValue<T> damageValue);
    }
}