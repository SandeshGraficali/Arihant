namespace Arihant.Models.Client
{
    public class LogisticsModel
    {
        public string BookedTo { get; set; }
        public string BookingType { get; set; }
        public string CreditPeriod { get; set; } // String from console "30"
        public string ExportType { get; set; }
        public string FreightType { get; set; }
        public bool IndicateViaMail { get; set; }
        public string OriginalInvoiceCopy { get; set; }
        public string OwnerID { get; set; }
        public string PaymentTerms { get; set; }
        public string TransporterName { get; set; }
    }
}
