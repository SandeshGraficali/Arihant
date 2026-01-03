namespace Arihant.Models.IP_Master
{
    public class IP_Master_Model
    {
        public string ID { get; set; }
        public string Location { get; set; }

        public List<IPRow> IPList { get; set; } = new List<IPRow>();
    }
    public class IPRow
    {
        public string IPAdd { get; set; }
        public string IP_Name { get; set; }
    }

    public class IPRowADd
    {
        public string LocationID { get; set; }
        public string IPAdd { get; set; }
        public string IP_Name { get; set; }
    }

}
