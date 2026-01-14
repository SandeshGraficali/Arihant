namespace Arihant.Models.Bom_Master
{
    public class BOMDetail
    {
        public long BOM_id { get; set; }
        public long material_id { get; set; }
        public decimal QuantityPerUnit { get; set; }
        public string BOM_Name { get; set; }
    }
}
