namespace Role {
    public class Role_TargetSupporting : BaseRole_Player {
        public RoleCom_TargetAddBuff RoleComTargetAddBuffRef;

        protected override void Editor_SetProperties() {
            base.Editor_SetProperties();
            Editor_InitRoleCom<RoleCom_TargetAddBuff, BaseRoleCtrl>(ref RoleComTargetAddBuffRef);
        }
    }
}