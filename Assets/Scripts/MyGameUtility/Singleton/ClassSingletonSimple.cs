using System;
using MyGameUtility.StateMachine;

namespace MyGameUtility {
    public abstract class ClassSingletonSimple<T> {
        private static T _I;

        public static T I {
            get {
                if (_I == null) {
                    _I = Activator.CreateInstance<T>();
                }

                return _I;
            }
        }
    }
}