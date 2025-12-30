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


        // Contact Details
        public string Email { get; set; }
        public string Website { get; set; }
        public string Telephone { get; set; }
        public string ContactPersonName { get; set; }
        public string OfficeMobileNO { get; set; }
        public string WhatsAppNO { get; set; }
        public string AlternetEmail { get; set; }
        public string MobileNumber { get; set; }
        public string MobileNumber2 { get; set; }
        public string MobileNumber3 { get; set; }
        public string MobileNumber4 { get; set; }
        public string MobileNumber5 { get; set; }

        // Bank Details
        public string BankName { get; set; }
        public string AccountNo { get; set; }
        public string AccountHolderName { get; set; }
        public string AccountType { get; set; }
        public string IFSCCode { get; set; }

        // Address Fields (Matching SQL Aliases)
        public string Company_Address1 { get; set; }
        public string Company_Address2 { get; set; }
        public string Company_Landmark { get; set; }
        public string Company_PinCode { get; set; }
        public string Company_City { get; set; }
        public string Company_State { get; set; }
        public string Company_Country { get; set; }

        public string CE_Address1 { get; set; }
        public string CE_Address2 { get; set; }
        public string CE_Landmark { get; set; }
        public string CE_PinCode { get; set; }
        public string CE_City { get; set; }
        public string CE_State { get; set; }
        public string CE_Country { get; set; }

        public string Div_Address1 { get; set; }
        public string Div_Address2 { get; set; }
        public string Div_Landmark { get; set; }
        public string Div_PinCode { get; set; }
        public string Div_City { get; set; }
        public string Div_State { get; set; }
        public string Div_Country { get; set; }

        public string Comm_Address1 { get; set; }
        public string Comm_Address2 { get; set; }
        public string Comm_Landmark { get; set; }
        public string Comm_PinCode { get; set; }
        public string Comm_City { get; set; }
        public string Comm_State { get; set; }
        public string Comm_Country { get; set; }

        public string Bank_Address1 { get; set; }
        public string Bank_Address2 { get; set; }
        public string Bank_Landmark { get; set; }
        public string Bank_PinCode { get; set; }
        public string Bank_City { get; set; }
        public string Bank_State { get; set; }
        public string Bank_Country { get; set; }
    }
}
