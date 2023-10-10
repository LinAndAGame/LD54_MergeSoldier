namespace Role {
    public class BaseRoleInfo {
        protected BaseRoleCtrl Owner;

        public BaseRoleInfo(BaseRoleCtrl owner) {
            Owner = owner;
        }
    }
}