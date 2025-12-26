namespace Arihant.Models.Company_Master
{
    public class CompanyViewModel
    {
        public string CompanyName { get; set; }
        public string RegNo { get; set; }
        public string GSTNo { get; set; }
        public string PANNo { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string Telephone { get; set; }
        public List<string> MobileNumbers { get; set; }
        public AddressInfo SalesAddress { get; set; }
        public AddressInfo PurchaseAddress { get; set; }
        public string BankName { get; set; }
        public string AccountNo { get; set; }
        public string IFSCCode { get; set; }
        public string BankAddress { get; set; }
        public string BankLandmark { get; set; }
        public string BankPincode { get; set; }
        public string BankCity { get; set; }
        public string BankState { get; set; }
    }
}
