namespace Arihant.Models.Rights_Master
{
    public class SaveRoleDTO
    {
        public int RoleID { get; set; }
        public string RoleName { get; set; }
        public List<int> MenuIDs { get; set; }
    }
}
