using System.ComponentModel.DataAnnotations;

namespace Arihant.Models.Bom_Master
{
    public class BOMMaster
    {
        public long BOM_id { get; set; }

        [Required(ErrorMessage = "BOM Name is required")]
        public string BOM_Name { get; set; }

        [Required(ErrorMessage = "Product (FG) ID is required")]
        public long fg_id { get; set; }

        [Required(ErrorMessage = "Quantity is required")]
        public decimal Quantity { get; set; }

        public string is_active { get; set; } = "Y";

        public string created_by { get; set; } = "admin";
        public long material_id { get; set; }
                                             
        public List<BOMDetail> BOMDetails { get; set; }
    }
}
