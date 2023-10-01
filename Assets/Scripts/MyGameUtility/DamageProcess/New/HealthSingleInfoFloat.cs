using UnityEngine;

namespace MyGameUtility {
    public class HealthSingleInfoFloat : BaseHealthSingleInfo<float> {
        public override float ReduceValue(float target) {
            float temp = Mathf.Min(Value, target);
            Value -= temp;
            return target - temp;
        }

        public override bool IsNoRemainingValue() {
            return Value <= 0 || Mathf.Approximately(Value, 0);
        }
    }
}