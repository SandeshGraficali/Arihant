namespace Arihant.Models.Client
{
    public class ContactModel
    {
        public string Email { get; set; }
        public string AlternateEmail { get; set; }
        public string ContactPersonName { get; set; }
        public string OffMobileNO { get; set; }
        public string WhatsappNo { get; set; }
        public string Website { get; set; }
        public string Telephone { get; set; }
        public List<string> MobileNumbers { get; set; }
    }
}
