namespace MyGameUtility {
    public class ValueCacheInt : BaseValueCacheNumber<int> {
        public ValueCacheInt(int originalValue) : base(originalValue) { }
        
        public override int GetValue() {
            int result = _OriginalValue;
            foreach (var element in ElementCaches) {
                result += element.Value;
            }

            return result;
        }

        protected override ValueCacheElement<int> GenElement(int defaultValue) {
            return new ValueCacheElement<int>(defaultValue,this);
        }

        public static ValueCacheInt operator +(ValueCacheInt a, int b) {
            a._OriginalValue += b;
            return a;
        }

        public static ValueCacheInt operator -(ValueCacheInt a, int b) {
            a._OriginalValue -= b;
            return a;
        }

        public static ValueCacheInt operator *(ValueCacheInt a, int b) {
            a._OriginalValue *= b;
            return a;
        }

        public static ValueCacheInt operator /(ValueCacheInt a, int b) {
            a._OriginalValue *= b;
            return a;
        }
    }
}