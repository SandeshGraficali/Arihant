namespace Arihant.Models.User_Master
{
    public class UserUpdateModel
    {
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string ContactNo { get; set; }
        public string EmailID { get; set; }
        public List<string> IPAddrID { get; set; }
    }
}
