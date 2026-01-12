namespace Arihant.Models.Common
{
    public class AddressDTO
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
        public string? GSTNO { get; set; }
    }
}
