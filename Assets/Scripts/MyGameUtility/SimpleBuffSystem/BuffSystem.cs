using System.Collections.Generic;

namespace MyGameUtility {
    public class BuffSystem {
        public CustomAction OnAddBuffBefore    = new CustomAction();
        public CustomAction OnAddBuffAfter     = new CustomAction();
        public CustomAction OnNeedToRemoveBuff = new CustomAction();
        public CustomAction OnRemoveBuffAfter = new CustomAction();

        public ValueCacheBool CanAddBuff    = new ValueCacheBool(true);
        public ValueCacheBool CanRemoveBuff = new ValueCacheBool(true);

        private List<BaseBuff> _AllBuffs = new List<BaseBuff>();

        public BaseBuff AddBuff(BaseBuff otherBuff) {
            OnAddBuffBefore.Invoke();
            if (CanAddBuff.GetValue() == false) {
                return null;
            }

            BaseBuff aliveBuff = _AllBuffs.Find(data => data.GetType() == otherBuff.GetType());
            if (aliveBuff == null) {
                _AllBuffs.Add(otherBuff);
                otherBuff.Init();
                OnAddBuffAfter.Invoke();
                return otherBuff;
            }
            else {
                aliveBuff.MergeBuff(otherBuff);
                OnAddBuffAfter.Invoke();
                return aliveBuff;
            }
        }

        public void RemoveBuff(BaseBuff otherBuff) {
            OnNeedToRemoveBuff.Invoke();
            if (CanRemoveBuff.GetValue() == false) {
                return;
            }

            BaseBuff aliveBuff = _AllBuffs.Find(data => data.GetType() == otherBuff.GetType());
            if (aliveBuff != null) {
                _AllBuffs.Remove(aliveBuff);
                aliveBuff.ClearBuff();
                OnRemoveBuffAfter.Invoke();
            }
        }

        public void RemoveBuff<T>() where T : BaseBuff {
            BaseBuff aliveBuff = _AllBuffs.Find(data => data.GetType() == typeof(T));
            RemoveBuff(aliveBuff);
        }

        public void Clear() {
            foreach (var buff in _AllBuffs) {
                RemoveBuff(buff);
            }
            
            _AllBuffs.Clear();
        }
    }
}