namespace Arihant.Models.Vender
{
    public class VendorMasterVM
    {
        public string? VendorID { get; set; }
        public string VendorName { get; set; }
        public string GSTNo { get; set; }

        public string EmailID { get; set; }
        public string PANNO { get; set; }
        public string Compare_Payment_Terms { get; set; }
        public string Payment_Terms { get; set; }
        public string Remark { get; set; }

        public string Email { get; set; }
        public string AlternateEmail { get; set; }
        public string ContactPersonName { get; set; }
        public string OffMobileNO { get; set; }
        public string WhatsappNo { get; set; }
        public string Website { get; set; }
        public string Tel1 { get; set; }
        public string MobileNo { get; set; }

        public List<AddressVM> AddressList { get; set; } = new();
        public List<BankVM> BankList { get; set; } = new();
    }

    public class AddressVM
    {
        public string AddressTypeID { get; set; }

        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string Landmark { get; set; }

        public string Pincode { get; set; }

        public string CityID { get; set; }
        public string StateID { get; set; }
        public string CountryID { get; set; }

        public string? CityName { get; set; }
        public string? StateName { get; set; }
        public string? CountryName { get; set; }

    }

    public class BankVM
    {
        public string BankName { get; set; }
        public string AccountNo { get; set; }
        public string IFSCCode { get; set; }
        public string AccountType { get; set; }
        public string AccountHolderName { get; set; }

        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string Landmark { get; set; }
        public string Pincode { get; set; }

        public string CityID { get; set; }
        public string StateID { get; set; }
        public string CountryID { get; set; }
    }
}
