using System;
using System.Collections.Generic;

namespace MyGameUtility {
    public abstract class BaseAtkData<T, T2> where T2 : BaseHealthGroupInfo<T> where T : IComparable<T> {
        public CustomAction<DamageValue<T>> OnApplyDamageBefore;
        public CustomAction<DamageValue<T>> OnApplyDamageAfter;
        public CustomAction<DamageValue<T>> OnKillBefore;
        public CustomAction<DamageValue<T>> OnKill;
        public CustomAction<DamageValue<T>> OnDamageProcessAfter;

        public BaseAtkData() {
            CustomEventUtility.CreateCustomEventInClass(this);
        }


        public abstract DamageValue<T> InitData(T2 healthGroupInfo);
    }
}