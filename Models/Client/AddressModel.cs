namespace Arihant.Models.Client
{
    public class AddressModel
    {
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string CityID { get; set; } // Changed to string based on your console log
        public string CountryID { get; set; }
        public string Landmark { get; set; }
        public string Pincode { get; set; }
        public string StateID { get; set; }
    }
}
