namespace Arihant.Models.User_Master
{
    public class UserUpdateModel
    {
        public int? ID { get; set; }
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string ContactNo { get; set; }
        public string EmailID { get; set; }
        public DateTime? ExpiryDate { get; set; }
    
        public string AccessType { get; set; }
        public bool IsDirectAccess { get; set; }

        public List<int> RoleIDs { get; set; } = new List<int>();
        public List<int> LocationIDs { get; set; } = new List<int>();
        public List<int> SelectedMenuRights { get; set; } = new List<int>();
    }
}
