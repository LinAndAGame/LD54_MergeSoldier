namespace MyGameUtility {
    public class ValueCacheFloat : BaseValueCacheNumber<float> {
        private ValueCacheFloat(float originalValue) : base(originalValue) { }
        
        public override float GetValue() {
            float result = _OriginalValue;
            foreach (var element in ElementCaches) {
                result += element.Value;
            }

            return result;
        }

        protected override ValueCacheElement<float> GenElement(float defaultValue) {
            return new ValueCacheElement<float>(defaultValue,this);
        }

        public static ValueCacheFloat operator +(ValueCacheFloat a, float b) {
            a._OriginalValue += b;
            return a;
        }

        public static ValueCacheFloat operator -(ValueCacheFloat a, float b) {
            a._OriginalValue -= b;
            return a;
        }

        public static ValueCacheFloat operator *(ValueCacheFloat a, float b) {
            a._OriginalValue *= b;
            return a;
        }

        public static ValueCacheFloat operator /(ValueCacheFloat a, float b) {
            a._OriginalValue *= b;
            return a;
        }

        public static implicit operator float(ValueCacheFloat target) {
            return target.GetValue();
        }

        public static implicit operator ValueCacheFloat(float target) {
            return new ValueCacheFloat(target);
        }
    }
}