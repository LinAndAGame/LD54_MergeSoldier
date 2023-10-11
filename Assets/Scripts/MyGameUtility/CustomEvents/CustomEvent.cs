using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MyGameExpand;
using MyGameUtility;
using Sirenix.OdinInspector;
[CustomEvent]
public class CustomAction {
    [ShowInInspector]
    public readonly string EventName;
    [ShowInInspector]
    public readonly string ScriptPath;

    private Action _Event;

    [ShowInInspector, ReadOnly]
    private List<CustomEventMark> _AllEventByTimes = new List<CustomEventMark>();

    public CustomAction([CallerMemberName] string eventName = null, [CallerFilePath] string scriptPath = null) {
        EventName = eventName;
        ScriptPath  = scriptPath;
    }

    public bool IsContain(Action targetAction) {
        foreach (CustomEventMark customEventMark in _AllEventByTimes) {
            if (customEventMark != null && customEventMark.IsContain(targetAction)) {
                return true;
            }
        }

        return false;
    }
    
    public void AddListener(Action action, CustomEventCollection customEventMarkUtility = null) {
        AddListener(action, CustomEventAddInfo.Default, customEventMarkUtility);
    }

    public void AddListener(Action action, CustomEventAddInfo addInfo, CustomEventCollection customEventMarkUtility = null) {
        if (action == null || addInfo.DefaultTimes <= 0) {
            return;
        }

        if (addInfo.CanAddWithSame) {
            CustomEventMark exitCustomEventMark = _AllEventByTimes.Find(data => data.Equals(action,addInfo.ReduceTimes,addInfo.AddTimes));

            if (exitCustomEventMark != null) {
                exitCustomEventMark.AddTime();
            }
            else {
                addToBaseEvent(new CustomEventMark(action, addInfo));
            }
        }
        else {
            addToBaseEvent(new CustomEventMark(action, addInfo));
        }

        // 内部方法，添加到BaseEvent中
        void addToBaseEvent(CustomEventMark targetEventByTimes) {
            _Event += (Action)(targetEventByTimes.TargetAction);
            _AllEventByTimes.Add(targetEventByTimes);
            _AllEventByTimes.Sort((a, b) => {
                if (a.AddInfo.PriorityType == b.AddInfo.PriorityType) {
                    return a.AddInfo.Priority - b.AddInfo.Priority;
                }

                return a.AddInfo.PriorityType - b.AddInfo.PriorityType;
            });
            targetEventByTimes.OnEventNoTimes += () => {
                _Event -= (Action)(targetEventByTimes.TargetAction);
                _AllEventByTimes.Remove(targetEventByTimes);
            };
            customEventMarkUtility?.Add(targetEventByTimes);
        }
    }

    public void RemoveListener(Action targetAction) {
        if (targetAction == null) {
            return;
        }

        _AllEventByTimes.Find(data => (Action)data.TargetAction == targetAction)?.KillTimesEvent();
    }

    public void RemoveAllListeners() {
        for (int i = _AllEventByTimes.Count - 1; i >= 0; i--) {
            _AllEventByTimes[i].KillTimesEvent();
        }
    }

    public void Invoke() {
        if (_AllEventByTimes.Count == 0) {
            return;
        }

        List<CustomEventMark> beforeMarks = _AllEventByTimes.Clone();

        _Event.Invoke();

        foreach (var beforeMark in beforeMarks) {
            beforeMark.ReduceTime();
        }
    }
}

[CustomEvent]
public class CustomAction<T0> {
    [ShowInInspector]
    public readonly string EventName;
    [ShowInInspector]
    public readonly string ScriptPath;

    private Action<T0> _Event;

    [ShowInInspector, ReadOnly]
    private List<CustomEventMark> _AllEventByTimes = new List<CustomEventMark>();

    public CustomAction([CallerMemberName] string eventName = null, [CallerFilePath] string scriptPath = null) {
        EventName = eventName;
        ScriptPath  = scriptPath;
    }

    public bool IsContain(Action<T0> targetAction) {
        foreach (CustomEventMark customEventMark in _AllEventByTimes) {
            if (customEventMark != null && customEventMark.IsContain(targetAction)) {
                return true;
            }
        }

        return false;
    }
    
    public void AddListener(Action<T0> action, CustomEventCollection customEventMarkUtility = null) {
        AddListener(action, CustomEventAddInfo.Default, customEventMarkUtility);
    }

    public void AddListener(Action<T0> action, CustomEventAddInfo addInfo, CustomEventCollection customEventMarkUtility = null) {
        if (action == null || addInfo.DefaultTimes <= 0) {
            return;
        }

        if (addInfo.CanAddWithSame) {
            CustomEventMark exitCustomEventMark = _AllEventByTimes.Find(data => data.Equals(action,addInfo.ReduceTimes,addInfo.AddTimes));

            if (exitCustomEventMark != null) {
                exitCustomEventMark.AddTime();
            }
            else {
                addToBaseEvent(new CustomEventMark(action, addInfo));
            }
        }
        else {
            addToBaseEvent(new CustomEventMark(action, addInfo));
        }

        // 内部方法，添加到BaseEvent中
        void addToBaseEvent(CustomEventMark targetEventByTimes) {
            _Event += (Action<T0>)(targetEventByTimes.TargetAction);
            _AllEventByTimes.Add(targetEventByTimes);
            _AllEventByTimes.Sort((a, b) => {
                if (a.AddInfo.PriorityType == b.AddInfo.PriorityType) {
                    return a.AddInfo.Priority - b.AddInfo.Priority;
                }

                return a.AddInfo.PriorityType - b.AddInfo.PriorityType;
            });
            targetEventByTimes.OnEventNoTimes += () => {
                _Event -= (Action<T0>)(targetEventByTimes.TargetAction);
                _AllEventByTimes.Remove(targetEventByTimes);
            };
            customEventMarkUtility?.Add(targetEventByTimes);
        }
    }

    public void RemoveListener(Action<T0> targetAction) {
        if (targetAction == null) {
            return;
        }

        _AllEventByTimes.Find(data => (Action<T0>)data.TargetAction == targetAction)?.KillTimesEvent();
    }

    public void RemoveAllListeners() {
        for (int i = _AllEventByTimes.Count - 1; i >= 0; i--) {
            _AllEventByTimes[i].KillTimesEvent();
        }
    }

    public void Invoke(T0 t0) {
        if (_AllEventByTimes.Count == 0) {
            return;
        }

        List<CustomEventMark> beforeMarks = _AllEventByTimes.Clone();

        _Event.Invoke(t0);

        foreach (var beforeMark in beforeMarks) {
            beforeMark.ReduceTime();
        }
    }
}

[CustomEvent]
public class CustomAction<T0,T1> {
    [ShowInInspector]
    public readonly string EventName;
    [ShowInInspector]
    public readonly string ScriptPath;

    private Action<T0,T1> _Event;

    [ShowInInspector, ReadOnly]
    private List<CustomEventMark> _AllEventByTimes = new List<CustomEventMark>();

    public CustomAction([CallerMemberName] string eventName = null, [CallerFilePath] string scriptPath = null) {
        EventName = eventName;
        ScriptPath  = scriptPath;
    }

    public bool IsContain(Action<T0,T1> targetAction) {
        foreach (CustomEventMark customEventMark in _AllEventByTimes) {
            if (customEventMark != null && customEventMark.IsContain(targetAction)) {
                return true;
            }
        }

        return false;
    }
    
    public void AddListener(Action<T0,T1> action, CustomEventCollection customEventMarkUtility = null) {
        AddListener(action, CustomEventAddInfo.Default, customEventMarkUtility);
    }

    public void AddListener(Action<T0,T1> action, CustomEventAddInfo addInfo, CustomEventCollection customEventMarkUtility = null) {
        if (action == null || addInfo.DefaultTimes <= 0) {
            return;
        }

        if (addInfo.CanAddWithSame) {
            CustomEventMark exitCustomEventMark = _AllEventByTimes.Find(data => data.Equals(action,addInfo.ReduceTimes,addInfo.AddTimes));

            if (exitCustomEventMark != null) {
                exitCustomEventMark.AddTime();
            }
            else {
                addToBaseEvent(new CustomEventMark(action, addInfo));
            }
        }
        else {
            addToBaseEvent(new CustomEventMark(action, addInfo));
        }

        // 内部方法，添加到BaseEvent中
        void addToBaseEvent(CustomEventMark targetEventByTimes) {
            _Event += (Action<T0,T1>)(targetEventByTimes.TargetAction);
            _AllEventByTimes.Add(targetEventByTimes);
            _AllEventByTimes.Sort((a, b) => {
                if (a.AddInfo.PriorityType == b.AddInfo.PriorityType) {
                    return a.AddInfo.Priority - b.AddInfo.Priority;
                }

                return a.AddInfo.PriorityType - b.AddInfo.PriorityType;
            });
            targetEventByTimes.OnEventNoTimes += () => {
                _Event -= (Action<T0,T1>)(targetEventByTimes.TargetAction);
                _AllEventByTimes.Remove(targetEventByTimes);
            };
            customEventMarkUtility?.Add(targetEventByTimes);
        }
    }

    public void RemoveListener(Action<T0,T1> targetAction) {
        if (targetAction == null) {
            return;
        }

        _AllEventByTimes.Find(data => (Action<T0,T1>)data.TargetAction == targetAction)?.KillTimesEvent();
    }

    public void RemoveAllListeners() {
        for (int i = _AllEventByTimes.Count - 1; i >= 0; i--) {
            _AllEventByTimes[i].KillTimesEvent();
        }
    }

    public void Invoke(T0 t0, T1 t1) {
        if (_AllEventByTimes.Count == 0) {
            return;
        }

        List<CustomEventMark> beforeMarks = _AllEventByTimes.Clone();

        _Event.Invoke(t0, t1);

        foreach (var beforeMark in beforeMarks) {
            beforeMark.ReduceTime();
        }
    }
}

[CustomEvent]
public class CustomAction<T0,T1,T2> {
    [ShowInInspector]
    public readonly string EventName;
    [ShowInInspector]
    public readonly string ScriptPath;

    private Action<T0,T1,T2> _Event;

    [ShowInInspector, ReadOnly]
    private List<CustomEventMark> _AllEventByTimes = new List<CustomEventMark>();

    public CustomAction([CallerMemberName] string eventName = null, [CallerFilePath] string scriptPath = null) {
        EventName = eventName;
        ScriptPath  = scriptPath;
    }

    public bool IsContain(Action<T0,T1,T2> targetAction) {
        foreach (CustomEventMark customEventMark in _AllEventByTimes) {
            if (customEventMark != null && customEventMark.IsContain(targetAction)) {
                return true;
            }
        }

        return false;
    }
    
    public void AddListener(Action<T0,T1,T2> action, CustomEventCollection customEventMarkUtility = null) {
        AddListener(action, CustomEventAddInfo.Default, customEventMarkUtility);
    }

    public void AddListener(Action<T0,T1,T2> action, CustomEventAddInfo addInfo, CustomEventCollection customEventMarkUtility = null) {
        if (action == null || addInfo.DefaultTimes <= 0) {
            return;
        }

        if (addInfo.CanAddWithSame) {
            CustomEventMark exitCustomEventMark = _AllEventByTimes.Find(data => data.Equals(action,addInfo.ReduceTimes,addInfo.AddTimes));

            if (exitCustomEventMark != null) {
                exitCustomEventMark.AddTime();
            }
            else {
                addToBaseEvent(new CustomEventMark(action, addInfo));
            }
        }
        else {
            addToBaseEvent(new CustomEventMark(action, addInfo));
        }

        // 内部方法，添加到BaseEvent中
        void addToBaseEvent(CustomEventMark targetEventByTimes) {
            _Event += (Action<T0,T1,T2>)(targetEventByTimes.TargetAction);
            _AllEventByTimes.Add(targetEventByTimes);
            _AllEventByTimes.Sort((a, b) => {
                if (a.AddInfo.PriorityType == b.AddInfo.PriorityType) {
                    return a.AddInfo.Priority - b.AddInfo.Priority;
                }

                return a.AddInfo.PriorityType - b.AddInfo.PriorityType;
            });
            targetEventByTimes.OnEventNoTimes += () => {
                _Event -= (Action<T0,T1,T2>)(targetEventByTimes.TargetAction);
                _AllEventByTimes.Remove(targetEventByTimes);
            };
            customEventMarkUtility?.Add(targetEventByTimes);
        }
    }

    public void RemoveListener(Action<T0,T1,T2> targetAction) {
        if (targetAction == null) {
            return;
        }

        _AllEventByTimes.Find(data => (Action<T0,T1,T2>)data.TargetAction == targetAction)?.KillTimesEvent();
    }

    public void RemoveAllListeners() {
        for (int i = _AllEventByTimes.Count - 1; i >= 0; i--) {
            _AllEventByTimes[i].KillTimesEvent();
        }
    }

    public void Invoke(T0 t0, T1 t1, T2 t2) {
        if (_AllEventByTimes.Count == 0) {
            return;
        }

        List<CustomEventMark> beforeMarks = _AllEventByTimes.Clone();

        _Event.Invoke(t0, t1, t2);

        foreach (var beforeMark in beforeMarks) {
            beforeMark.ReduceTime();
        }
    }
}

[CustomEvent]
public class CustomAction<T0,T1,T2,T3> {
    [ShowInInspector]
    public readonly string EventName;
    [ShowInInspector]
    public readonly string ScriptPath;

    private Action<T0,T1,T2,T3> _Event;

    [ShowInInspector, ReadOnly]
    private List<CustomEventMark> _AllEventByTimes = new List<CustomEventMark>();

    public CustomAction([CallerMemberName] string eventName = null, [CallerFilePath] string scriptPath = null) {
        EventName = eventName;
        ScriptPath  = scriptPath;
    }

    public bool IsContain(Action<T0,T1,T2,T3> targetAction) {
        foreach (CustomEventMark customEventMark in _AllEventByTimes) {
            if (customEventMark != null && customEventMark.IsContain(targetAction)) {
                return true;
            }
        }

        return false;
    }
    
    public void AddListener(Action<T0,T1,T2,T3> action, CustomEventCollection customEventMarkUtility = null) {
        AddListener(action, CustomEventAddInfo.Default, customEventMarkUtility);
    }

    public void AddListener(Action<T0,T1,T2,T3> action, CustomEventAddInfo addInfo, CustomEventCollection customEventMarkUtility = null) {
        if (action == null || addInfo.DefaultTimes <= 0) {
            return;
        }

        if (addInfo.CanAddWithSame) {
            CustomEventMark exitCustomEventMark = _AllEventByTimes.Find(data => data.Equals(action,addInfo.ReduceTimes,addInfo.AddTimes));

            if (exitCustomEventMark != null) {
                exitCustomEventMark.AddTime();
            }
            else {
                addToBaseEvent(new CustomEventMark(action, addInfo));
            }
        }
        else {
            addToBaseEvent(new CustomEventMark(action, addInfo));
        }

        // 内部方法，添加到BaseEvent中
        void addToBaseEvent(CustomEventMark targetEventByTimes) {
            _Event += (Action<T0,T1,T2,T3>)(targetEventByTimes.TargetAction);
            _AllEventByTimes.Add(targetEventByTimes);
            _AllEventByTimes.Sort((a, b) => {
                if (a.AddInfo.PriorityType == b.AddInfo.PriorityType) {
                    return a.AddInfo.Priority - b.AddInfo.Priority;
                }

                return a.AddInfo.PriorityType - b.AddInfo.PriorityType;
            });
            targetEventByTimes.OnEventNoTimes += () => {
                _Event -= (Action<T0,T1,T2,T3>)(targetEventByTimes.TargetAction);
                _AllEventByTimes.Remove(targetEventByTimes);
            };
            customEventMarkUtility?.Add(targetEventByTimes);
        }
    }

    public void RemoveListener(Action<T0,T1,T2,T3> targetAction) {
        if (targetAction == null) {
            return;
        }

        _AllEventByTimes.Find(data => (Action<T0,T1,T2,T3>)data.TargetAction == targetAction)?.KillTimesEvent();
    }

    public void RemoveAllListeners() {
        for (int i = _AllEventByTimes.Count - 1; i >= 0; i--) {
            _AllEventByTimes[i].KillTimesEvent();
        }
    }

    public void Invoke(T0 t0, T1 t1, T2 t2, T3 t3) {
        if (_AllEventByTimes.Count == 0) {
            return;
        }

        List<CustomEventMark> beforeMarks = _AllEventByTimes.Clone();

        _Event.Invoke(t0, t1, t2, t3);

        foreach (var beforeMark in beforeMarks) {
            beforeMark.ReduceTime();
        }
    }
}

[CustomEvent]
public class CustomAction<T0,T1,T2,T3,T4> {
    [ShowInInspector]
    public readonly string EventName;
    [ShowInInspector]
    public readonly string ScriptPath;

    private Action<T0,T1,T2,T3,T4> _Event;

    [ShowInInspector, ReadOnly]
    private List<CustomEventMark> _AllEventByTimes = new List<CustomEventMark>();

    public CustomAction([CallerMemberName] string eventName = null, [CallerFilePath] string scriptPath = null) {
        EventName = eventName;
        ScriptPath  = scriptPath;
    }

    public bool IsContain(Action<T0,T1,T2,T3,T4> targetAction) {
        foreach (CustomEventMark customEventMark in _AllEventByTimes) {
            if (customEventMark != null && customEventMark.IsContain(targetAction)) {
                return true;
            }
        }

        return false;
    }
    
    public void AddListener(Action<T0,T1,T2,T3,T4> action, CustomEventCollection customEventMarkUtility = null) {
        AddListener(action, CustomEventAddInfo.Default, customEventMarkUtility);
    }

    public void AddListener(Action<T0,T1,T2,T3,T4> action, CustomEventAddInfo addInfo, CustomEventCollection customEventMarkUtility = null) {
        if (action == null || addInfo.DefaultTimes <= 0) {
            return;
        }

        if (addInfo.CanAddWithSame) {
            CustomEventMark exitCustomEventMark = _AllEventByTimes.Find(data => data.Equals(action,addInfo.ReduceTimes,addInfo.AddTimes));

            if (exitCustomEventMark != null) {
                exitCustomEventMark.AddTime();
            }
            else {
                addToBaseEvent(new CustomEventMark(action, addInfo));
            }
        }
        else {
            addToBaseEvent(new CustomEventMark(action, addInfo));
        }

        // 内部方法，添加到BaseEvent中
        void addToBaseEvent(CustomEventMark targetEventByTimes) {
            _Event += (Action<T0,T1,T2,T3,T4>)(targetEventByTimes.TargetAction);
            _AllEventByTimes.Add(targetEventByTimes);
            _AllEventByTimes.Sort((a, b) => {
                if (a.AddInfo.PriorityType == b.AddInfo.PriorityType) {
                    return a.AddInfo.Priority - b.AddInfo.Priority;
                }

                return a.AddInfo.PriorityType - b.AddInfo.PriorityType;
            });
            targetEventByTimes.OnEventNoTimes += () => {
                _Event -= (Action<T0,T1,T2,T3,T4>)(targetEventByTimes.TargetAction);
                _AllEventByTimes.Remove(targetEventByTimes);
            };
            customEventMarkUtility?.Add(targetEventByTimes);
        }
    }

    public void RemoveListener(Action<T0,T1,T2,T3,T4> targetAction) {
        if (targetAction == null) {
            return;
        }

        _AllEventByTimes.Find(data => (Action<T0,T1,T2,T3,T4>)data.TargetAction == targetAction)?.KillTimesEvent();
    }

    public void RemoveAllListeners() {
        for (int i = _AllEventByTimes.Count - 1; i >= 0; i--) {
            _AllEventByTimes[i].KillTimesEvent();
        }
    }

    public void Invoke(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4) {
        if (_AllEventByTimes.Count == 0) {
            return;
        }

        List<CustomEventMark> beforeMarks = _AllEventByTimes.Clone();

        _Event.Invoke(t0, t1, t2, t3, t4);

        foreach (var beforeMark in beforeMarks) {
            beforeMark.ReduceTime();
        }
    }
}

[CustomEvent]
public class CustomAction<T0,T1,T2,T3,T4,T5> {
    [ShowInInspector]
    public readonly string EventName;
    [ShowInInspector]
    public readonly string ScriptPath;

    private Action<T0,T1,T2,T3,T4,T5> _Event;

    [ShowInInspector, ReadOnly]
    private List<CustomEventMark> _AllEventByTimes = new List<CustomEventMark>();

    public CustomAction([CallerMemberName] string eventName = null, [CallerFilePath] string scriptPath = null) {
        EventName = eventName;
        ScriptPath  = scriptPath;
    }

    public bool IsContain(Action<T0,T1,T2,T3,T4,T5> targetAction) {
        foreach (CustomEventMark customEventMark in _AllEventByTimes) {
            if (customEventMark != null && customEventMark.IsContain(targetAction)) {
                return true;
            }
        }

        return false;
    }
    
    public void AddListener(Action<T0,T1,T2,T3,T4,T5> action, CustomEventCollection customEventMarkUtility = null) {
        AddListener(action, CustomEventAddInfo.Default, customEventMarkUtility);
    }

    public void AddListener(Action<T0,T1,T2,T3,T4,T5> action, CustomEventAddInfo addInfo, CustomEventCollection customEventMarkUtility = null) {
        if (action == null || addInfo.DefaultTimes <= 0) {
            return;
        }

        if (addInfo.CanAddWithSame) {
            CustomEventMark exitCustomEventMark = _AllEventByTimes.Find(data => data.Equals(action,addInfo.ReduceTimes,addInfo.AddTimes));

            if (exitCustomEventMark != null) {
                exitCustomEventMark.AddTime();
            }
            else {
                addToBaseEvent(new CustomEventMark(action, addInfo));
            }
        }
        else {
            addToBaseEvent(new CustomEventMark(action, addInfo));
        }

        // 内部方法，添加到BaseEvent中
        void addToBaseEvent(CustomEventMark targetEventByTimes) {
            _Event += (Action<T0,T1,T2,T3,T4,T5>)(targetEventByTimes.TargetAction);
            _AllEventByTimes.Add(targetEventByTimes);
            _AllEventByTimes.Sort((a, b) => {
                if (a.AddInfo.PriorityType == b.AddInfo.PriorityType) {
                    return a.AddInfo.Priority - b.AddInfo.Priority;
                }

                return a.AddInfo.PriorityType - b.AddInfo.PriorityType;
            });
            targetEventByTimes.OnEventNoTimes += () => {
                _Event -= (Action<T0,T1,T2,T3,T4,T5>)(targetEventByTimes.TargetAction);
                _AllEventByTimes.Remove(targetEventByTimes);
            };
            customEventMarkUtility?.Add(targetEventByTimes);
        }
    }

    public void RemoveListener(Action<T0,T1,T2,T3,T4,T5> targetAction) {
        if (targetAction == null) {
            return;
        }

        _AllEventByTimes.Find(data => (Action<T0,T1,T2,T3,T4,T5>)data.TargetAction == targetAction)?.KillTimesEvent();
    }

    public void RemoveAllListeners() {
        for (int i = _AllEventByTimes.Count - 1; i >= 0; i--) {
            _AllEventByTimes[i].KillTimesEvent();
        }
    }

    public void Invoke(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5) {
        if (_AllEventByTimes.Count == 0) {
            return;
        }

        List<CustomEventMark> beforeMarks = _AllEventByTimes.Clone();

        _Event.Invoke(t0, t1, t2, t3, t4, t5);

        foreach (var beforeMark in beforeMarks) {
            beforeMark.ReduceTime();
        }
    }
}

[CustomEvent]
public class CustomAction<T0,T1,T2,T3,T4,T5,T6> {
    [ShowInInspector]
    public readonly string EventName;
    [ShowInInspector]
    public readonly string ScriptPath;

    private Action<T0,T1,T2,T3,T4,T5,T6> _Event;

    [ShowInInspector, ReadOnly]
    private List<CustomEventMark> _AllEventByTimes = new List<CustomEventMark>();

    public CustomAction([CallerMemberName] string eventName = null, [CallerFilePath] string scriptPath = null) {
        EventName = eventName;
        ScriptPath  = scriptPath;
    }

    public bool IsContain(Action<T0,T1,T2,T3,T4,T5,T6> targetAction) {
        foreach (CustomEventMark customEventMark in _AllEventByTimes) {
            if (customEventMark != null && customEventMark.IsContain(targetAction)) {
                return true;
            }
        }

        return false;
    }
    
    public void AddListener(Action<T0,T1,T2,T3,T4,T5,T6> action, CustomEventCollection customEventMarkUtility = null) {
        AddListener(action, CustomEventAddInfo.Default, customEventMarkUtility);
    }

    public void AddListener(Action<T0,T1,T2,T3,T4,T5,T6> action, CustomEventAddInfo addInfo, CustomEventCollection customEventMarkUtility = null) {
        if (action == null || addInfo.DefaultTimes <= 0) {
            return;
        }

        if (addInfo.CanAddWithSame) {
            CustomEventMark exitCustomEventMark = _AllEventByTimes.Find(data => data.Equals(action,addInfo.ReduceTimes,addInfo.AddTimes));

            if (exitCustomEventMark != null) {
                exitCustomEventMark.AddTime();
            }
            else {
                addToBaseEvent(new CustomEventMark(action, addInfo));
            }
        }
        else {
            addToBaseEvent(new CustomEventMark(action, addInfo));
        }

        // 内部方法，添加到BaseEvent中
        void addToBaseEvent(CustomEventMark targetEventByTimes) {
            _Event += (Action<T0,T1,T2,T3,T4,T5,T6>)(targetEventByTimes.TargetAction);
            _AllEventByTimes.Add(targetEventByTimes);
            _AllEventByTimes.Sort((a, b) => {
                if (a.AddInfo.PriorityType == b.AddInfo.PriorityType) {
                    return a.AddInfo.Priority - b.AddInfo.Priority;
                }

                return a.AddInfo.PriorityType - b.AddInfo.PriorityType;
            });
            targetEventByTimes.OnEventNoTimes += () => {
                _Event -= (Action<T0,T1,T2,T3,T4,T5,T6>)(targetEventByTimes.TargetAction);
                _AllEventByTimes.Remove(targetEventByTimes);
            };
            customEventMarkUtility?.Add(targetEventByTimes);
        }
    }

    public void RemoveListener(Action<T0,T1,T2,T3,T4,T5,T6> targetAction) {
        if (targetAction == null) {
            return;
        }

        _AllEventByTimes.Find(data => (Action<T0,T1,T2,T3,T4,T5,T6>)data.TargetAction == targetAction)?.KillTimesEvent();
    }

    public void RemoveAllListeners() {
        for (int i = _AllEventByTimes.Count - 1; i >= 0; i--) {
            _AllEventByTimes[i].KillTimesEvent();
        }
    }

    public void Invoke(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6) {
        if (_AllEventByTimes.Count == 0) {
            return;
        }

        List<CustomEventMark> beforeMarks = _AllEventByTimes.Clone();

        _Event.Invoke(t0, t1, t2, t3, t4, t5, t6);

        foreach (var beforeMark in beforeMarks) {
            beforeMark.ReduceTime();
        }
    }
}

[CustomEvent]
public class CustomAction<T0,T1,T2,T3,T4,T5,T6,T7> {
    [ShowInInspector]
    public readonly string EventName;
    [ShowInInspector]
    public readonly string ScriptPath;

    private Action<T0,T1,T2,T3,T4,T5,T6,T7> _Event;

    [ShowInInspector, ReadOnly]
    private List<CustomEventMark> _AllEventByTimes = new List<CustomEventMark>();

    public CustomAction([CallerMemberName] string eventName = null, [CallerFilePath] string scriptPath = null) {
        EventName = eventName;
        ScriptPath  = scriptPath;
    }

    public bool IsContain(Action<T0,T1,T2,T3,T4,T5,T6,T7> targetAction) {
        foreach (CustomEventMark customEventMark in _AllEventByTimes) {
            if (customEventMark != null && customEventMark.IsContain(targetAction)) {
                return true;
            }
        }

        return false;
    }
    
    public void AddListener(Action<T0,T1,T2,T3,T4,T5,T6,T7> action, CustomEventCollection customEventMarkUtility = null) {
        AddListener(action, CustomEventAddInfo.Default, customEventMarkUtility);
    }

    public void AddListener(Action<T0,T1,T2,T3,T4,T5,T6,T7> action, CustomEventAddInfo addInfo, CustomEventCollection customEventMarkUtility = null) {
        if (action == null || addInfo.DefaultTimes <= 0) {
            return;
        }

        if (addInfo.CanAddWithSame) {
            CustomEventMark exitCustomEventMark = _AllEventByTimes.Find(data => data.Equals(action,addInfo.ReduceTimes,addInfo.AddTimes));

            if (exitCustomEventMark != null) {
                exitCustomEventMark.AddTime();
            }
            else {
                addToBaseEvent(new CustomEventMark(action, addInfo));
            }
        }
        else {
            addToBaseEvent(new CustomEventMark(action, addInfo));
        }

        // 内部方法，添加到BaseEvent中
        void addToBaseEvent(CustomEventMark targetEventByTimes) {
            _Event += (Action<T0,T1,T2,T3,T4,T5,T6,T7>)(targetEventByTimes.TargetAction);
            _AllEventByTimes.Add(targetEventByTimes);
            _AllEventByTimes.Sort((a, b) => {
                if (a.AddInfo.PriorityType == b.AddInfo.PriorityType) {
                    return a.AddInfo.Priority - b.AddInfo.Priority;
                }

                return a.AddInfo.PriorityType - b.AddInfo.PriorityType;
            });
            targetEventByTimes.OnEventNoTimes += () => {
                _Event -= (Action<T0,T1,T2,T3,T4,T5,T6,T7>)(targetEventByTimes.TargetAction);
                _AllEventByTimes.Remove(targetEventByTimes);
            };
            customEventMarkUtility?.Add(targetEventByTimes);
        }
    }

    public void RemoveListener(Action<T0,T1,T2,T3,T4,T5,T6,T7> targetAction) {
        if (targetAction == null) {
            return;
        }

        _AllEventByTimes.Find(data => (Action<T0,T1,T2,T3,T4,T5,T6,T7>)data.TargetAction == targetAction)?.KillTimesEvent();
    }

    public void RemoveAllListeners() {
        for (int i = _AllEventByTimes.Count - 1; i >= 0; i--) {
            _AllEventByTimes[i].KillTimesEvent();
        }
    }

    public void Invoke(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7) {
        if (_AllEventByTimes.Count == 0) {
            return;
        }

        List<CustomEventMark> beforeMarks = _AllEventByTimes.Clone();

        _Event.Invoke(t0, t1, t2, t3, t4, t5, t6, t7);

        foreach (var beforeMark in beforeMarks) {
            beforeMark.ReduceTime();
        }
    }
}

[CustomEvent]
public class CustomFunc<TOut> {
    [ShowInInspector]
    public readonly string EventName;
    [ShowInInspector]
    public readonly string ScriptPath;

    private Func<TOut> _Event;

    [ShowInInspector, ReadOnly]
    private List<CustomEventMark> _AllEventByTimes = new List<CustomEventMark>();

    public CustomFunc([CallerMemberName] string eventName = null, [CallerFilePath] string scriptPath = null) {
        EventName = eventName;
        ScriptPath  = scriptPath;
    }

    public bool IsContain(Func<TOut> targetAction) {
        foreach (CustomEventMark customEventMark in _AllEventByTimes) {
            if (customEventMark != null && customEventMark.IsContain(targetAction)) {
                return true;
            }
        }

        return false;
    }
    
    public void AddListener(Func<TOut> action, CustomEventCollection customEventMarkUtility = null) {
        AddListener(action, CustomEventAddInfo.Default, customEventMarkUtility);
    }
    
    public void AddListener(Func<TOut> action, CustomEventAddInfo addInfo, CustomEventCollection customEventMarkUtility = null) {
        if (action == null || addInfo.DefaultTimes <= 0) {
            return;
        }

        if (addInfo.CanAddWithSame) {
            CustomEventMark exitCustomEventMark = _AllEventByTimes.Find(data => data.Equals(action,addInfo.ReduceTimes,addInfo.AddTimes));

            if (exitCustomEventMark != null) {
                exitCustomEventMark.AddTime();
            }
            else {
                addToBaseEvent(new CustomEventMark(action, addInfo));
            }
        }
        else {
            addToBaseEvent(new CustomEventMark(action, addInfo));
        }

        // 内部方法，添加到BaseEvent中
        void addToBaseEvent(CustomEventMark targetEventByTimes) {
            _Event += (Func<TOut>)(targetEventByTimes.TargetAction);
            _AllEventByTimes.Add(targetEventByTimes);
            _AllEventByTimes.Sort((a, b) => {
                if (a.AddInfo.PriorityType == b.AddInfo.PriorityType) {
                    return a.AddInfo.Priority - b.AddInfo.Priority;
                }

                return a.AddInfo.PriorityType - b.AddInfo.PriorityType;
            });
            targetEventByTimes.OnEventNoTimes += () => {
                _Event -= (Func<TOut>)(targetEventByTimes.TargetAction);
                _AllEventByTimes.Remove(targetEventByTimes);
            };
            customEventMarkUtility?.Add(targetEventByTimes);
        }
    }

    public void RemoveListener(Func<TOut> targetAction) {
        if (targetAction == null) {
            return;
        }

        _AllEventByTimes.Find(data => (Func<TOut>)data.TargetAction == targetAction)?.KillTimesEvent();
    }

    public void RemoveAllListeners() {
        for (int i = _AllEventByTimes.Count - 1; i >= 0; i--) {
            _AllEventByTimes[i].KillTimesEvent();
        }
    }

    public TOut Invoke() {
        if (_AllEventByTimes.Count == 0) {
            throw new Exception("事件没有任何注册的监听!");
        }

        List<CustomEventMark> beforeMarks = _AllEventByTimes.Clone();

        TOut result = _Event.Invoke();

        foreach (var beforeMark in beforeMarks) {
            beforeMark.ReduceTime();
        }

        return result;
    }
}
[CustomEvent]
public class CustomFunc<T0, TOut> {
    [ShowInInspector]
    public readonly string EventName;
    [ShowInInspector]
    public readonly string ScriptPath;

    private Func<T0, TOut> _Event;

    [ShowInInspector, ReadOnly]
    private List<CustomEventMark> _AllEventByTimes = new List<CustomEventMark>();

    public CustomFunc([CallerMemberName] string eventName = null, [CallerFilePath] string scriptPath = null) {
        EventName = eventName;
        ScriptPath  = scriptPath;
    }

    public bool IsContain(Func<T0, TOut> targetAction) {
        foreach (CustomEventMark customEventMark in _AllEventByTimes) {
            if (customEventMark != null && customEventMark.IsContain(targetAction)) {
                return true;
            }
        }

        return false;
    }
    
    public void AddListener(Func<T0, TOut> action, CustomEventCollection customEventMarkUtility = null) {
        AddListener(action, CustomEventAddInfo.Default, customEventMarkUtility);
    }
    
    public void AddListener(Func<T0, TOut> action, CustomEventAddInfo addInfo, CustomEventCollection customEventMarkUtility = null) {
        if (action == null || addInfo.DefaultTimes <= 0) {
            return;
        }

        if (addInfo.CanAddWithSame) {
            CustomEventMark exitCustomEventMark = _AllEventByTimes.Find(data => data.Equals(action,addInfo.ReduceTimes,addInfo.AddTimes));

            if (exitCustomEventMark != null) {
                exitCustomEventMark.AddTime();
            }
            else {
                addToBaseEvent(new CustomEventMark(action, addInfo));
            }
        }
        else {
            addToBaseEvent(new CustomEventMark(action, addInfo));
        }

        // 内部方法，添加到BaseEvent中
        void addToBaseEvent(CustomEventMark targetEventByTimes) {
            _Event += (Func<T0, TOut>)(targetEventByTimes.TargetAction);
            _AllEventByTimes.Add(targetEventByTimes);
            _AllEventByTimes.Sort((a, b) => {
                if (a.AddInfo.PriorityType == b.AddInfo.PriorityType) {
                    return a.AddInfo.Priority - b.AddInfo.Priority;
                }

                return a.AddInfo.PriorityType - b.AddInfo.PriorityType;
            });
            targetEventByTimes.OnEventNoTimes += () => {
                _Event -= (Func<T0, TOut>)(targetEventByTimes.TargetAction);
                _AllEventByTimes.Remove(targetEventByTimes);
            };
            customEventMarkUtility?.Add(targetEventByTimes);
        }
    }

    public void RemoveListener(Func<T0, TOut> targetAction) {
        if (targetAction == null) {
            return;
        }

        _AllEventByTimes.Find(data => (Func<T0, TOut>)data.TargetAction == targetAction)?.KillTimesEvent();
    }

    public void RemoveAllListeners() {
        for (int i = _AllEventByTimes.Count - 1; i >= 0; i--) {
            _AllEventByTimes[i].KillTimesEvent();
        }
    }

    public TOut Invoke(T0 t0) {
        if (_AllEventByTimes.Count == 0) {
            throw new Exception("事件没有任何注册的监听!");
        }

        List<CustomEventMark> beforeMarks = _AllEventByTimes.Clone();

        TOut result = _Event.Invoke(t0);

        foreach (var beforeMark in beforeMarks) {
            beforeMark.ReduceTime();
        }

        return result;
    }
}
[CustomEvent]
public class CustomFunc<T0, T1, TOut> {
    [ShowInInspector]
    public readonly string EventName;
    [ShowInInspector]
    public readonly string ScriptPath;

    private Func<T0, T1, TOut> _Event;

    [ShowInInspector, ReadOnly]
    private List<CustomEventMark> _AllEventByTimes = new List<CustomEventMark>();

    public CustomFunc([CallerMemberName] string eventName = null, [CallerFilePath] string scriptPath = null) {
        EventName = eventName;
        ScriptPath  = scriptPath;
    }

    public bool IsContain(Func<T0, T1, TOut> targetAction) {
        foreach (CustomEventMark customEventMark in _AllEventByTimes) {
            if (customEventMark != null && customEventMark.IsContain(targetAction)) {
                return true;
            }
        }

        return false;
    }
    
    public void AddListener(Func<T0, T1, TOut> action, CustomEventCollection customEventMarkUtility = null) {
        AddListener(action, CustomEventAddInfo.Default, customEventMarkUtility);
    }
    
    public void AddListener(Func<T0, T1, TOut> action, CustomEventAddInfo addInfo, CustomEventCollection customEventMarkUtility = null) {
        if (action == null || addInfo.DefaultTimes <= 0) {
            return;
        }

        if (addInfo.CanAddWithSame) {
            CustomEventMark exitCustomEventMark = _AllEventByTimes.Find(data => data.Equals(action,addInfo.ReduceTimes,addInfo.AddTimes));

            if (exitCustomEventMark != null) {
                exitCustomEventMark.AddTime();
            }
            else {
                addToBaseEvent(new CustomEventMark(action, addInfo));
            }
        }
        else {
            addToBaseEvent(new CustomEventMark(action, addInfo));
        }

        // 内部方法，添加到BaseEvent中
        void addToBaseEvent(CustomEventMark targetEventByTimes) {
            _Event += (Func<T0, T1, TOut>)(targetEventByTimes.TargetAction);
            _AllEventByTimes.Add(targetEventByTimes);
            _AllEventByTimes.Sort((a, b) => {
                if (a.AddInfo.PriorityType == b.AddInfo.PriorityType) {
                    return a.AddInfo.Priority - b.AddInfo.Priority;
                }

                return a.AddInfo.PriorityType - b.AddInfo.PriorityType;
            });
            targetEventByTimes.OnEventNoTimes += () => {
                _Event -= (Func<T0, T1, TOut>)(targetEventByTimes.TargetAction);
                _AllEventByTimes.Remove(targetEventByTimes);
            };
            customEventMarkUtility?.Add(targetEventByTimes);
        }
    }

    public void RemoveListener(Func<T0, T1, TOut> targetAction) {
        if (targetAction == null) {
            return;
        }

        _AllEventByTimes.Find(data => (Func<T0, T1, TOut>)data.TargetAction == targetAction)?.KillTimesEvent();
    }

    public void RemoveAllListeners() {
        for (int i = _AllEventByTimes.Count - 1; i >= 0; i--) {
            _AllEventByTimes[i].KillTimesEvent();
        }
    }

    public TOut Invoke(T0 t0, T1 t1) {
        if (_AllEventByTimes.Count == 0) {
            throw new Exception("事件没有任何注册的监听!");
        }

        List<CustomEventMark> beforeMarks = _AllEventByTimes.Clone();

        TOut result = _Event.Invoke(t0, t1);

        foreach (var beforeMark in beforeMarks) {
            beforeMark.ReduceTime();
        }

        return result;
    }
}
[CustomEvent]
public class CustomFunc<T0, T1, T2, TOut> {
    [ShowInInspector]
    public readonly string EventName;
    [ShowInInspector]
    public readonly string ScriptPath;

    private Func<T0, T1, T2, TOut> _Event;

    [ShowInInspector, ReadOnly]
    private List<CustomEventMark> _AllEventByTimes = new List<CustomEventMark>();

    public CustomFunc([CallerMemberName] string eventName = null, [CallerFilePath] string scriptPath = null) {
        EventName = eventName;
        ScriptPath  = scriptPath;
    }

    public bool IsContain(Func<T0, T1, T2, TOut> targetAction) {
        foreach (CustomEventMark customEventMark in _AllEventByTimes) {
            if (customEventMark != null && customEventMark.IsContain(targetAction)) {
                return true;
            }
        }

        return false;
    }
    
    public void AddListener(Func<T0, T1, T2, TOut> action, CustomEventCollection customEventMarkUtility = null) {
        AddListener(action, CustomEventAddInfo.Default, customEventMarkUtility);
    }
    
    public void AddListener(Func<T0, T1, T2, TOut> action, CustomEventAddInfo addInfo, CustomEventCollection customEventMarkUtility = null) {
        if (action == null || addInfo.DefaultTimes <= 0) {
            return;
        }

        if (addInfo.CanAddWithSame) {
            CustomEventMark exitCustomEventMark = _AllEventByTimes.Find(data => data.Equals(action,addInfo.ReduceTimes,addInfo.AddTimes));

            if (exitCustomEventMark != null) {
                exitCustomEventMark.AddTime();
            }
            else {
                addToBaseEvent(new CustomEventMark(action, addInfo));
            }
        }
        else {
            addToBaseEvent(new CustomEventMark(action, addInfo));
        }

        // 内部方法，添加到BaseEvent中
        void addToBaseEvent(CustomEventMark targetEventByTimes) {
            _Event += (Func<T0, T1, T2, TOut>)(targetEventByTimes.TargetAction);
            _AllEventByTimes.Add(targetEventByTimes);
            _AllEventByTimes.Sort((a, b) => {
                if (a.AddInfo.PriorityType == b.AddInfo.PriorityType) {
                    return a.AddInfo.Priority - b.AddInfo.Priority;
                }

                return a.AddInfo.PriorityType - b.AddInfo.PriorityType;
            });
            targetEventByTimes.OnEventNoTimes += () => {
                _Event -= (Func<T0, T1, T2, TOut>)(targetEventByTimes.TargetAction);
                _AllEventByTimes.Remove(targetEventByTimes);
            };
            customEventMarkUtility?.Add(targetEventByTimes);
        }
    }

    public void RemoveListener(Func<T0, T1, T2, TOut> targetAction) {
        if (targetAction == null) {
            return;
        }

        _AllEventByTimes.Find(data => (Func<T0, T1, T2, TOut>)data.TargetAction == targetAction)?.KillTimesEvent();
    }

    public void RemoveAllListeners() {
        for (int i = _AllEventByTimes.Count - 1; i >= 0; i--) {
            _AllEventByTimes[i].KillTimesEvent();
        }
    }

    public TOut Invoke(T0 t0, T1 t1, T2 t2) {
        if (_AllEventByTimes.Count == 0) {
            throw new Exception("事件没有任何注册的监听!");
        }

        List<CustomEventMark> beforeMarks = _AllEventByTimes.Clone();

        TOut result = _Event.Invoke(t0, t1, t2);

        foreach (var beforeMark in beforeMarks) {
            beforeMark.ReduceTime();
        }

        return result;
    }
}
[CustomEvent]
public class CustomFunc<T0, T1, T2, T3, TOut> {
    [ShowInInspector]
    public readonly string EventName;
    [ShowInInspector]
    public readonly string ScriptPath;

    private Func<T0, T1, T2, T3, TOut> _Event;

    [ShowInInspector, ReadOnly]
    private List<CustomEventMark> _AllEventByTimes = new List<CustomEventMark>();

    public CustomFunc([CallerMemberName] string eventName = null, [CallerFilePath] string scriptPath = null) {
        EventName = eventName;
        ScriptPath  = scriptPath;
    }

    public bool IsContain(Func<T0, T1, T2, T3, TOut> targetAction) {
        foreach (CustomEventMark customEventMark in _AllEventByTimes) {
            if (customEventMark != null && customEventMark.IsContain(targetAction)) {
                return true;
            }
        }

        return false;
    }
    
    public void AddListener(Func<T0, T1, T2, T3, TOut> action, CustomEventCollection customEventMarkUtility = null) {
        AddListener(action, CustomEventAddInfo.Default, customEventMarkUtility);
    }
    
    public void AddListener(Func<T0, T1, T2, T3, TOut> action, CustomEventAddInfo addInfo, CustomEventCollection customEventMarkUtility = null) {
        if (action == null || addInfo.DefaultTimes <= 0) {
            return;
        }

        if (addInfo.CanAddWithSame) {
            CustomEventMark exitCustomEventMark = _AllEventByTimes.Find(data => data.Equals(action,addInfo.ReduceTimes,addInfo.AddTimes));

            if (exitCustomEventMark != null) {
                exitCustomEventMark.AddTime();
            }
            else {
                addToBaseEvent(new CustomEventMark(action, addInfo));
            }
        }
        else {
            addToBaseEvent(new CustomEventMark(action, addInfo));
        }

        // 内部方法，添加到BaseEvent中
        void addToBaseEvent(CustomEventMark targetEventByTimes) {
            _Event += (Func<T0, T1, T2, T3, TOut>)(targetEventByTimes.TargetAction);
            _AllEventByTimes.Add(targetEventByTimes);
            _AllEventByTimes.Sort((a, b) => {
                if (a.AddInfo.PriorityType == b.AddInfo.PriorityType) {
                    return a.AddInfo.Priority - b.AddInfo.Priority;
                }

                return a.AddInfo.PriorityType - b.AddInfo.PriorityType;
            });
            targetEventByTimes.OnEventNoTimes += () => {
                _Event -= (Func<T0, T1, T2, T3, TOut>)(targetEventByTimes.TargetAction);
                _AllEventByTimes.Remove(targetEventByTimes);
            };
            customEventMarkUtility?.Add(targetEventByTimes);
        }
    }

    public void RemoveListener(Func<T0, T1, T2, T3, TOut> targetAction) {
        if (targetAction == null) {
            return;
        }

        _AllEventByTimes.Find(data => (Func<T0, T1, T2, T3, TOut>)data.TargetAction == targetAction)?.KillTimesEvent();
    }

    public void RemoveAllListeners() {
        for (int i = _AllEventByTimes.Count - 1; i >= 0; i--) {
            _AllEventByTimes[i].KillTimesEvent();
        }
    }

    public TOut Invoke(T0 t0, T1 t1, T2 t2, T3 t3) {
        if (_AllEventByTimes.Count == 0) {
            throw new Exception("事件没有任何注册的监听!");
        }

        List<CustomEventMark> beforeMarks = _AllEventByTimes.Clone();

        TOut result = _Event.Invoke(t0, t1, t2, t3);

        foreach (var beforeMark in beforeMarks) {
            beforeMark.ReduceTime();
        }

        return result;
    }
}
[CustomEvent]
public class CustomFunc<T0, T1, T2, T3, T4, TOut> {
    [ShowInInspector]
    public readonly string EventName;
    [ShowInInspector]
    public readonly string ScriptPath;

    private Func<T0, T1, T2, T3, T4, TOut> _Event;

    [ShowInInspector, ReadOnly]
    private List<CustomEventMark> _AllEventByTimes = new List<CustomEventMark>();

    public CustomFunc([CallerMemberName] string eventName = null, [CallerFilePath] string scriptPath = null) {
        EventName = eventName;
        ScriptPath  = scriptPath;
    }

    public bool IsContain(Func<T0, T1, T2, T3, T4, TOut> targetAction) {
        foreach (CustomEventMark customEventMark in _AllEventByTimes) {
            if (customEventMark != null && customEventMark.IsContain(targetAction)) {
                return true;
            }
        }

        return false;
    }
    
    public void AddListener(Func<T0, T1, T2, T3, T4, TOut> action, CustomEventCollection customEventMarkUtility = null) {
        AddListener(action, CustomEventAddInfo.Default, customEventMarkUtility);
    }
    
    public void AddListener(Func<T0, T1, T2, T3, T4, TOut> action, CustomEventAddInfo addInfo, CustomEventCollection customEventMarkUtility = null) {
        if (action == null || addInfo.DefaultTimes <= 0) {
            return;
        }

        if (addInfo.CanAddWithSame) {
            CustomEventMark exitCustomEventMark = _AllEventByTimes.Find(data => data.Equals(action,addInfo.ReduceTimes,addInfo.AddTimes));

            if (exitCustomEventMark != null) {
                exitCustomEventMark.AddTime();
            }
            else {
                addToBaseEvent(new CustomEventMark(action, addInfo));
            }
        }
        else {
            addToBaseEvent(new CustomEventMark(action, addInfo));
        }

        // 内部方法，添加到BaseEvent中
        void addToBaseEvent(CustomEventMark targetEventByTimes) {
            _Event += (Func<T0, T1, T2, T3, T4, TOut>)(targetEventByTimes.TargetAction);
            _AllEventByTimes.Add(targetEventByTimes);
            _AllEventByTimes.Sort((a, b) => {
                if (a.AddInfo.PriorityType == b.AddInfo.PriorityType) {
                    return a.AddInfo.Priority - b.AddInfo.Priority;
                }

                return a.AddInfo.PriorityType - b.AddInfo.PriorityType;
            });
            targetEventByTimes.OnEventNoTimes += () => {
                _Event -= (Func<T0, T1, T2, T3, T4, TOut>)(targetEventByTimes.TargetAction);
                _AllEventByTimes.Remove(targetEventByTimes);
            };
            customEventMarkUtility?.Add(targetEventByTimes);
        }
    }

    public void RemoveListener(Func<T0, T1, T2, T3, T4, TOut> targetAction) {
        if (targetAction == null) {
            return;
        }

        _AllEventByTimes.Find(data => (Func<T0, T1, T2, T3, T4, TOut>)data.TargetAction == targetAction)?.KillTimesEvent();
    }

    public void RemoveAllListeners() {
        for (int i = _AllEventByTimes.Count - 1; i >= 0; i--) {
            _AllEventByTimes[i].KillTimesEvent();
        }
    }

    public TOut Invoke(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4) {
        if (_AllEventByTimes.Count == 0) {
            throw new Exception("事件没有任何注册的监听!");
        }

        List<CustomEventMark> beforeMarks = _AllEventByTimes.Clone();

        TOut result = _Event.Invoke(t0, t1, t2, t3, t4);

        foreach (var beforeMark in beforeMarks) {
            beforeMark.ReduceTime();
        }

        return result;
    }
}
[CustomEvent]
public class CustomFunc<T0, T1, T2, T3, T4, T5, TOut> {
    [ShowInInspector]
    public readonly string EventName;
    [ShowInInspector]
    public readonly string ScriptPath;

    private Func<T0, T1, T2, T3, T4, T5, TOut> _Event;

    [ShowInInspector, ReadOnly]
    private List<CustomEventMark> _AllEventByTimes = new List<CustomEventMark>();

    public CustomFunc([CallerMemberName] string eventName = null, [CallerFilePath] string scriptPath = null) {
        EventName = eventName;
        ScriptPath  = scriptPath;
    }

    public bool IsContain(Func<T0, T1, T2, T3, T4, T5, TOut> targetAction) {
        foreach (CustomEventMark customEventMark in _AllEventByTimes) {
            if (customEventMark != null && customEventMark.IsContain(targetAction)) {
                return true;
            }
        }

        return false;
    }
    
    public void AddListener(Func<T0, T1, T2, T3, T4, T5, TOut> action, CustomEventCollection customEventMarkUtility = null) {
        AddListener(action, CustomEventAddInfo.Default, customEventMarkUtility);
    }
    
    public void AddListener(Func<T0, T1, T2, T3, T4, T5, TOut> action, CustomEventAddInfo addInfo, CustomEventCollection customEventMarkUtility = null) {
        if (action == null || addInfo.DefaultTimes <= 0) {
            return;
        }

        if (addInfo.CanAddWithSame) {
            CustomEventMark exitCustomEventMark = _AllEventByTimes.Find(data => data.Equals(action,addInfo.ReduceTimes,addInfo.AddTimes));

            if (exitCustomEventMark != null) {
                exitCustomEventMark.AddTime();
            }
            else {
                addToBaseEvent(new CustomEventMark(action, addInfo));
            }
        }
        else {
            addToBaseEvent(new CustomEventMark(action, addInfo));
        }

        // 内部方法，添加到BaseEvent中
        void addToBaseEvent(CustomEventMark targetEventByTimes) {
            _Event += (Func<T0, T1, T2, T3, T4, T5, TOut>)(targetEventByTimes.TargetAction);
            _AllEventByTimes.Add(targetEventByTimes);
            _AllEventByTimes.Sort((a, b) => {
                if (a.AddInfo.PriorityType == b.AddInfo.PriorityType) {
                    return a.AddInfo.Priority - b.AddInfo.Priority;
                }

                return a.AddInfo.PriorityType - b.AddInfo.PriorityType;
            });
            targetEventByTimes.OnEventNoTimes += () => {
                _Event -= (Func<T0, T1, T2, T3, T4, T5, TOut>)(targetEventByTimes.TargetAction);
                _AllEventByTimes.Remove(targetEventByTimes);
            };
            customEventMarkUtility?.Add(targetEventByTimes);
        }
    }

    public void RemoveListener(Func<T0, T1, T2, T3, T4, T5, TOut> targetAction) {
        if (targetAction == null) {
            return;
        }

        _AllEventByTimes.Find(data => (Func<T0, T1, T2, T3, T4, T5, TOut>)data.TargetAction == targetAction)?.KillTimesEvent();
    }

    public void RemoveAllListeners() {
        for (int i = _AllEventByTimes.Count - 1; i >= 0; i--) {
            _AllEventByTimes[i].KillTimesEvent();
        }
    }

    public TOut Invoke(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5) {
        if (_AllEventByTimes.Count == 0) {
            throw new Exception("事件没有任何注册的监听!");
        }

        List<CustomEventMark> beforeMarks = _AllEventByTimes.Clone();

        TOut result = _Event.Invoke(t0, t1, t2, t3, t4, t5);

        foreach (var beforeMark in beforeMarks) {
            beforeMark.ReduceTime();
        }

        return result;
    }
}
[CustomEvent]
public class CustomFunc<T0, T1, T2, T3, T4, T5, T6, TOut> {
    [ShowInInspector]
    public readonly string EventName;
    [ShowInInspector]
    public readonly string ScriptPath;

    private Func<T0, T1, T2, T3, T4, T5, T6, TOut> _Event;

    [ShowInInspector, ReadOnly]
    private List<CustomEventMark> _AllEventByTimes = new List<CustomEventMark>();

    public CustomFunc([CallerMemberName] string eventName = null, [CallerFilePath] string scriptPath = null) {
        EventName = eventName;
        ScriptPath  = scriptPath;
    }

    public bool IsContain(Func<T0, T1, T2, T3, T4, T5, T6, TOut> targetAction) {
        foreach (CustomEventMark customEventMark in _AllEventByTimes) {
            if (customEventMark != null && customEventMark.IsContain(targetAction)) {
                return true;
            }
        }

        return false;
    }
    
    public void AddListener(Func<T0, T1, T2, T3, T4, T5, T6, TOut> action, CustomEventCollection customEventMarkUtility = null) {
        AddListener(action, CustomEventAddInfo.Default, customEventMarkUtility);
    }
    
    public void AddListener(Func<T0, T1, T2, T3, T4, T5, T6, TOut> action, CustomEventAddInfo addInfo, CustomEventCollection customEventMarkUtility = null) {
        if (action == null || addInfo.DefaultTimes <= 0) {
            return;
        }

        if (addInfo.CanAddWithSame) {
            CustomEventMark exitCustomEventMark = _AllEventByTimes.Find(data => data.Equals(action,addInfo.ReduceTimes,addInfo.AddTimes));

            if (exitCustomEventMark != null) {
                exitCustomEventMark.AddTime();
            }
            else {
                addToBaseEvent(new CustomEventMark(action, addInfo));
            }
        }
        else {
            addToBaseEvent(new CustomEventMark(action, addInfo));
        }

        // 内部方法，添加到BaseEvent中
        void addToBaseEvent(CustomEventMark targetEventByTimes) {
            _Event += (Func<T0, T1, T2, T3, T4, T5, T6, TOut>)(targetEventByTimes.TargetAction);
            _AllEventByTimes.Add(targetEventByTimes);
            _AllEventByTimes.Sort((a, b) => {
                if (a.AddInfo.PriorityType == b.AddInfo.PriorityType) {
                    return a.AddInfo.Priority - b.AddInfo.Priority;
                }

                return a.AddInfo.PriorityType - b.AddInfo.PriorityType;
            });
            targetEventByTimes.OnEventNoTimes += () => {
                _Event -= (Func<T0, T1, T2, T3, T4, T5, T6, TOut>)(targetEventByTimes.TargetAction);
                _AllEventByTimes.Remove(targetEventByTimes);
            };
            customEventMarkUtility?.Add(targetEventByTimes);
        }
    }

    public void RemoveListener(Func<T0, T1, T2, T3, T4, T5, T6, TOut> targetAction) {
        if (targetAction == null) {
            return;
        }

        _AllEventByTimes.Find(data => (Func<T0, T1, T2, T3, T4, T5, T6, TOut>)data.TargetAction == targetAction)?.KillTimesEvent();
    }

    public void RemoveAllListeners() {
        for (int i = _AllEventByTimes.Count - 1; i >= 0; i--) {
            _AllEventByTimes[i].KillTimesEvent();
        }
    }

    public TOut Invoke(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6) {
        if (_AllEventByTimes.Count == 0) {
            throw new Exception("事件没有任何注册的监听!");
        }

        List<CustomEventMark> beforeMarks = _AllEventByTimes.Clone();

        TOut result = _Event.Invoke(t0, t1, t2, t3, t4, t5, t6);

        foreach (var beforeMark in beforeMarks) {
            beforeMark.ReduceTime();
        }

        return result;
    }
}
[CustomEvent]
public class CustomFunc<T0, T1, T2, T3, T4, T5, T6, T7, TOut> {
    [ShowInInspector]
    public readonly string EventName;
    [ShowInInspector]
    public readonly string ScriptPath;

    private Func<T0, T1, T2, T3, T4, T5, T6, T7, TOut> _Event;

    [ShowInInspector, ReadOnly]
    private List<CustomEventMark> _AllEventByTimes = new List<CustomEventMark>();

    public CustomFunc([CallerMemberName] string eventName = null, [CallerFilePath] string scriptPath = null) {
        EventName = eventName;
        ScriptPath  = scriptPath;
    }

    public bool IsContain(Func<T0, T1, T2, T3, T4, T5, T6, T7, TOut> targetAction) {
        foreach (CustomEventMark customEventMark in _AllEventByTimes) {
            if (customEventMark != null && customEventMark.IsContain(targetAction)) {
                return true;
            }
        }

        return false;
    }
    
    public void AddListener(Func<T0, T1, T2, T3, T4, T5, T6, T7, TOut> action, CustomEventCollection customEventMarkUtility = null) {
        AddListener(action, CustomEventAddInfo.Default, customEventMarkUtility);
    }
    
    public void AddListener(Func<T0, T1, T2, T3, T4, T5, T6, T7, TOut> action, CustomEventAddInfo addInfo, CustomEventCollection customEventMarkUtility = null) {
        if (action == null || addInfo.DefaultTimes <= 0) {
            return;
        }

        if (addInfo.CanAddWithSame) {
            CustomEventMark exitCustomEventMark = _AllEventByTimes.Find(data => data.Equals(action,addInfo.ReduceTimes,addInfo.AddTimes));

            if (exitCustomEventMark != null) {
                exitCustomEventMark.AddTime();
            }
            else {
                addToBaseEvent(new CustomEventMark(action, addInfo));
            }
        }
        else {
            addToBaseEvent(new CustomEventMark(action, addInfo));
        }

        // 内部方法，添加到BaseEvent中
        void addToBaseEvent(CustomEventMark targetEventByTimes) {
            _Event += (Func<T0, T1, T2, T3, T4, T5, T6, T7, TOut>)(targetEventByTimes.TargetAction);
            _AllEventByTimes.Add(targetEventByTimes);
            _AllEventByTimes.Sort((a, b) => {
                if (a.AddInfo.PriorityType == b.AddInfo.PriorityType) {
                    return a.AddInfo.Priority - b.AddInfo.Priority;
                }

                return a.AddInfo.PriorityType - b.AddInfo.PriorityType;
            });
            targetEventByTimes.OnEventNoTimes += () => {
                _Event -= (Func<T0, T1, T2, T3, T4, T5, T6, T7, TOut>)(targetEventByTimes.TargetAction);
                _AllEventByTimes.Remove(targetEventByTimes);
            };
            customEventMarkUtility?.Add(targetEventByTimes);
        }
    }

    public void RemoveListener(Func<T0, T1, T2, T3, T4, T5, T6, T7, TOut> targetAction) {
        if (targetAction == null) {
            return;
        }

        _AllEventByTimes.Find(data => (Func<T0, T1, T2, T3, T4, T5, T6, T7, TOut>)data.TargetAction == targetAction)?.KillTimesEvent();
    }

    public void RemoveAllListeners() {
        for (int i = _AllEventByTimes.Count - 1; i >= 0; i--) {
            _AllEventByTimes[i].KillTimesEvent();
        }
    }

    public TOut Invoke(T0 t0, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7) {
        if (_AllEventByTimes.Count == 0) {
            throw new Exception("事件没有任何注册的监听!");
        }

        List<CustomEventMark> beforeMarks = _AllEventByTimes.Clone();

        TOut result = _Event.Invoke(t0, t1, t2, t3, t4, t5, t6, t7);

        foreach (var beforeMark in beforeMarks) {
            beforeMark.ReduceTime();
        }

        return result;
    }
}
