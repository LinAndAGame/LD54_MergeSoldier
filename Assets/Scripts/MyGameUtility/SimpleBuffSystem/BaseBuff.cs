namespace MyGameUtility {
    public abstract class BaseBuff {
        public int    Layer;
        public string Name;

        protected ValueCacheCollection VCC = new ValueCacheCollection();
        protected CustomEventCache     CEC = new CustomEventCache();

        public virtual void Init() {
            
        }

        public void ClearBuff() {
            VCC.Clear();
            CEC.Clear();
        }

        public virtual void MergeBuff(BaseBuff otherBuff) {
            this.Layer += otherBuff.Layer;
        }
    }
}