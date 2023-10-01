namespace Role {
    public class BaseRoleInfo {
        protected RoleCtrl Owner;

        public BaseRoleInfo(RoleCtrl owner) {
            Owner = owner;
        }
    }
}