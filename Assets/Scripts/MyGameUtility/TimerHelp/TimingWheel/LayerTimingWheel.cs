/*
 * https://www.lpnote.com/2017/11/15/hashed-and-hierarchical-timing-wheels/
 * https://zhuanlan.zhihu.com/p/102476356
 * https://www.rafalesoftware.com/
 * 分层时间轮 ： 当前层走完一轮后上层走一格
 */

namespace MyGameUtility {
    public class LayerTimingWheel {
        private readonly int _Interval;
        private readonly int _WheelSize;
        private readonly int _TickTime;
        
        private LayerTimingWheel      _UpTimingWheel;
        public LayerTimingWheel      _HeadTimingWheel;
        private LayerTimingTaskList[] _AllLists;

        public int CurrentTime = -1;

        public LayerTimingWheel(int wheelSize, int tickTime,LayerTimingWheel headTimingWheel = null) {
            _WheelSize       = wheelSize;
            _TickTime        = tickTime;
            _Interval        = _WheelSize * _TickTime;
            _HeadTimingWheel = headTimingWheel;
            _AllLists        = new LayerTimingTaskList[_WheelSize];

            for (int i = 0; i < _WheelSize; i++) {
                _AllLists[i] = new LayerTimingTaskList();
            }
        }

        public void Run(int offset) {
            for (int i = 0; i < offset; i++) {
                RunOnce();
            }
        }

        public void RunOnce() {
            CurrentTime++;
            if (CurrentTime >= _WheelSize) {
                CurrentTime = 0;
                
                if (_UpTimingWheel != null) {
                    _UpTimingWheel.RunOnce();
                }
            }

            if (_HeadTimingWheel == null) {
                foreach (var timingTask in _AllLists[CurrentTime]._AllTimingTask) {
                    timingTask.Invoke();
                }
            }
            else {
                foreach (var timingTask in _AllLists[CurrentTime]._AllTimingTask) {
                    _HeadTimingWheel.AddTimingTask(timingTask);
                }
            }
            _AllLists[CurrentTime]._AllTimingTask.Clear();
        }

        public bool AddTimingTask(FrameTimingTask task) {
            int totalDelayMs = task.RemainingTime;
            if (totalDelayMs == CurrentTime) {
                // if (task.IsCanceled == false) {
                //     task.Invoke();
                // }

                return false;
            }

            if (totalDelayMs < _TickTime) {
                return _HeadTimingWheel.AddTimingTask(task);
            }
            else {
                if (totalDelayMs <= _Interval) {
                    int index = (CurrentTime + (totalDelayMs / _TickTime)) % _WheelSize;
                    _AllLists[index].AddTask(task);
                    task.RemainingTime -= (totalDelayMs / _TickTime) * _TickTime;
                    return true;
                }
                else {
                    // 上层时间轮尝试添加此任务
                    return GetUpTimingWheel().AddTimingTask(task);
                }
            }
        }

        private LayerTimingWheel GetUpTimingWheel() {
            if (_UpTimingWheel == null) {
                _UpTimingWheel = new LayerTimingWheel(_WheelSize, _Interval,_HeadTimingWheel);
            }

            return _UpTimingWheel;
        }
    }
}