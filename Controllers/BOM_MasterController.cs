using Arihant.Models.Bom_Master;
using Arihant.Services;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.Common;

namespace Arihant.Controllers
{
    public class BOM_MasterController : Controller
    {
        private readonly DL_BOM _bom;
        public BOM_MasterController(DL_BOM _master)
        {
            _bom = _master;
        }

        public IActionResult Index()
        {
            DataSet dataSet = _bom.GetProductTNG();
            DataTable dt = dataSet.Tables[0];

            List<string> productTypes = dt.AsEnumerable().Select(r => r["fg_type"].ToString()).Distinct().ToList();
            ViewBag.ProductTypes = productTypes;
            DataSet MaterialDS = _bom.GetMaterialTNS();
            DataTable Matdt = MaterialDS.Tables[0];
            List<string> materialType = new List<string>();
            foreach (DataRow row in Matdt.Rows)
            {
                string name = row["material_type"].ToString();
                if (!materialType.Contains(name))
                {
                    materialType.Add(name);
                }
            }

            ViewBag.MaterialType = materialType;
            return View();
        }
        public ActionResult Test()
        {
            DataSet dataSet = _bom.GetProductTNG();
            DataTable dt = dataSet.Tables[0];

            List<string> productTypes = dt.AsEnumerable().Select(r => r["fg_type"].ToString()).Distinct().ToList();
            ViewBag.ProductTypes = productTypes;
            DataSet MaterialDS = _bom.GetMaterialTNS();
            DataTable Matdt = MaterialDS.Tables[0];
            List<string> materialType = new List<string>();

            foreach (DataRow row in Matdt.Rows)
            {
                string name = row["material_type"].ToString();
                if (!materialType.Contains(name))
                {
                    materialType.Add(name);
                }
            }

            ViewBag.MaterialType = materialType;
            return View();
        }
        [HttpGet]
        public JsonResult GetProductDetails(string productType)
        {
            if (string.IsNullOrEmpty(productType))
                return Json(new { success = false, message = "No product type provided." });

            try
            {
                DataSet ds = _bom.GetProductTNGParam(productType);

                if (ds == null || ds.Tables.Count == 0)
                    return Json(new { success = false, message = "No data found." });

                DataTable dt = ds.Tables[0];
                if (!dt.Columns.Contains("FG_Name") || !dt.Columns.Contains("fg_application") || !dt.Columns.Contains("FGID"))
                    return Json(new { success = false, message = "Required columns not found." });

                var productMap = dt.AsEnumerable().GroupBy(r => r["FG_Name"].ToString()).ToDictionary(
                        g => g.Key,
                        g => new
                        {
                            Grades = g.Select(r => r["fg_application"].ToString()).Distinct().ToList(),
                            FGIDs = g.Where(r => r["FGID"] != DBNull.Value).Select(r => Convert.ToInt32(r["FGID"])).ToList()
                        });

                return Json(new { success = true, data = productMap });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in GetProductDetails: " + ex);
                return Json(new { success = false, message = "Server error occurred." });
            }
        }


        [HttpGet]
        public JsonResult GetMaterialProductByAllParams(string productType, string productName, string productGrade)
        {
            if (string.IsNullOrEmpty(productType) || string.IsNullOrEmpty(productName) || string.IsNullOrEmpty(productGrade))
            {
                return Json(new { success = false, message = "All parameters must be provided." });
            }

            try
            {
                DataSet ds = _bom.GetProductTNSParamFull(productType, productName, productGrade);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                    return Json(new { success = false, message = "No data found." });

                DataTable dt = ds.Tables[0];
                var FGID = dt.Rows[0]["FGID"] != DBNull.Value ? dt.Rows[0]["FGID"].ToString() : null;
                var UOM = dt.Rows[0]["UOM"] != DBNull.Value ? dt.Rows[0]["UOM"].ToString() : null;

                return Json(new
                {
                    success = true,
                    fgid = FGID,
                    UOM = UOM
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in GetMaterialDetailsByAllParams: " + ex);
                return Json(new { success = false, message = "Server error occurred." });
            }
        }

        [HttpGet]
        public JsonResult GetMaterialDetails(string productType)
        {
            if (string.IsNullOrEmpty(productType))
                return Json(new { success = false, message = "No product type provided." });

            try
            {
                DataSet ds = _bom.GetMaterialTNSParam(productType);
                if (ds == null || ds.Tables.Count == 0)
                    return Json(new { success = false, message = "No data found." });

                DataTable dt = ds.Tables[0];

                if (!dt.Columns.Contains("material_name") || !dt.Columns.Contains("material_specification"))
                    return Json(new { success = false, message = "Required columns not found." });

                var nameGradeMap = dt.AsEnumerable().GroupBy(r => r["material_name"].ToString()).ToDictionary(
                        g => g.Key,
                        g => g.Select(r => r["material_specification"].ToString()).Distinct().ToList()
                    );

                return Json(new { success = true, data = nameGradeMap });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in GetMaterialDetails: " + ex);
                return Json(new { success = false, message = "Server error occurred." });
            }
        }


        [HttpGet]
        public JsonResult GetMaterialDetailsByAllParams(string materialType, string materialName, string materialGrade)
        {
            if (string.IsNullOrEmpty(materialType) || string.IsNullOrEmpty(materialName) || string.IsNullOrEmpty(materialGrade))
            {
                return Json(new { success = false, message = "All parameters must be provided." });
            }

            try
            {
                DataSet ds = _bom.GetMaterialTNSParamfull(materialType, materialName, materialGrade);

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                    return Json(new { success = false, message = "No data found." });

                DataTable dt = ds.Tables[0];

                var materialID = dt.Rows[0]["material_id"].ToString();
                var Uom = dt.Rows[0]["UOM"].ToString();
                return Json(new
                {
                    success = true,
                    materialId = materialID,
                    UOM = Uom
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in GetMaterialDetailsByAllParams: " + ex);
                return Json(new { success = false, message = "Server error occurred." });
            }
        }


        [HttpGet]
        public JsonResult GetAllBOMWithProductIndexWithParameters(string productType, string productName, string productGrade)
        {
            try
            {

                DataSet ds = _bom.ToGetAlltheBOMdetailsWithParametersFGTNA(productType, productName, productGrade);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                    return Json(new { success = false, message = "No data found." });

                DataTable dt = ds.Tables[0];

                var bomList = dt.AsEnumerable().Select(row => new
                {
                    BOM_id = row["BOM_id"],
                    BOM_Name = row["BOM_Name"],
                    FG_Type = row["fg_type"],
                    FG_Name = row["FG_name"],
                    FG_Application = row["fg_application"],
                    Quantity = row["Quantity"],
                    Total_Materials = row["Total_Materials"],
                    IsActive = row["is_active"],
                    UOM = row["UOM"],
                    TotalQuantityMaterial = row["TotalQuantityMaterial"],
                    CreatedDate = Convert.ToDateTime(row["created_date"]).ToString("dd-MM-yyyy"),
                    ModifiedDate =
                        row["modified_date"] == DBNull.Value || string.IsNullOrWhiteSpace(row["modified_date"].ToString())
                        ? Convert.ToDateTime(row["created_date"]).ToString("dd-MM-yyyy")   // fallback to created date
                        : Convert.ToDateTime(row["modified_date"]).ToString("dd-MM-yyyy")
                }).ToList();

                return Json(new { success = true, data = bomList });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Server error: " + ex.Message });
            }
        }


        [HttpPost]
        public JsonResult GetAllTheDeatilsBasedOnFilterSelected(string productType, string productName = null, string productGrade = null)
        {
            if (string.IsNullOrEmpty(productType))
                return Json(new { success = false, message = "Product Type must be provided." });

            DataSet ds = _bom.GetAlltheBOMdetailsFirstLayer(productType, productName, productGrade);
            var bomList = ds.Tables[0].AsEnumerable().Select(row => new
            {
                BOM_Name = row.Table.Columns.Contains("BOM_Name") ? row["BOM_Name"].ToString() : null,
                FG_Type = row.Table.Columns.Contains("fg_type") ? row["fg_type"].ToString() : null,
                FG_Name = row.Table.Columns.Contains("FG_name") ? row["FG_name"].ToString() : null,
                FG_Application = row.Table.Columns.Contains("fg_application") ? row["fg_application"].ToString() : null,
                Total = row.Table.Columns.Contains("Total") ? Convert.ToInt32(row["Total"]) : 0

            }).ToList();

            return Json(new { success = true, data = bomList });
        }

        [HttpPost]
        public JsonResult GetAllTheDeatilsBasedOnFilterName(string productType, string productName = null, string productGrade = null)
        {
            if (string.IsNullOrEmpty(productType))
                return Json(new { success = false, message = "Product Type must be provided." });

            string fgTypeParam = string.IsNullOrEmpty(productType) ? "NULL" : $"'{productType.Replace("'", "''")}'";
            string fgNameParam = string.IsNullOrEmpty(productName) ? "NULL" : $"'{productName.Replace("'", "''")}'";
            string fgApplicationParam = string.IsNullOrEmpty(productGrade) ? "NULL" : $"'{productGrade.Replace("'", "''")}'";
            DataSet ds = _bom.GetAlltheBOMdetailsFirstLayer(fgTypeParam, fgNameParam, fgApplicationParam);
            DataTable dt = ds.Tables[0];

            var bomList = dt.AsEnumerable().Select(row => new
            {
                BOM_id = row["BOM_id"],
                BOM_Name = row["BOM_Name"],
                FG_Type = row["fg_type"],
                FG_Name = row["FG_name"],
                FG_Application = row["fg_application"],
                Quantity = row["Quantity"],
                Total_Materials = row["Total_Materials"],
                IsActive = row["is_active"],
                UOM = row["UOM"],
                CreatedDate = Convert.ToDateTime(row["created_date"]).ToString("dd-MM-yyyy")
            }).ToList();

            return Json(new { success = true, data = bomList });
        }



        [HttpGet]
        public JsonResult EditBOMDetailsGet(long bomId)
        {
            try
            {
                DataSet dsBOM = _bom.BOMDetailsGet(bomId);
                List<Dictionary<string, object>> bomMaster = new List<Dictionary<string, object>>();

                if (dsBOM != null && dsBOM.Tables.Count > 0)
                {
                    foreach (DataRow row in dsBOM.Tables[0].Rows)
                    {
                        var dict = new Dictionary<string, object>();
                        foreach (DataColumn col in dsBOM.Tables[0].Columns)
                            dict[col.ColumnName] = row[col];
                        bomMaster.Add(dict);
                    }
                }

                DataSet dsDetails = _bom.BOMDetailsGetNew(bomId);
                List<Dictionary<string, object>> bomDetails = new List<Dictionary<string, object>>();

                if (dsDetails != null && dsDetails.Tables.Count > 0)
                {
                    foreach (DataRow row in dsDetails.Tables[0].Rows)
                    {
                        var dict = new Dictionary<string, object>();
                        foreach (DataColumn col in dsDetails.Tables[0].Columns)
                            dict[col.ColumnName] = row[col];
                        bomDetails.Add(dict);
                    }
                }

                return Json(new { success = true, BOMMaster = bomMaster, BOMDetails = bomDetails });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult DeleteBOMDetailsByBOMId(long BOM_id)
        {
            try
            {
                DataSet ds = _bom.DeleteBOMDetailsByBOMId(BOM_id);
                return Json(new { success = true, message = "BOM details deleted successfully!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


        [HttpPost]
        public JsonResult EditBOMDetailsPost(BOMMaster model, List<BOMDetail> materials)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, message = "Invalid input." });

            try
            {
                _bom.EditBOMDetailsPost(model, materials);
                return Json(new { success = true, message = "BOM updated successfully!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult DeleteBOMDetails(int BOMID)
        {
            try
            {
                _bom.DeleteBOMDetails(BOMID);
                return Json(new { success = true, message = "BOM details deleted successfully!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error deleting BOM: " + ex.Message });
            }
        }

        [HttpPost]
        public JsonResult SetIsActive(int BOMID, bool IsActive)
        {
            try
            {
                _bom.SetIsActive(BOMID , IsActive);
                string message = IsActive ? "BOM Activated!" : "BOM Deactivated!";
                return Json(new { success = true, message = message });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error updating BOM status: " + ex.Message });
            }
        }

        [HttpPost]
        public JsonResult SaveOrUpdateBOM([FromBody] BOMMaster model)
        {
           
            try
            {
                long bomId = model.BOM_id;

                if (bomId > 0)
                {
                    _bom.UpdateBOMMaster(model);
                    _bom.DeleteBOMDetailsByBOMID(model);
                   
                }
                else
                {
                   
                    DataSet ds = _bom.InsertBOMMaster(model);
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        bomId = Convert.ToInt64(ds.Tables[0].Rows[0]["BOM_id"]);
                    }
                }

                _bom.InsertBOMDetails(model , bomId);

                return Json(new { success = true, message = "BOM saved successfully!", BOM_id = bomId });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


        public ActionResult ExportBOM(int BOMID)
        {

            DataSet dsMaster =  _bom.FinishGoodsMaster(BOMID);
            DataTable dtMaster = dsMaster.Tables[0];

            
             DataSet dsDetails = _bom.RawMaterialMaster(BOMID);
            DataTable dtDetails = dsDetails.Tables[0];

            string qty = dtMaster.Rows[0]["Quantity"].ToString();
            string uom = dtMaster.Rows[0]["UOM"].ToString();
            string fgName = dtMaster.Rows[0]["FG_Name"].ToString();
            string fgType = dtMaster.Rows[0]["fg_type"].ToString();
            string fgApp = dtMaster.Rows[0]["fg_application"].ToString();
            string BOMname = dtMaster.Rows[0]["BOM_Name"].ToString();

            decimal totalMaterialQty = dtDetails.AsEnumerable()
                .Where(r => r["UOM"].ToString() == uom)  
                .Sum(r => Convert.ToDecimal(r["QuantityPerUnit"]));

            using (var workbook = new XLWorkbook())
            {
                var ws = workbook.Worksheets.Add($"{BOMname}");
                ws.Cell("A1").Value = $"BOM Name: {BOMname}";
                ws.Range("A1:F1").Merge().Style.Font.Bold = true;
                ws.Range("A1:F1").Style.Font.FontSize = 14;
                ws.Row(2).InsertRowsAbove(1); 
                ws.Cell("A3").Value = $"To produce {qty} {uom} of {fgName} / {fgType} / {fgApp}.";
                ws.Range("A3:F3").Merge().Style.Font.Bold = true;
                ws.Range("A3:F3").Style.Font.FontSize = 12;
                ws.Row(4).InsertRowsAbove(1); 
                ws.Cell("A5").Value = "BOM Information";
                ws.Range("A5:F5").Merge().Style.Font.Bold = true;
                ws.Range("A5:F5").Style.Font.FontSize = 13;
                ws.Row(6).InsertRowsAbove(1);
                var masterTable = ws.Cell(7, 1).InsertTable(dtMaster);
                masterTable.Field("BOM_Name").Name = "BOM Name";
                masterTable.Field("fg_type").Name = "Product Type";
                masterTable.Field("FG_Name").Name = "Product Name";
                masterTable.Field("fg_application").Name = "Product Grade";
                masterTable.Field("Quantity").Name = "Quantity";
                masterTable.Field("UOM").Name = "UOM";

                int afterMasterRow = 7 + dtMaster.Rows.Count + 2; 
                ws.Cell(afterMasterRow, 1).Value = $"Total Material Quantity : {totalMaterialQty} {uom}";
                ws.Range(afterMasterRow, 1, afterMasterRow, 6).Merge().Style.Font.Bold = true;
                ws.Range(afterMasterRow, 1, afterMasterRow, 6).Style.Font.FontSize = 12;
                ws.Row(afterMasterRow + 1).InsertRowsAbove(1);

                int nextRow = afterMasterRow + 2;
                ws.Cell(nextRow, 1).Value = "Raw Material Details";
                ws.Range(nextRow, 1, nextRow, 6).Merge().Style.Font.Bold = true;
                ws.Range(nextRow, 1, nextRow, 6).Style.Font.FontSize = 13;

                ws.Row(nextRow + 1).InsertRowsAbove(1);
                var detailsTable = ws.Cell(nextRow + 2, 1).InsertTable(dtDetails);
                detailsTable.Field("material_type").Name = "Material Type";
                detailsTable.Field("material_name").Name = "Material Name";
                detailsTable.Field("material_specification").Name = "Material Grade";
                detailsTable.Field("QuantityPerUnit").Name = "Quantity Per Unit";
                detailsTable.Field("UOM").Name = "UOM";
                ws.Columns().AdjustToContents();
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    string fileName = $"BOM_{BOMname}.xlsx";
                    return File(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        fileName);
                }
            }


        }

        [HttpPost]
        public ActionResult ExportMultipleBOM([FromBody] List<int> bomIds)
        {
            if (bomIds == null || bomIds.Count == 0)
                return Json(new { success = false, message = "No BOM IDs received" });

            using (var workbook = new XLWorkbook())
            {

                foreach (int BOMID in bomIds)
                {
                    DataSet dsMaster = _bom.FinishGoodsMaster(BOMID);
                    DataTable dtMaster = dsMaster.Tables[0];

                    if (dtMaster.Rows.Count == 0)
                        continue;

                    string bomDetailsQuery = $@"
                                    SELECT r.material_type, r.material_name, r.material_specification, 
                                           d.QuantityPerUnit, r.UOM
                                    FROM BOMDetails d
                                    INNER JOIN RawMaterialMaster r ON d.material_id = r.material_id
                                    WHERE d.BOM_id = {BOMID}";

                    DataSet dsDetails = _bom.RawMaterialMaster(BOMID);
                    DataTable dtDetails = dsDetails.Tables[0];


                    string qty = dtMaster.Rows[0]["Quantity"].ToString();
                    string uom = dtMaster.Rows[0]["UOM"].ToString();
                    string fgName = dtMaster.Rows[0]["FG_Name"].ToString();
                    string fgType = dtMaster.Rows[0]["fg_type"].ToString();
                    string fgApp = dtMaster.Rows[0]["fg_application"].ToString();
                    string BOMname = dtMaster.Rows[0]["BOM_Name"].ToString();

                    if (string.IsNullOrWhiteSpace(BOMname))
                        BOMname = "BOM_" + BOMID;


                    string baseName = BOMname.Length > 31 ? BOMname.Substring(0, 31) : BOMname;
                    string wsName = baseName;
                    int counter = 1;

                    while (workbook.Worksheets.Any(w => w.Name.Equals(wsName, StringComparison.OrdinalIgnoreCase)))
                    {
                        string suffix = $"({counter})";
                        int maxLength = 31 - suffix.Length;
                        wsName = baseName.Length > maxLength ? baseName.Substring(0, maxLength) + suffix : baseName + suffix;
                        counter++;
                    }


                    var ws = workbook.Worksheets.Add(wsName);

                    decimal totalMaterialQty = dtDetails
                        .AsEnumerable()
                        .Where(r => r["UOM"].ToString() == uom)
                        .Sum(r => Convert.ToDecimal(r["QuantityPerUnit"]));

                    ws.Cell("A1").Value = $"BOM Name: {BOMname}";
                    ws.Range("A1:F1").Merge().Style.Font.Bold = true;
                    ws.Range("A1:F1").Style.Font.FontSize = 14;

                    ws.Row(2).InsertRowsAbove(1);

                    ws.Cell("A3").Value = $"To produce {qty} {uom} of {fgName} / {fgType} / {fgApp}.";
                    ws.Range("A3:F3").Merge().Style.Font.Bold = true;
                    ws.Range("A3:F3").Style.Font.FontSize = 12;

                    ws.Row(4).InsertRowsAbove(1);

                    ws.Cell("A5").Value = "BOM Information";
                    ws.Range("A5:F5").Merge().Style.Font.Bold = true;
                    ws.Range("A5:F5").Style.Font.FontSize = 13;

                    ws.Row(6).InsertRowsAbove(1);

                    var masterTable = ws.Cell(7, 1).InsertTable(dtMaster);
                    masterTable.Field("BOM_Name").Name = "BOM Name";
                    masterTable.Field("fg_type").Name = "Product Type";
                    masterTable.Field("FG_Name").Name = "Product Name";
                    masterTable.Field("fg_application").Name = "Product Grade";
                    masterTable.Field("Quantity").Name = "Quantity";
                    masterTable.Field("UOM").Name = "UOM";

                    int afterMasterRow = 7 + dtMaster.Rows.Count + 2;

                    ws.Cell(afterMasterRow, 1).Value = $"Total Material Quantity : {totalMaterialQty} {uom}";
                    ws.Range(afterMasterRow, 1, afterMasterRow, 6).Merge().Style.Font.Bold = true;

                    ws.Row(afterMasterRow + 1).InsertRowsAbove(1);

                    int nextRow = afterMasterRow + 2;

                    ws.Cell(nextRow, 1).Value = "Raw Material Details";
                    ws.Range(nextRow, 1, nextRow, 6).Merge().Style.Font.Bold = true;

                    ws.Row(nextRow + 1).InsertRowsAbove(1);

                    var detailsTable = ws.Cell(nextRow + 2, 1).InsertTable(dtDetails);
                    detailsTable.Field("material_type").Name = "Material Type";
                    detailsTable.Field("material_name").Name = "Material Name";
                    detailsTable.Field("material_specification").Name = "Material Grade";
                    detailsTable.Field("QuantityPerUnit").Name = "Quantity Per Unit";
                    detailsTable.Field("UOM").Name = "UOM";

                    ws.Columns().AdjustToContents();
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return File(
                        stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "Multiple_BOM_Export.xlsx"
                    );
                }
            }
        }
    }
}
