using System;
using UnityEngine;

namespace MyGameUtility {
    public class ExampleCharacter : MonoBehaviour {
        public ExampleCharacterAtkData  AtkData;
        public ExampleCharacterDefData  DefData;
        public ExampleGiveDamageUnit    GiveDamageUnitRef;
        public ExampleReceiveDamageUnit ReceiveDamageUnitRef;

        private void Start() {
            if (GiveDamageUnitRef != null) {
                GiveDamageUnitRef.SetAttackData(AtkData);
            }

            if (ReceiveDamageUnitRef != null) {
                ReceiveDamageUnitRef.SetDefData(DefData);
            }
        }
    }
}