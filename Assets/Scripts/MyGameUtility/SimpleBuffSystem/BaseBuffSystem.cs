using System;
using System.Collections.Generic;
using MyGameExpand;
using Sirenix.OdinInspector;

namespace MyGameUtility {
    [Serializable]
    public abstract class BaseBuffSystem {
        public CustomAction OnAddBuffBefore    = new CustomAction();
        public CustomAction OnAddBuffAfter     = new CustomAction();
        public CustomAction OnNeedToRemoveBuff = new CustomAction();
        public CustomAction OnRemoveBuffAfter = new CustomAction();

        public ValueCacheBool CanAddBuff    = true;
        public ValueCacheBool CanRemoveBuff = true;

        [ShowInInspector, ReadOnly]
        private List<BaseBuff> _AllBuffs = new List<BaseBuff>();

        public virtual void AddBuff(BaseBuff otherBuff, BuffCollection buffCollection = null) {
            OnAddBuffBefore.Invoke();
            if (CanAddBuff.GetValue() == false) {
                return;
            }

            
            otherBuff.InitBuffOwner(this);
            buffCollection?.Add(otherBuff);
            BaseBuff aliveBuff = _AllBuffs.Find(data => data.GetType() == otherBuff.GetType());
            if (aliveBuff == null) {
                _AllBuffs.Add(otherBuff);
                otherBuff.Init();
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
                aliveBuff.SetLayerOffset(otherBuff.Layer);
            }
            otherBuff.ClearBuff();
        }

        public virtual void SetBuffLayerOffset(BaseBuff target, int offsetLayer) {
            if (target.BuffOwner == null || target.BuffOwner != this) {
                return;
            }
            
            var aliveBuff = GetAliveBuff(target);
            if (aliveBuff == null) {
                return;
            }

            aliveBuff.SetLayerOffset(offsetLayer);
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
            for (var i = _AllBuffs.Count - 1; i >= 0; i--) {
                var buff = _AllBuffs[i];
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