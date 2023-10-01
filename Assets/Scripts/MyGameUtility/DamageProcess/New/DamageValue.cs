using System;
using System.Collections.Generic;

namespace MyGameUtility {
    public class DamageValue<T> {
        public string                      BeHitAnimation;
        public List<DamageValueElement<T>> AllDamageValueElements = new List<DamageValueElement<T>>();

        public DamageValue() { }
    }
}