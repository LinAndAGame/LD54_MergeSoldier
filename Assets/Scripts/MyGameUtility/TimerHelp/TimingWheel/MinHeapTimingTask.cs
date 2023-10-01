using System;

namespace MyGameUtility {
    public class MinHeapTimingTask : IComparable<MinHeapTimingTask> {
        public float         EndTime;
        public System.Action Task;
        public System.Action OnUpdate;

        public MinHeapTimingTask(float endTime, Action task) {
            EndTime = endTime;
            Task    = task;
        }

        public void ModifyEndTime(float offset) {
            EndTime += offset;
        }

        public void InvokeTask() {
            Task?.Invoke();
        }

        public int CompareTo(MinHeapTimingTask other) {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            return EndTime.CompareTo(other.EndTime);
        }
    }
}