namespace Role {
    public class Role_AreaSupporting : BaseRole_Player {
        public RoleCom_AreaPlayerAddBuff RoleComAreaPlayerAddBuffRef;

        protected override void Editor_SetProperties() {
            base.Editor_SetProperties();
            Editor_InitRoleCom<RoleCom_AreaPlayerAddBuff, BaseRole_Player>(ref RoleComAreaPlayerAddBuffRef);
        }
    }
}