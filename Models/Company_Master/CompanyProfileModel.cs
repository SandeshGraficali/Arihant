using Arihant.Models.Client;
using Arihant.Models.Common;
using Arihant.Models.Vender;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json;

namespace Arihant.Models.Company_Master
{
    public class CompanyProfileModel
    {
        public int CompanyID { get; set; }
        public string CompanyName { get; set; }
        public string GSTNo { get; set; }
        public string Central_Excise_Registration { get; set; }
        public string Central_Excise_Range { get; set; }
        public string TIN_NO { get; set; }
        public string VAT_NO { get; set; }
        public string CSTNO { get; set; }
        public string STNO { get; set; }
        public string ECCNO { get; set; }
        public string CompanyemailID { get; set; }
        public string IsActive { get; set; }



        public string Email { get; set; }
        public string Website { get; set; }
        public string Tel1 { get; set; }
        public string ContactPersonName { get; set; }
        public string OfficeMobileNO { get; set; }
        public string WhatsAppNO { get; set; }
        public string AlternetEmail { get; set; }
        public string MobileNumber { get; set; }
        public string MobileNumber2 { get; set; }
        public string MobileNumber3 { get; set; }
        public string MobileNumber4 { get; set; }
        public string MobileNumber5 { get; set; }

        public string AddressList { get; set; }
        public string BankList { get; set; }
        public List<AddressDTO> AddressDetails
        {
            get => string.IsNullOrEmpty(AddressList) ? new List<AddressDTO>()
                   : JsonSerializer.Deserialize<List<AddressDTO>>(AddressList);
        }

        public List<BankDTO> BankDetails
        {
            get => string.IsNullOrEmpty(BankList) ? new List<BankDTO>()
                   : JsonSerializer.Deserialize<List<BankDTO>>(BankList);
            //}

        }
    }
}
