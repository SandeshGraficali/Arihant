using Arihant.Models.Common;
using Arihant.Models.Vender;

namespace Arihant.Models.Company_Master
{
    public class CompanyViewModel
    {
        public string CompanyID { get; set; }
        public string CompanyName { get; set; }
        public string CEegistration { get; set; } 
        public string CERange { get; set; }
        public string TINNO { get; set; }
        public string VATNO { get; set; }
        public string CSTNO { get; set; }
        public string STNO { get; set; }
        public string ECCNO { get; set; }
        public string GSTNo { get; set; }
        public string CompanyemailID { get; set; }
  
        public string Email { get; set; }
        public string AlternateEmail { get; set; }
        public string ContactPersonName { get; set; }
        public string OffMobileNO { get; set; }
        public string WhatsappNo { get; set; }

        public string Website { get; set; }
        public string Tel1 { get; set; }
        public string MobileNo { get; set; }

        public List<AddressDTO> AddressList { get; set; } = new();
        public List<BankDTO> BankList { get; set; } = new();
    }
}
