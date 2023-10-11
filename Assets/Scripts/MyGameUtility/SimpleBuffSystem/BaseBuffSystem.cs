using System.Collections.Generic;
using MyGameExpand;

namespace MyGameUtility {
    public abstract class BaseBuffSystem {
        public CustomAction OnAddBuffBefore    = new CustomAction();
        public CustomAction OnAddBuffAfter     = new CustomAction();
        public CustomAction OnNeedToRemoveBuff = new CustomAction();
        public CustomAction OnRemoveBuffAfter = new CustomAction();

        public ValueCacheBool CanAddBuff    = true;
        public ValueCacheBool CanRemoveBuff = true;

        private List<BaseBuff> _AllBuffs = new List<BaseBuff>();

        public virtual void AddBuff(BaseBuff otherBuff, BuffCollection buffCollection = null) {
            OnAddBuffBefore.Invoke();
            if (CanAddBuff.GetValue() == false) {
                return;
            }
            
            buffCollection?.Add(otherBuff);
            BaseBuff aliveBuff = _AllBuffs.Find(data => data.GetType() == otherBuff.GetType());
            if (aliveBuff == null) {
                _AllBuffs.Add(otherBuff);
                otherBuff.Init(this);
                OnAddBuffAfter.Invoke();
            }
            else {
                aliveBuff.MergeBuff(otherBuff);
            }
        }

        public virtual void AddBuffs(List<BaseBuff> readyAddedBuffs, BuffCollection buffCollection = null) {
            if (readyAddedBuffs.IsNullOrEmpty()) {
                return;
            }
            
            foreach (var readyAddedBuff in readyAddedBuffs) {
                if (readyAddedBuff == null) {
                    continue;
                }
                
                AddBuff(readyAddedBuff,buffCollection);
            }
        }

        public virtual void RemoveBuffLayer(BaseBuff otherBuff) {
            if (otherBuff.BuffOwner != this) {
                return;
            }
            
            BaseBuff aliveBuff = GetAliveBuff(otherBuff);
            if (aliveBuff!=null) {
                aliveBuff.Layer -= otherBuff.Layer;
            }
            otherBuff.ClearBuff();
        }

        public virtual void RemoveBuff(BaseBuff otherBuff) {
            OnNeedToRemoveBuff.Invoke();
            if (CanRemoveBuff.GetValue() == false) {
                return;
            }

            BaseBuff aliveBuff = GetAliveBuff(otherBuff);
            if (aliveBuff != null) {
                _AllBuffs.Remove(aliveBuff);
                aliveBuff.ClearBuff();
                OnRemoveBuffAfter.Invoke();
            }
        }

        public virtual void RemoveBuff<T>() where T : BaseBuff {
            BaseBuff aliveBuff = _AllBuffs.Find(data => data.GetType() == typeof(T));
            RemoveBuff(aliveBuff);
        }

        public virtual void Clear() {
            foreach (var buff in _AllBuffs) {
                RemoveBuff(buff);
            }
            
            _AllBuffs.Clear();
        }

        protected virtual BaseBuff GetAliveBuff(BaseBuff otherBuff) {
            BaseBuff aliveBuff = _AllBuffs.Find(data => data.GetType() == otherBuff.GetType());
            return aliveBuff;
        }
    }
}