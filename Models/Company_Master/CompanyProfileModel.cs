namespace Arihant.Models.Company_Master
{
    public class CompanyProfileModel
    {
        public int CompanyID { get; set; }
        public string CompanyName { get; set; }
        public string RegNo { get; set; }
        public string PANNo { get; set; }
        public string GSTNo { get; set; }

        public string Email { get; set; }
        public string? Website { get; set; }
        public string? MobileNumber { get; set; }
        public string? MobileNumber2 { get; set; }
        public string? MobileNumber3 { get; set; }
        public string? MobileNumber4 { get; set; }
        public string? MobileNumber5 { get; set; }


        public string BankName { get; set; }
        public string AccountNo { get; set; }
        public string IFSCCode { get; set; }

        public string? Sales_Address { get; set; }
        public string? Sales_State { get; set; }
        public string? Sales_City { get; set; }
        public string? Sales_PinCode { get; set; }
        public string? Sales_Landmark { get; set; }

        public string? Purchase_Address { get; set; }
        public string? Purchase_State { get; set; }
        public string? Purchase_City { get; set; }
        public string? Purchase_PinCode { get; set; }
        public string? Purchase_Landmark { get; set; }

        public string? Bank_Address { get; set; }
        public string? Bank_State { get; set; }
        public string? Bank_City { get; set; }
        public string? Bank_PinCode { get; set; }
    }
}
