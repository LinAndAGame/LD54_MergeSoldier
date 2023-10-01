using System.Collections.Generic;

namespace MyGameUtility {
    public class LayerTimingTaskList {
        public List<FrameTimingTask> _AllTimingTask = new List<FrameTimingTask>();

        public void AddTask(FrameTimingTask frameTimingTask) {
            _AllTimingTask.Add(frameTimingTask);
        }
    }
}