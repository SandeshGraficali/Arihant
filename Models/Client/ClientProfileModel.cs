namespace Arihant.Models.Client
{
    public class ClientProfileModel
    {
        public int ClientID { get; set; }
        public string ClientName { get; set; }
        public string GSTNo { get; set; }
        public string PANNo { get; set; }
        
        public string TransporterName { get; set; }
        public string IsActive { get; set; }

    }
}
