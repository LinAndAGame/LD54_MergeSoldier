using UnityEngine;

namespace MyGameUtility {
    public interface ITimingTaskNode {
        public event System.Action OnRemoved;
        public event System.Action OnInvoked;

        public Component AttachTo { get; set; }
        
        void SetOffset(int offset);

        void Remove();

        void Invoke();
    }
}