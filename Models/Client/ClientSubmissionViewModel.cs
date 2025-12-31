namespace Arihant.Models.Client
{
    public class ClientSubmissionViewModel
    {
        public int ClientID { get; set; }
        public ClientDetailModel ClientDetails { get; set; }
        public LogisticsModel Logistics { get; set; }
        public ContactModel ContactDetails { get; set; }
        public AddressModel CompanyAddress { get; set; }
        public AddressModel DeliveryAddress { get; set; }
    }
}
