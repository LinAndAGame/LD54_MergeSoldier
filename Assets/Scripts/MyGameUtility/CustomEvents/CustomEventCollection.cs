using System.Collections.Generic;

namespace MyGameUtility {
    public class CustomEventCollection {
        private List<CustomEventMark> _AllCache = new List<CustomEventMark>();
        
        public void Add(CustomEventMark target) {
            _AllCache.Add(target);
        }

        public void Clear() {
            foreach (CustomEventMark customEventMark in _AllCache) {
                customEventMark.KillTimesEvent();
            }
            _AllCache.Clear();
        }
    }
}