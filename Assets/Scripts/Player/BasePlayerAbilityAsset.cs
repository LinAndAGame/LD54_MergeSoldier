using UnityEngine;

namespace Player {
    public abstract class BasePlayerAbilityAsset : ScriptableObject {
        public string Description;

        public virtual void ApplyAbility() { }
    }
}