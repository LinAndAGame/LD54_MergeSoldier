using System;
using System.Collections.Generic;
using MyGameExpand;

namespace MyGameUtility {
    [Serializable]
    public abstract class BaseHealthGroupInfo<T> where T : IComparable<T> {
        protected BaseHealthSingleInfo<T>[,] AllSingleInfos;

        protected BaseHealthGroupInfo() {
            Init();
            RefreshIndex();
        }

        public virtual void ReduceValue(BaseHealthSingleInfo<T> start, BaseHealthSingleInfo<T> end, T remainingValue) {
            var reduceQueue = GetReduceQueue(start,end);
            foreach (var healthSingleInfo in reduceQueue) {
                remainingValue = healthSingleInfo.ReduceValue(remainingValue);
                if (healthSingleInfo.IsNoRemainingValue()) {
                    return;
                }
            }
        }
        public virtual void ReduceValue(List<BaseHealthSingleInfo<T>> forcePath, T remainingValue) {
            foreach (var healthSingleInfo in forcePath) {
                remainingValue = healthSingleInfo.ReduceValue(remainingValue);
                if (healthSingleInfo.IsNoRemainingValue()) {
                    return;
                }
            }
        }

        private List<BaseHealthSingleInfo<T>> GetReduceQueue(BaseHealthSingleInfo<T> start, BaseHealthSingleInfo<T> end) {
            List<BaseHealthSingleInfo<T>> result = new List<BaseHealthSingleInfo<T>>();
            while (start != null) {
                result.Add(start);
                if (start == end) {
                    return result;
                }
                start = start.Parent;
            }

            return new List<BaseHealthSingleInfo<T>>();
        }

        protected void RefreshIndex() {
            int width = AllSingleInfos.GetLength(0);
            for (var i0 = 0; i0 < width; i0++)
            for (var i1 = 0; i1 < AllSingleInfos.GetLength(1); i1++) {
                var healthSingleInfo = AllSingleInfos[i0, i1];
                if (healthSingleInfo == null) {
                    continue;
                }

                healthSingleInfo.Index = i0 + i1 * width;
            }
        }

        protected abstract void Init();
    }
}