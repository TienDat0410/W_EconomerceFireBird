namespace WebSiteBanHang.ViewModels
{
    public class ManageUserRolesViewModel
    {
        public ManageUserRolesViewModel()
        {
            Roles = new List<RoleViewModel>();
        }

        public string UserId { get; set; }
        public string UserName { get; set; }
        public List<RoleViewModel> Roles { get; set; }
    }
}
