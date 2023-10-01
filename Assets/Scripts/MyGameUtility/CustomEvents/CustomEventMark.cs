using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MyGameUtility {
    [Serializable]
    public class CustomEventMark : IEquatable<CustomEventMark> {
        [HideInInspector]
        public System.Action OnEventNoTimes;

        [ShowInInspector, ReadOnly]
        public readonly object TargetAction;

        public readonly CustomEventAddInfo AddInfo;

        [ShowInInspector, ReadOnly]
        private int _AllTimes;
        public int AllTimes {
            get => _AllTimes;
            set {
                _AllTimes = value;
                if (_AllTimes <= 0) {
                    OnEventNoTimes?.Invoke();
                }
            }
        }

        public CustomEventMark(object targetAction, CustomEventAddInfo addInfo) {
            // 时间周期性事件，需要设置存活时间，每次激活时减少的次数，累加的次数（为null则在上一次的基础上进行累加）
            TargetAction = targetAction;
            AddInfo      = addInfo;
            _AllTimes    = AddInfo.DefaultTimes;
        }

        public bool IsContain(object targetAction) {
            return TargetAction != null && targetAction != null && TargetAction.Equals(targetAction);
        }

        public void KillTimesEvent() {
            AllTimes = 0;
        }

        public void ReduceTime() {
            AllTimes -= AddInfo.ReduceTimes;
        }

        public void AddTime() {
            AllTimes += AddInfo.AddTimes;
        }

        public bool Equals(CustomEventMark other) {
            if (other == null) {
                return false;
            }

            if (this.TargetAction.Equals(other.TargetAction)) {
                if ((this.AddInfo.ReduceTimes != other.AddInfo.ReduceTimes || this.AddInfo.AddTimes != other.AddInfo.AddTimes)) {
                    Debug.LogError("相比较的两个时间周期性事件的委托一样，但是加减策略不同，所以不认为是相同的实例对象！有可能出错！");
                    return false;
                }
                else {
                    return true;
                }
            }
            else {
                return false;
            }
        }
        
        public bool Equals(object otherAction, int reduceTimes, int addTimes) {
            if (this.TargetAction.Equals(otherAction)) {
                if ((this.AddInfo.ReduceTimes != reduceTimes || this.AddInfo.AddTimes != addTimes)) {
                    Debug.LogError("相比较的两个时间周期性事件的委托一样，但是加减策略不同，所以不认为是相同的实例对象！有可能出错！");
                    return false;
                }
                else {
                    return true;
                }
            }
            else {
                return false;
            }
        }
    }
}