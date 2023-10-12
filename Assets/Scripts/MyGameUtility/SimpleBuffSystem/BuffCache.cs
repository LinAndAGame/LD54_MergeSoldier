namespace MyGameUtility {
    public struct BuffCache {
        public BaseBuff BuffRef;
        public int      Layer;

        public BuffCache(BaseBuff buffRef, int layer) {
            BuffRef = buffRef;
            Layer   = layer;
        }

        public void Clear() {
            if (BuffRef.BuffOwner == null) {
                return;
            }
            
            BuffRef.BuffOwner.SetBuffLayerOffset(BuffRef, -Layer);
        }
    }
}