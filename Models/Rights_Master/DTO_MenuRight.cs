namespace Arihant.Models.Rights_Master
{
    public class DTO_MenuRight
    {
        public int RoleID { get; set; }
        public string RoleName { get; set; }

        public int MenuID { get; set; }
        public string Module { get; set; }
        public string RightName { get; set; } 


        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }

    
        public int RoleAssignedCount { get; set; }
    }
}
