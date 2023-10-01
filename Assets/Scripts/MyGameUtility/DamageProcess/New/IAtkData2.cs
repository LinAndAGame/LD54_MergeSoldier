using System;
using System.Collections.Generic;

namespace MyGameUtility {
    public interface IAtkData2<T, T2> where T2 : BaseHealthGroupInfo<T> where T : IComparable<T> {
        public CustomAction<DamageValue<T>> OnApplyDamageBefore  { get; }
        public CustomAction<DamageValue<T>> OnApplyDamageAfter   { get; }
        public CustomAction<DamageValue<T>> OnKillBefore         { get; }
        public CustomAction<DamageValue<T>> OnKill               { get; }
        public CustomAction<DamageValue<T>> OnDamageProcessAfter { get; }
        List<DamageValueElement<T>>         InitData(T2 healthGroupInfo);
    }
}