using UnityEngine;

namespace MyGameUtility {
    public class HealthSingleInfoInt : BaseHealthSingleInfo<int> {
        public override int ReduceValue(int target) {
            int temp = Mathf.Min(Value, target);
            Value -= temp;
            return target - temp;
        }

        public override bool IsNoRemainingValue() {
            return Value <= 0;
        }
    }
}