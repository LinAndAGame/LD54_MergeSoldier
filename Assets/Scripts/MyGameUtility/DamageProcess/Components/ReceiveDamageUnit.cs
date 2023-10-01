/*
 * 功能 : 挂在这个组件的对象可以被攻击
 */

using System;
using JetBrains.Annotations;
using MyGameUtility;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Damage {
    [RequireComponent(typeof(PhysicalEventTrigger))]
    public abstract class ReceiveDamageUnit<T,T1,T2> : MonoBehaviour 
        where T1 : BaseDefData<T, T2> 
        where T : IComparable<T>
    where T2 : BaseHealthGroupInfo<T> {
        public PhysicalEventTrigger PETRef;

        [SerializeField,HideInInspector]
        private bool _CanBeDamage;
        
        [ShowInInspector]
        public T1 DefDataRef { get; private set; }

        [ShowInInspector]
        public bool CanBeDamage {
            get => _CanBeDamage;
            set {
                _CanBeDamage = value;
                PETRef.SetEnableOfCollider(_CanBeDamage);
            }
        }

        private void OnValidate() {
            if (PETRef == null) {
                PETRef = this.GetComponent<PhysicalEventTrigger>();
            }
        }

        public void SetDefData(T1 defData) {
            DefDataRef = defData;
        }
    }
}