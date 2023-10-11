using System;

namespace MyGameUtility {
    public abstract class BaseBuff {
        private int _Layer;
        public int Layer {
            get => _Layer;
            set {
                _Layer = value;
                if (_Layer <= 0) {
                    BuffOwner.RemoveBuff(this);
                }
            }
        }

        protected CustomAction OnMerged = new CustomAction();
        
        protected ValueCacheCollection  VCC = new ValueCacheCollection();
        protected CustomEventCollection CEC = new CustomEventCollection();
        protected BuffCollection        BC  = new BuffCollection();
        
        public BaseBuffSystem BuffOwner { get; private set; }

        public BaseBuff(int layer) {
            this._Layer = layer;
        }
        
        public void Init(BaseBuffSystem buffSystem) {
            BuffOwner = buffSystem;
            InitInternal();
        }

        public void RemoveFromBuffOwner() {
            if (BuffOwner == null) {
                return;
            }

            BuffOwner.RemoveBuffLayer(this);
        }

        protected virtual void InitInternal() { }

        public void ClearBuff() {
            VCC.Clear();
            CEC.Clear();
            BuffOwner = null;
        }

        public virtual void MergeBuff(BaseBuff otherBuff) {
            this.Layer += otherBuff.Layer;
            OnMerged.Invoke();
        }
    }
}