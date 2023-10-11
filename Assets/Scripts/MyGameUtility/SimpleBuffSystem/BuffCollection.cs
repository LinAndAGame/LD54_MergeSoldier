using System.Collections.Generic;

namespace MyGameUtility {
    public class BuffCollection {
        private List<BaseBuff> _AllBuffCaches = new List<BaseBuff>();

        public void Add(BaseBuff buff) {
            _AllBuffCaches.Add(buff);
        }

        public void Clear() {
            foreach (var buffCache in _AllBuffCaches) {
                buffCache.RemoveFromBuffOwner();
            }
            _AllBuffCaches.Clear();
        }
    }
}