using Arihant.Models.Common;

namespace Arihant.Models.Vender
{
    public class VendorMasterVM
    {
        public string? VendorID { get; set; }
        public string? VendorName { get; set; }
        public string? GSTNo { get; set; }

        public string? EmailID { get; set; }
        public string? PANNO { get; set; }
        public string? TINNO { get; set; }
        public string? Compare_Payment_Terms { get; set; }
        public string? Payment_Terms { get; set; }
        public string? Remark { get; set; }
        public string? IsActive { get; set; }
        public string? Email { get; set; }
        public string? AlternateEmail { get; set; }
        public string? ContactPersonName { get; set; }
        public string? OffMobileNO { get; set; }
        public string? WhatsappNo { get; set; }
        public string? Website { get; set; }
        public string? Tel1 { get; set; }
        public string? MobileNo { get; set; }

        public List<AddressDTO>  AddressList { get; set; } = new();
        public List<BankDTO> BankList { get; set; } = new();
    }


}
