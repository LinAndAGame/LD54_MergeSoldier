/*
 * 1、对所有元素从大到小排序，倒序查找过时的任务
 * 2、小顶堆对所有元素进行存储，每一帧从小顶堆中取出元素并查看是否过时
 * 3、Round时间轮存储，每个节点记录Round( = targetTime / size),存放在当前之后的第N( = targetTime % size)个格子
 * 4、分层时间轮存储，当前时间轮走过一周时上层时间轮走过一格
 */

using System;
using System.Collections;
using UnityEngine;

namespace MyGameUtility {
    public class TimerAgent : MonoSingletonSimple<TimerAgent> {
        private WaitForEndOfFrame  _WaitForEndOfFrame;
        private WaitForFixedUpdate _WaitForFixedUpdate;
        private RoundTimingWheel   _RoundTimingWheel;

        protected override void Awake() {
            base.Awake();
            DontDestroyOnLoad(this);
            _WaitForEndOfFrame  = new WaitForEndOfFrame();
            _WaitForFixedUpdate = new WaitForFixedUpdate();
            _RoundTimingWheel   = new RoundTimingWheel(512,Mathf.RoundToInt(Time.fixedDeltaTime * 1000f));
        }

        private void FixedUpdate() {
            _RoundTimingWheel.RunOnce();
        }

        public RoundTimingWheel.Node AddTimingTask(TimeSpan timeSpan, Action task) {
            var node = _RoundTimingWheel.AddTimingTask(timeSpan, task);
            if (node != null) {
                node.AttachTo = this;
            }
            return node;
        }

        public ITimingTaskNode AddTimingTaskByUpdateFrame(int frameCount, Action task) {
            if (frameCount <= 0) {
                task?.Invoke();
                return null;
            }

            FrameTimingTask frameTimingTask = new FrameTimingTask(frameCount, task, this);
            frameTimingTask.CoroutineRef = StartCoroutine(delayUpdateFrameTask());
            return frameTimingTask;

            IEnumerator delayUpdateFrameTask() {
                int usedFrameCount = 0;
                for (int i = 0; i < frameTimingTask.RemainingTime; i++) {
                    yield return _WaitForEndOfFrame;
                    usedFrameCount++;
                }

                frameTimingTask.Invoke();
                Debug.Log($"定时任务完成！共用Update帧数【{usedFrameCount}】");
            }
        }

        public ITimingTaskNode AddTimingTaskByFixedUpdateFrame(int frameCount, Action task) {
            if (frameCount <= 0) {
                task?.Invoke();
                return null;
            }

            FrameTimingTask frameTimingTask = new FrameTimingTask(frameCount, task, this);
            frameTimingTask.CoroutineRef = StartCoroutine(delayUpdateFrameTask());
            return frameTimingTask;

            IEnumerator delayUpdateFrameTask() {
                int usedFrameCount = 0;
                for (int i = 0; i < frameTimingTask.RemainingTime; i++) {
                    yield return _WaitForFixedUpdate;
                    usedFrameCount++;
                }

                frameTimingTask.Invoke();
                Debug.Log($"定时任务完成！共用FixedUpdate帧数【{usedFrameCount}】");
            }
        }
    }
}