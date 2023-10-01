using System;
using System.Collections.Generic;
using MyGameExpand;

namespace MyGameUtility {
    public class DamageInvokeByTime : BaseDamageInvokeDelayCom {
        public List<float> InvokeDelayQueue;

        private List<ITimingTaskNode> _AllTimingTaskNodes = new List<ITimingTaskNode>();
        
        public override void RunInvokeProcess() {
            foreach (float delay in InvokeDelayQueue) {
                var timingTaskNode = TimerHelp.AddTimingTask(TimeSpan.FromSeconds(delay), InvokeDamageProcess);
                if (timingTaskNode != null) {
                    _AllTimingTaskNodes.Add(timingTaskNode);
                    timingTaskNode.OnRemoved += () => {
                        _AllTimingTaskNodes.Remove(timingTaskNode);
                        if (_AllTimingTaskNodes.IsNullOrEmpty()) {
                            Recycle();
                        }
                    };
                }
            }
        }
    }
}