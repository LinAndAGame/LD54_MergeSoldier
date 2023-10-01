namespace MyGameUtility.StateMachine {
    public abstract class StateMachineSimpleState<T,K> {
            public readonly T StateName;
            public readonly K DataRef;
            
            public StateMachineSimpleState(T stateName,K dataRef) {
                StateName = stateName;
                DataRef   = dataRef;
            }
            
            public abstract void OnInit();
            public abstract void OnEnter();
            public abstract void OnExit();
    }
}