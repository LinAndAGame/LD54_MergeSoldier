using UnityEngine;

namespace MyGameUtility {
    public abstract class BaseDamageInvokeDelayCom : MonoBehaviour {
        private IOperateGiveDamageUnit OperateGiveDamageUnit;
        
        public void Init(IOperateGiveDamageUnit operateGiveDamageUnit) {
            OperateGiveDamageUnit = operateGiveDamageUnit;
            this.gameObject.SetActive(true);
        }

        protected void InvokeDamageProcess() {
            OperateGiveDamageUnit.InvokeDamage();
        }

        protected void Recycle() {
            OperateGiveDamageUnit = null;
            this.gameObject.SetActive(false);
            MyPoolSimpleComponent.Release(this);
        }
        
        public abstract void RunInvokeProcess();
    }
}