using System;
using UnityEngine;

namespace MyGameUtility {
    public static class TimerHelp {
        static TimerHelp() {
            if (TimerAgent.I == null) {
                new GameObject("计时器帮助单例对象",typeof(TimerAgent));
                Debug.Log($"创建计时器帮助单例对象，路径：{OtherUtility.GetScenePath(TimerAgent.I.gameObject)}");
            }
        }

        public static ITimingTaskNode AddTimingTask(TimeSpan timingDelayData, System.Action timingTaskAct) {
            return TimerAgent.I.AddTimingTask(timingDelayData, timingTaskAct);
        }
        public static ITimingTaskNode AddTimingTaskByUpdateFrame(int frameCount, System.Action timingTaskAct) {
            return TimerAgent.I.AddTimingTaskByUpdateFrame(frameCount, timingTaskAct);
        }
        public static ITimingTaskNode AddTimingTaskByFixedUpdateFrame(int frameCount, System.Action timingTaskAct) {
            return TimerAgent.I.AddTimingTaskByFixedUpdateFrame(frameCount, timingTaskAct);
        }

        // TODO : 每隔一段事件触发一次
        public static ITimingTaskNode AddLoopTimingTask(TimeSpan interval, System.Action timingTaskAct) {
            ITimingTaskNode node = null;
            initEvents();
            return node;

            void initEvents() {
                node = TimerAgent.I.AddTimingTask(interval, timingTaskAct);
                node.OnRemoved += () => {
                    initEvents();
                };
            }
        }
    }
}