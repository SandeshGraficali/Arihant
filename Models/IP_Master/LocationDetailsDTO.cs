namespace Arihant.Models.IP_Master
{
    public class LocationDetailsDTO
    {
        public List<IPAddressDTO> IPs { get; set; }
        public List<UserDTO> Users { get; set; }
    }
    public class IPAddressDTO { public int IPAddrID { get; set; } public string IPAddress { get; set; } public string IP_Name { get; set; } }

    public class UserDTO { public string UserID { get; set; } }
}
