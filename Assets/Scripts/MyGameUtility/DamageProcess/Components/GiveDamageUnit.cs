/*
 * 从 OpenDetection 到 CloseDetection 为一个检测区间
 * 功能 : 挂在这个组件的物体可以对受击者发起一次伤害结算
 * 攻击间隔分为 ： 手动设置、固定时长
 * 触发次数 ： 自身统计次数（陷阱，固定触发N次）、根据被攻击者统计次数（普通武器，通常一次攻击激活时最多触发1次）
 * 可以做的伤害武器类型 : 普通武器,电锯,伤害型场景道具
 */

using System;
using System.Collections.Generic;
using Damage;
using JetBrains.Annotations;
using MyGameExpand;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

namespace MyGameUtility {
    [RequireComponent(typeof(PhysicalEventTrigger))]
    public abstract class GiveDamageUnit<T, T2, T3, T4> : MonoBehaviour, IOperateGiveDamageUnit 
        where T2 : BaseAtkData<T, T4> 
        where T3 : BaseDefData<T, T4> 
        where T : IComparable<T>
        where  T4 : BaseHealthGroupInfo<T> {

        public Rigidbody   RigidbodyRef;
        public Rigidbody2D Rigidbody2DRef;
        public bool        IgnoreSelfBeDamageUnit = true;

        [ShowIf("@IgnoreSelfBeDamageUnit")]
        public Transform IgnoreSelfBeDamageUnitSearchHead;

        public List<ReceiveDamageUnit<T,T3,T4>> IgnoreBeDamageUnits;
        public DamageCalculateEnum           DamageCalculateType    = DamageCalculateEnum.BaseOnSelf;  // 对下一次激活时间的计算，用公用的时间计算，自己拥有单独的时间进行计算
        public DamageCloseEnum               DamageCloseType        = DamageCloseEnum.Manuelly;        // 怎样此次检测
        public DamageTriggerTimesEnum        DamageTriggerTimesType = DamageTriggerTimesEnum.Infinity; // 对于一个相同的目标，怎样使其从检测队列中移除

        public BaseDamageInvokeDelayCom DelayInvokePrefab;  // 延迟伤害结算的对象

        [ShowIf("@_CloseByDelay")]
        public float CloseDelay = 0.5f; // 从开始检测到检测结束的时长，只有在固定时长检测模式下有效
        [ShowIf("@_CloseByTriggerTimes")]
        public int CloseTriggerTimes = 1;     // 在一次激活时间内，总共可以激活几次
        [ShowIf("@TriggerSameOneByFixedTimes")]
        public int MaxTriggerTimerToSameOne = 1;

        public float                TriggerInterval = 0.5f; // 激活的间隔时多少，只有在激活次数大于1时才有意义
        public PhysicalEventTrigger PETRef;

        private List<DamageObjData>                 _AllDamageObjDatas = new List<DamageObjData>();
        private float                               _NextRemoveTime;
        private HashSet<ReceiveDamageUnit<T,T3,T4>> _IgnoreBeDamageUnits = new HashSet<ReceiveDamageUnit<T,T3,T4>>();
        private CustomEventCollection                    _EventCache          = new CustomEventCollection();
        [CanBeNull]
        private Dictionary<ReceiveDamageUnit<T,T3,T4>, DamageObjData> _DetectionMap = new Dictionary<ReceiveDamageUnit<T,T3,T4>, DamageObjData>();
        private                                                                            int _CurInvokeTimes;
        [ShowInInspector, ReadOnly]
        private bool                                         _CurDetecting;
        private List<ITimingTaskNode>                        _AllTimingTaskNodes = new List<ITimingTaskNode>();

        private bool _CloseByDelay        => DamageCloseType == DamageCloseEnum.ByDelay;
        private bool _CloseByTriggerTimes => DamageCloseType == DamageCloseEnum.ByTriggerTimes;
        private bool TriggerSameOneByFixedTimes => DamageTriggerTimesType == DamageTriggerTimesEnum.FixedTimes;
        
        [ShowInInspector, ReadOnly]
        public T2 AttackDataRef { get; private set; }


        private void OnValidate() {
            if (IgnoreSelfBeDamageUnitSearchHead == null) {
                IgnoreSelfBeDamageUnitSearchHead = this.transform;
            }

            if (PETRef == null) {
                PETRef = this.GetComponent<PhysicalEventTrigger>();
            }
        }

        public void SetAttackData(T2 attackData){
            this.AttackDataRef = attackData;
        }

        public void OpenDetection(float closeDelay) {
            DamageCloseType = DamageCloseEnum.ByDelay;
            CloseDelay      = closeDelay;
            OpenDetection();
        }

        [Button]
        public void OpenDetection() {
            if (RigidbodyRef != null) {
                RigidbodyRef.WakeUp();
            }
            if (Rigidbody2DRef != null) {
                Rigidbody2DRef.WakeUp();
            }
            
            // 对象池优化，预加载
            if (DelayInvokePrefab != null) {
                MyPoolSimpleComponent.Preload(5, DelayInvokePrefab);
            }

            ClearData();
            if (IgnoreSelfBeDamageUnit) {
                _IgnoreBeDamageUnits.AddRange(IgnoreSelfBeDamageUnitSearchHead.GetComponentsInChildren<ReceiveDamageUnit<T,T3,T4>>());
            }

            _IgnoreBeDamageUnits.AddRange(IgnoreBeDamageUnits);

            PETRef.OnTriggerEnter2DAct.AddListener((other) => { AddDamageDetection(other.GetComponent<ReceiveDamageUnit<T,T3,T4>>()); }, _EventCache);
            PETRef.OnTriggerStay2DAct.AddListener((other) => { AddDamageDetection(other.GetComponent<ReceiveDamageUnit<T,T3,T4>>()); }, _EventCache);
            PETRef.OnTriggerExit2DAct.AddListener((other) => { RemoveDamageDetection(other.GetComponent<ReceiveDamageUnit<T,T3,T4>>()); }, _EventCache);

            if (_CloseByDelay) {
                _AllTimingTaskNodes.Add(TimerHelp.AddTimingTask(TimeSpan.FromSeconds(CloseDelay), CloseDetection));
            }

            _CurDetecting = true;
        }

        [Button]
        public void CloseDetection() {
            ClearData();
        }

        public void InvokeDamage() {
            
        }

        private void ClearData() {
            _NextRemoveTime = 0;
            _CurDetecting   = false;
            _EventCache.Clear();
            StopAllCoroutines();
            _AllDamageObjDatas.Clear();
            _IgnoreBeDamageUnits.Clear();
            _DetectionMap.Clear();
            _CurInvokeTimes = 0;
            foreach (ITimingTaskNode timingTaskNode in _AllTimingTaskNodes) {
                timingTaskNode.Remove();
            }
        }

        private void Update() {
            if (_CurDetecting == false) {
                return;
            }

            if (_AllDamageObjDatas.IsNullOrEmpty()) {
                return;
            }

            if (Time.time >= _NextRemoveTime) {
                _NextRemoveTime = Time.time + TriggerInterval;
            }
            
            for (int i = _AllDamageObjDatas.Count - 1; i >= 0; i--) {
                var damageObjData = _AllDamageObjDatas[i];
                var invokeSucceed = damageObjData.TryInvokeDamageProcess();
                if (invokeSucceed) {
                    _CurInvokeTimes++;
                    // 根据总的触发次数关闭检测
                    if (_CloseByTriggerTimes) {
                        if (_CurInvokeTimes >= CloseTriggerTimes) {
                            CloseDetection();
                            return;
                        }
                    }
                }
                if (damageObjData.RemainingDamageTimes <= 0) {
                    _IgnoreBeDamageUnits.Add(damageObjData.ReceiveDamageUnitRef);
                }
            }
        }

        private void AddDamageDetection(ReceiveDamageUnit<T,T3,T4> receiveDamageUnit) {
            if (receiveDamageUnit == null) {
                return;
            }

            if (IgnoreBeDamageUnits.Contains(receiveDamageUnit)) {
                return;
            }

            if (_DetectionMap.ContainsKey(receiveDamageUnit)) {
                return;
            }

            var damageObjData = new DamageObjData(this, receiveDamageUnit, DamageTriggerTimesType, MaxTriggerTimerToSameOne);
            _AllDamageObjDatas.Add(damageObjData);
            _DetectionMap.Add(receiveDamageUnit, damageObjData);
        }

        private void RemoveDamageDetection(ReceiveDamageUnit<T,T3,T4> receiveDamageUnit) {
            if (receiveDamageUnit == null) {
                return;
            }
            if (_DetectionMap.ContainsKey(receiveDamageUnit) == false) {
                return;
            }

            _AllDamageObjDatas.Remove(_DetectionMap[receiveDamageUnit]);
            _DetectionMap.Remove(receiveDamageUnit);
        }

        #region 编辑器操作

        [Button]
        private void 添加默认2D碰撞体() {
            if (this.GetComponent<BoxCollider2D>() == null) {
                var boxCollider2D = this.gameObject.AddComponent<BoxCollider2D>();
                boxCollider2D.isTrigger = true;
            }

            if (this.GetComponent<Rigidbody2D>() == null) {
                var rigidbody2D = this.gameObject.AddComponent<Rigidbody2D>();
                rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
            }
            
            PETRef.Collider2DSelf = this.GetComponent<Collider2D>();
            Rigidbody2DRef        = this.GetComponent<Rigidbody2D>();
        }

        #endregion


        public enum DamageCalculateEnum {
            BaseOnSelf,
            BaseOnTarget,
        }

        public enum DamageTriggerTimesEnum {
            Infinity,
            FixedTimes,
        }

        public enum DamageCloseEnum {
            Manuelly,
            ByDelay,
            ByTriggerTimes,
        }

        private class DamageObjData {
            public readonly ReceiveDamageUnit<T,T3,T4> ReceiveDamageUnitRef;
            public readonly GiveDamageUnit<T,T2,T3,T4>    GiveDamageUnitRef;

            private readonly DamageTriggerTimesEnum DamageTriggerTimesType;

            public  int   RemainingDamageTimes { get; private set; }
            private float _NextInvokeTime;

            public DamageObjData(GiveDamageUnit<T,T2,T3,T4> giveDamageUnitRef, ReceiveDamageUnit<T,T3,T4> receiveDamageUnitRef, DamageTriggerTimesEnum damageTriggerTimesType, int remainingDamageTimes) {
                ReceiveDamageUnitRef   = receiveDamageUnitRef;
                GiveDamageUnitRef      = giveDamageUnitRef;
                DamageTriggerTimesType = damageTriggerTimesType;
                if (DamageTriggerTimesType == DamageTriggerTimesEnum.Infinity) {
                    RemainingDamageTimes = 1;
                }
                else {
                    RemainingDamageTimes = remainingDamageTimes;
                }

                _NextInvokeTime = Time.time;
            }

            public bool TryInvokeDamageProcess() {
                if (RemainingDamageTimes <= 0) {
                    return false;
                }

                if (ReceiveDamageUnitRef.CanBeDamage == false) {
                    return false;
                }

                if (Time.time < _NextInvokeTime) {
                    return false;
                }

                if (GiveDamageUnitRef.DamageCalculateType == DamageCalculateEnum.BaseOnTarget) {
                    _NextInvokeTime = Time.time + GiveDamageUnitRef.TriggerInterval;
                }
                else if (GiveDamageUnitRef.DamageCalculateType == DamageCalculateEnum.BaseOnSelf) {
                    _NextInvokeTime = GiveDamageUnitRef._NextRemoveTime;
                }

                // 对象池优化
                if (GiveDamageUnitRef.DelayInvokePrefab != null) {
                    var ins = MyPoolSimpleComponent.Get(GiveDamageUnitRef.DelayInvokePrefab);
                    ins.Init(GiveDamageUnitRef);
                    ins.RunInvokeProcess();
                }
                else {
                    // 没有延迟触发预制体就直接触发伤害计算
                    DamageProcess.CalculateDamage<T,T2,T3,T4>(GiveDamageUnitRef.AttackDataRef, ReceiveDamageUnitRef.DefDataRef);
                }

                if (GiveDamageUnitRef.TriggerSameOneByFixedTimes) {
                    RemainingDamageTimes--;
                }

                return true;
            }
        }
    }
}