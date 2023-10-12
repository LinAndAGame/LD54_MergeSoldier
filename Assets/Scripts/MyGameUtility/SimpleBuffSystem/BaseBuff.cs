using System;
using Sirenix.OdinInspector;

namespace MyGameUtility {
    [Serializable, ReadOnly]
    public abstract class BaseBuff {
        [ShowInInspector]
        private string TypeName => GetType().Name;
        
        [ShowInInspector]
        private int _Layer;
        public int Layer {
            get => _Layer;
            set {
                _Layer = value;
                OnLayerChanged.Invoke();
                if (_Layer <= 0) {
                    BuffOwner.RemoveBuff(this);
                }
            }
        }

        protected CustomAction OnMerged = new CustomAction();
        protected CustomAction OnLayerChanged = new CustomAction();
        
        protected ValueCacheCollection  VCC = new ValueCacheCollection();
        protected CustomEventCollection CEC = new CustomEventCollection();
        protected BuffCollection        BC  = new BuffCollection();
        
        public BaseBuffSystem BuffOwner { get; private set; }

        public BaseBuff(int layer) {
            this._Layer = layer;
        }

        public void InitBuffOwner(BaseBuffSystem buffOwner) {
            BuffOwner = buffOwner;
        }
        
        public void Init() {
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
            BC.Clear();
            BuffOwner = null;
        }

        public virtual void MergeBuff(BaseBuff otherBuff) {
            this.Layer += otherBuff.Layer;
            OnMerged.Invoke();
        }

        public BuffCache GetBuffCache() {
            return new BuffCache(this, Layer);
        }
    }
}