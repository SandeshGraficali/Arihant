using Arihant.Models.Common;

namespace Arihant.Models.Client
{
    public class ClientSubmissionViewModel
    {
        public int ClientID { get; set; }
    
        public string? Email { get; set; }
        public string? AlternateEmail { get; set; }
        public string? ContactPersonName { get; set; }
        public string? OffMobileNO { get; set; }
        public string? WhatsappNo { get; set; }
        public string? Website { get; set; }
        public string? Tel1 { get; set; }
        public string? MobileNo { get; set; }
        public string? IsActive { get; set; }
        public ClientDetailModel ClientDetails { get; set; }
        public LogisticsModel Logistics { get; set; }
        public List<AddressDTO> AddressList { get; set; } = new();
    }


}
