namespace Arihant.Models.Menu
{
    public class MenuViewModel
    {
        public string Module { get; set; }
        public string RightName { get; set; }
        public string UrlPage { get; set; }
        public int ParentMenuID { get; set; }
        public int MenuID { get; set; }
        public string RightAllowed { get; set; }
    }
}
