using System;
using UnityEngine;

namespace MyGameUtility {
    public class FrameTimingTask : ITimingTaskNode {
        public event Action OnRemoved;
        public event Action OnInvoked;
        public Component    AttachTo { get; set; }
        
        public int          RemainingTime;
        public Coroutine    CoroutineRef;

        private readonly System.Action _SourceTask;

        public FrameTimingTask(int remainingTime, Action sourceTask, Component attachTo) {
            RemainingTime = remainingTime;
            _SourceTask   = sourceTask;
            AttachTo      = attachTo;
        }

        public void SetOffset(int offset) {
            RemainingTime += offset;
        }

        public void Remove() {
            if (CoroutineRef == null) {
                Debug.Log("协程已关闭");
                return;
            }
            
            TimerAgent.I.StopCoroutine(CoroutineRef);
            CoroutineRef = null;
        }

        public void Invoke() {
            if (AttachTo == null) {
                return;
            }
            
            _SourceTask?.Invoke();
        }
    }
}