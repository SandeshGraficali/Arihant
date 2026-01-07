namespace Arihant.Models.User_Master
{
    public class UserSaveViewModel
    {
        public string? UserID { get; set; }
        public string UserName { get; set; }
        public string ContactNo { get; set; }
        public string EmailID { get; set; }
        public string? ExpiryDate { get; set; }
        public string Password { get; set; }
        public string AccessType { get; set; }
        public bool IsDirectAccess { get; set; }

        public List<int> RoleIDs { get; set; }
        public List<int> LocationIDs { get; set; }
        public List<int> SelectedMenuRights { get; set; }
    }



}
