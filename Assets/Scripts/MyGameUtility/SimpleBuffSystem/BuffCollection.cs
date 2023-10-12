using System.Collections.Generic;

namespace MyGameUtility {
    public class BuffCollection {
        private List<BuffCache> _AllBuffCaches = new List<BuffCache>();

        public void Add(BaseBuff buff) {
            _AllBuffCaches.Add(buff.GetBuffCache());
        }

        public void Clear() {
            foreach (var buffCache in _AllBuffCaches) {
                buffCache.Clear();
            }
            _AllBuffCaches.Clear();
        }
    }
}