using System;
using UnityEngine;

namespace MyGameUtility {
    public abstract class MonoSingletonSimple<T> : MonoBehaviour where T : Component{
        public static T I;

        protected virtual void Awake() {
            I = this.GetComponent<T>();
        }
    }
}