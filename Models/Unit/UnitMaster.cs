using Arihant.Models.Common;

namespace Arihant.Models.Unit
{
    public class UnitMaster
    {
        public string? UnitID { get; set; }
        public string? companyName { get; set; }
        public string? factoryName { get; set; }

        public string? unitCode { get; set; }

        public string? IsActive { get; set; }
        public string? Email { get; set; }
        public string? AlternateEmail { get; set; }
        public string? ContactPersonName { get; set; }
        public string? OffMobileNO { get; set; }
        public string? WhatsappNo { get; set; }
        public string? Website { get; set; }
        public string? Tel1 { get; set; }
        public string? MobileNo { get; set; }

        public List<AddressDTO> AddressList { get; set; } = new();
        public List<BankDTO> BankList { get; set; } = new();
    }
}
