namespace MyGameUtility {
    public struct CustomEventAddInfo {
        public int          DefaultTimes;
        public int          ReduceTimes;
        public int          AddTimes;
        public bool         CanAddWithSame;
        public PriorityEnum PriorityType;
        public int          Priority;

        public CustomEventAddInfo(int defaultTimes, int reduceTimes, int addTimes, bool canAddWithSame, PriorityEnum priorityType, int priority) {
            DefaultTimes   = defaultTimes;
            ReduceTimes    = reduceTimes;
            AddTimes       = addTimes;
            CanAddWithSame = canAddWithSame;
            PriorityType   = priorityType;
            Priority       = priority;
        }

        public static CustomEventAddInfo Default       = new CustomEventAddInfo(1, 0, 0, false, PriorityEnum.Normal,0);
        public static CustomEventAddInfo SingleTrigger = new CustomEventAddInfo(1, 0, 0, true, PriorityEnum.Normal, 0);
        public static CustomEventAddInfo DefaultUI       = new CustomEventAddInfo(1, 0, 0, false, PriorityEnum.UI,0);
        public static CustomEventAddInfo SingleTriggerUI = new CustomEventAddInfo(1, 0, 0, true, PriorityEnum.UI, 0);
        
        public enum PriorityEnum {
            Normal,
            UI,
        }
    }
}