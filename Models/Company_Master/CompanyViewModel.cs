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
        public string Telephone { get; set; }
        public List<string> MobileNumbers { get; set; }

        public AddressInfo CompanyAddr { get; set; }
        public AddressInfo Central_ExciseAddr { get; set; }
        public AddressInfo DivisionAddr { get; set; }
        public AddressInfo CommissionerateAddr { get; set; }

        public string BankName { get; set; }
        public string AccountNo { get; set; }
        public string AccountHolderName { get; set; }
        public string AccountType { get; set; }
        public string IFSCCode { get; set; }
        public string BankAddressLine1 { get; set; }
        public string BankAddressLine2 { get; set; }
        public string BankLandmark { get; set; }
      
        public string BankPincode { get; set; }
        public string BankCity { get; set; }
        public string BankState { get; set; }
        public string BankCountry { get; set; }
    }
}
