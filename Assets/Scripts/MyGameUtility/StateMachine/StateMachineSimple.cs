using System.Collections.Generic;

namespace MyGameUtility.StateMachine {
    public class StateMachineSimple<T,K> {
        private StateMachineSimpleState<T,K>                _CurState;
        private Dictionary<T, StateMachineSimpleState<T,K>> _StateMaps = new Dictionary<T, StateMachineSimpleState<T,K>>();

        public StateMachineSimple(List<StateMachineSimpleState<T,K>> allStates, StateMachineSimpleState<T,K> defaultState) {
            foreach (var state in allStates) {
                _StateMaps.Add(state.StateName, state);
            }

            defaultState.OnInit();
            ChangeTo(defaultState.StateName);
        }
        
        public void ChangeTo(T stateName) {
            StateMachineSimpleState<T,K> changeToState = null;
            if (_StateMaps.ContainsKey(stateName)) {
                changeToState = _StateMaps[stateName];
            }

            if (changeToState != null) {
                if (_CurState != null) {
                    _CurState.OnExit();
                }
                changeToState.OnEnter();
                _CurState = changeToState;
            }
        }
    }
}