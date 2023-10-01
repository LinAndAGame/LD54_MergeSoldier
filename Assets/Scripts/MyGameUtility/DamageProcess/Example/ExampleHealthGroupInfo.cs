using System;

namespace MyGameUtility {
    public class ExampleHealthGroupInfo : BaseHealthGroupInfo<int> {
        public             HealthSingleInfoInt Hp     = new HealthSingleInfoInt();
        public             HealthSingleInfoInt Shield = new HealthSingleInfoInt();
        
        protected override void Init() {
            AllSingleInfos       = new BaseHealthSingleInfo<int>[1, 2];
            AllSingleInfos[0, 0] = Hp;
            AllSingleInfos[0, 1] = Shield;
            
            Hp.AddChild(Shield);
            
            RefreshIndex();
        }
    }
}