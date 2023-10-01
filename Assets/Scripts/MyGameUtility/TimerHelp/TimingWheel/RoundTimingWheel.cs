/*
 * Round时间轮 ： 每经过一格，各自上所有节点的Round-1，格子中Round为0的节点直接触发
 */

using System;
using System.Collections.Generic;
using UnityEngine;

namespace MyGameUtility {
    public class RoundTimingWheel {
        private int _CurIndex;
        private int _Count;

        private readonly int                      _Interval;
        private readonly int                      _Size;
        private readonly int                      _ModValue;
        private readonly Bucket[] _AllBuckets;
        private readonly int                      _Tick;

        public RoundTimingWheel(int size,int tick) {
            _Size     = Mathf.NextPowerOfTwo(size);
            _Tick     = tick;
            _Interval = _Size * _Tick;
            _ModValue = _Size - 1;

            _AllBuckets = new Bucket[size];
            for (int i = 0; i < _AllBuckets.Length; i++) {
                _AllBuckets[i] = new Bucket();
            }
        }

        public Node AddTimingTask(TimeSpan delay, System.Action task) {
            int delayMs = (int) delay.TotalMilliseconds;
            if (delayMs <= 0) {
                task?.Invoke();
                return null;
            }

            int nodeIndex = 0;
            nodeIndex = (_CurIndex + delayMs / _Tick + 1) & _ModValue;
             _Count++;
             Debug.Log("TimerHelp,Count++");

            return _AllBuckets[nodeIndex].AddTask(this, delayMs, task);
        }

        public void RunOnce() {
            if (_Count <= 0) {
                return;
            }

            _CurIndex++;
            if (_CurIndex >= _Size) {
                _CurIndex = 0;
            }

            _AllBuckets[_CurIndex].TryInvokeTasks();
        }

        private class Bucket {
            private LinkedList<Node> _AllNodes = new LinkedList<Node>();

            public Node AddTask(RoundTimingWheel timingWheel, int remainingTime, System.Action task) {
                Node node = new Node(timingWheel, remainingTime, task);
                _AllNodes.AddLast(node.LinkedListNode);
                return node;
            }

            public void AddTask(Node node) {
                _AllNodes.AddLast(node.LinkedListNode);
            }

            public void TryInvokeTasks() {
                if (_AllNodes.Count == 0) {
                    return;
                }

                var curNode = _AllNodes.First;
                while (curNode != null) {
                    var nextNode = curNode.Next;
                    curNode.Value.Invoke();
                    curNode = nextNode;
                }
            }
        }

        public class Node : ITimingTaskNode {

            public readonly LinkedListNode<Node> LinkedListNode;

            private TimingNodeStateEnum _CurTimingNodeState = TimingNodeStateEnum.WaitForInvoke;
            private int                 _RemainingTime; // 此数值用int存储存在越界的风险，可以用一种数据结构对其进行存储，与分层时间轮本质相同，将剩余时间通过一种计算方式做到不能越界的存储
            private int                 _LastWheelIndex;

            private readonly RoundTimingWheel _RoundTimingWheelRef;
            private readonly int              _Interval;
            private readonly System.Action    _Task;

            public Node(RoundTimingWheel timingWheel, int remainingTime, Action task) {
                _RoundTimingWheelRef = timingWheel;
                _Interval            = _RoundTimingWheelRef._Interval;
                _RemainingTime       = remainingTime;
                _Task                = task;
                _LastWheelIndex      = _RoundTimingWheelRef._CurIndex;
                LinkedListNode       = new LinkedListNode<Node>(this);
            }

            public event Action OnRemoved;
            public event Action OnInvoked;
            public Component    AttachTo { get; set; }

            public void SetOffset(int offset) {
                int curWheelIndex  = _RoundTimingWheelRef._CurIndex;
                int wheelRunOffset = curWheelIndex > _LastWheelIndex ? curWheelIndex - _LastWheelIndex : _RoundTimingWheelRef._Size - _LastWheelIndex + curWheelIndex;
                _RemainingTime -= wheelRunOffset * _RoundTimingWheelRef._Tick; // 再减去当前时间轮经过的时间
                _RemainingTime += offset;
                
                if (_RemainingTime <= 0) {
                    IntervalInvoke();
                    InternalRemoveFromBucket();
                }
                else {
                    _LastWheelIndex = _RoundTimingWheelRef._CurIndex;
                    InternalRemoveFromBucket();
                    int nodeIndex = (_RoundTimingWheelRef._CurIndex + _RemainingTime / _RoundTimingWheelRef._Tick + 1) & _RoundTimingWheelRef._ModValue;
                    _RoundTimingWheelRef._AllBuckets[nodeIndex].AddTask(this);
                }
            }

            public void Remove() {
                if (_CurTimingNodeState != TimingNodeStateEnum.WaitForInvoke) {
                    return;
                }

                _CurTimingNodeState = TimingNodeStateEnum.WaitForRemove;
                _RoundTimingWheelRef._Count--;
                Debug.Log("TimerHelp  Remove , Count--");
                OnRemoved?.Invoke();
            }

            public void Invoke() {
                if (_CurTimingNodeState == TimingNodeStateEnum.WaitForRemove || AttachTo == null) {
                    InternalRemoveFromBucket();
                    return;
                }

                _RemainingTime -= _Interval;
                if (_RemainingTime <= 0) {
                    InternalRemoveFromBucket();
                    IntervalInvoke();
                }
                else {
                    _LastWheelIndex = _RoundTimingWheelRef._CurIndex;
                }
            }

            private void IntervalInvoke() {
                _RoundTimingWheelRef._Count--;
                _Task?.Invoke();
                Debug.Log("TimerHelp  Invoke , Count--");
                OnInvoked?.Invoke();
            }

            private void InternalRemoveFromBucket() {
                LinkedListNode.List.Remove(LinkedListNode);
                _CurTimingNodeState = TimingNodeStateEnum.Finished;
            }
        }
        
        private enum TimingNodeStateEnum {
            WaitForInvoke,
            WaitForRemove,
            Finished,
        }
    }
}