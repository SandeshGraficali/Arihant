using Arihant.Models.Bom_Master;
using Azure;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.Common;
using System.Reflection;

namespace Arihant.Services
{
    public class DL_BOM
    {
        private readonly GraficaliClasses.GraficaliClasses gc;
        private readonly DL_Email _email;
        private readonly IConfiguration _configuration;
        public DL_BOM(GraficaliClasses.GraficaliClasses _gc, DL_Email user, IConfiguration configuration)
        {
            gc = _gc;
            _email = user;
            _configuration = configuration;
        }


        public DataSet GetProductTNG()
        {
            DataSet ds = gc.ExecuteStoredProcedureGetDataSet("sp_GetProductTNG");
            return ds;
        }

        public DataSet GetMaterialTNS()
        {
            DataSet ds = gc.ExecuteStoredProcedureGetDataSet("sp_GetMaterialTNS");
            return ds;
        }
        public DataSet GetProductTNGParam(string productType)
        {
            var parameters = new Dictionary<string, SqlParameter>
                {
                    { "Name", new SqlParameter("@Name", productType) }

                };

            DataSet ds = gc.ExecuteStoredProcedureGetDataSet("sp_GetProductTNGParam", parameters.Values.ToArray());
            return ds;
        }

        public DataSet GetProductTNSParamFull(string productType, string productName, string productGrade)
        {
            var parameters = new Dictionary<string, SqlParameter>
                {
                    { "ProductType", new SqlParameter("@ProductType", productType) },
                    { "ProductName", new SqlParameter("@ProductName", productName) },
                    { "ProductGrade", new SqlParameter("@ProductGrade", productGrade) },

                };

            DataSet ds = gc.ExecuteStoredProcedureGetDataSet("sp_GetProductTNSParamFull", parameters.Values.ToArray());
            return ds;
        }


        public DataSet GetMaterialTNSParam(string productType)
        {
            var parameters = new Dictionary<string, SqlParameter>
                {
                    { "Name", new SqlParameter("@Name", productType) }

                };

            DataSet ds = gc.ExecuteStoredProcedureGetDataSet("sp_GetMaterialTNSParam", parameters.Values.ToArray());
            return ds;
        }
        public DataSet GetMaterialTNSParamfull(string materialType, string materialName, string materialGrade)
        {
            var parameters = new Dictionary<string, SqlParameter>
                {
                    { "MaterialType", new SqlParameter("@MaterialType", materialType) },
                    { "MaterialName", new SqlParameter("@MaterialName", materialName) },
                    { "MaterialGrade", new SqlParameter("@MaterialGrade", materialGrade) },
                };
            DataSet ds = gc.ExecuteStoredProcedureGetDataSet("sp_GetMaterialTNSParamfull", parameters.Values.ToArray());
            return ds;
        }


        public DataSet ToGetAlltheBOMdetailsWithParametersFGTNA(string fg_type, string fg_name, string fg_application)
        {
            var parameters = new Dictionary<string, SqlParameter>
                {
                    { "fg_type", new SqlParameter("@fg_type", fg_type) },
                    { "fg_name", new SqlParameter("@fg_name", fg_name) },
                    { "fg_application", new SqlParameter("@fg_application", fg_application) },
                };
            DataSet ds = gc.ExecuteStoredProcedureGetDataSet("sp_ToGetAlltheBOMdetailsWithParametersFGTNA", parameters.Values.ToArray());
            return ds;
        }


        public DataSet GetAlltheBOMdetailsFirstLayer(string fg_type, string fg_name, string fg_application)
        {
            var parameters = new Dictionary<string, SqlParameter>
                {
                    { "fg_type", new SqlParameter("@fg_type", fg_type) },
                    { "fg_name", new SqlParameter("@fg_name", fg_name) },
                    { "fg_application", new SqlParameter("@fg_application", fg_application) },
                };
            DataSet ds = gc.ExecuteStoredProcedureGetDataSet("sp_ToGetAlltheBOMdetailsFirstLayer", parameters.Values.ToArray());
            return ds;
        }

        public DataSet GetAlltheBOMdetailsFirstLayerName(string fg_type, string fg_name, string fg_application)
        {
            var parameters = new Dictionary<string, SqlParameter>
                {
                    { "fg_type", new SqlParameter("@fg_type", fg_type) },
                    { "fg_name", new SqlParameter("@fg_name", fg_name) },
                    { "fg_application", new SqlParameter("@fg_application", fg_application) },
                };
            DataSet ds = gc.ExecuteStoredProcedureGetDataSet("sp_ToGetAlltheBOMdetailsFirstLayerName", parameters.Values.ToArray());
            return ds;
        }

        public DataSet BOMDetailsGet(long bomId)
        {
            string bomMasterQuery = $@"
                    SELECT b.BOM_id, b.BOM_Name, b.fg_id, b.Quantity,
                           f.fg_type, f.FG_Name, f.fg_application,f.UOM
                    FROM BOMMaster b
                    INNER JOIN FinishGoodsMaster f ON b.fg_id = f.FGID
                    WHERE b.BOM_id = {bomId}";
            DataSet dt = gc.GetDataSet(bomMasterQuery);

            return dt;
        }

        public DataSet BOMDetailsGetNew(long bomId)
        {
            string bomDetailsQuery = $@"
                    SELECT d.material_id, d.QuantityPerUnit,
                           r.material_type, r.material_name, r.material_specification, r.UOM
                    FROM BOMDetails d
                    INNER JOIN RawMaterialMaster r ON d.material_id = r.material_id
                    WHERE d.BOM_id = {bomId}";
            DataSet dt = gc.GetDataSet(bomDetailsQuery);

            return dt;
        }


        public DataSet DeleteBOMDetailsByBOMId(long BOM_id)
        {
            var parameters = new Dictionary<string, SqlParameter>
                {
                    { "BOM_id", new SqlParameter("@BOM_id", BOM_id) }

                };

            DataSet ds = gc.ExecuteStoredProcedureGetDataSet("sp_DeleteBOMDetailsByBOMId", parameters.Values.ToArray());
            return ds;
        }
        public void EditBOMDetailsPost(BOMMaster model, List<BOMDetail> materials)
        {
            gc.ExecuteCommand($"DELETE FROM BOMDetails WHERE BOM_id = {model.BOM_id}");
            gc.ExecuteCommand($@"UPDATE BOMMaster SET BOM_Name = '{model.BOM_Name.Replace("'", "''")}',Quantity = {model.Quantity},last_updated_by = 'admin',modified_date = GETDATE()WHERE BOM_id = {model.BOM_id}");

            foreach (var item in materials)
            {
                string insertQuery = $@"
                INSERT INTO BOMDetails (BOM_id, material_id, QuantityPerUnit, created_date)
                VALUES ({model.BOM_id}, {item.material_id}, {item.QuantityPerUnit}, GETDATE())";

                gc.ExecuteCommand(insertQuery);
            }
        }

        public void DeleteBOMDetails(int BOMID)
        {
            string sql = $"UPDATE BOMMaster SET is_deleted = 'Y' WHERE BOM_id = {BOMID}";
            gc.ExecuteCommand(sql);
        }

        public void SetIsActive(int BOMID, bool IsActive)
        {
            string newStatus = IsActive ? "Y" : "N";
            string sql = $"UPDATE BOMMaster SET is_active = '{newStatus}' WHERE BOM_id = {BOMID}";
            gc.ExecuteCommand(sql);
        }

        public DataSet UpdateBOMMaster(BOMMaster model)
        {
            var parameters = new Dictionary<string, SqlParameter>
                {
                    { "BOM_id", new SqlParameter("@BOM_id", model.BOM_id) },
                    { "BOM_Name", new SqlParameter("@BOM_Name", model.BOM_Name) },
                    { "fg_id", new SqlParameter("@fg_id", model.fg_id) },
                    { "Quantity", new SqlParameter("@Quantity", model.Quantity) },
                    { "is_active", new SqlParameter("@is_active", model.is_active) },
                       { "created_by", new SqlParameter("@created_by", model.created_by) },
                };
            DataSet ds = gc.ExecuteStoredProcedureGetDataSet("sp_UpdateBOMMaster", parameters.Values.ToArray());
            return ds;
        }


        public DataSet DeleteBOMDetailsByBOMID(BOMMaster model)
        {
            var parameters = new Dictionary<string, SqlParameter>
                {
                    { "BOM_id", new SqlParameter("@BOM_id", model.BOM_id) }
                   
                };
            DataSet ds = gc.ExecuteStoredProcedureGetDataSet("sp_DeleteBOMDetailsByBOMID", parameters.Values.ToArray());
            return ds;
        }

        public DataSet InsertBOMMaster(BOMMaster model)
        {
            var parameters = new Dictionary<string, SqlParameter>
                {
                 
                    { "BOM_Name", new SqlParameter("@BOM_Name", model.BOM_Name) },
                    { "fg_id", new SqlParameter("@fg_id", model.fg_id) },
                    { "Quantity", new SqlParameter("@Quantity", model.Quantity) },
                    { "is_active", new SqlParameter("@is_active", model.is_active) },
                       { "created_by", new SqlParameter("@created_by", model.created_by) },
                };
            DataSet ds = gc.ExecuteStoredProcedureGetDataSet("sp_InsertBOMMaster", parameters.Values.ToArray());
            return ds;
        }


        public void InsertBOMDetails(BOMMaster model , long bomId)
        {

            foreach (var item in model.BOMDetails)
            {
                var parameters = new Dictionary<string, SqlParameter>
                {

                    { "BOM_id", new SqlParameter("@BOM_id", bomId) },
                    { "material_id", new SqlParameter("@material_id", item.material_id) },
                    { "QuantityPerUnit", new SqlParameter("@QuantityPerUnit", item.QuantityPerUnit) }

                };

                DataSet ds = gc.ExecuteStoredProcedureGetDataSet("sp_InsertBOMDetails", parameters.Values.ToArray());
            }
        }

        public DataSet FinishGoodsMaster(int BOMID)
        {
            string bomMasterQuery = $@"
                    SELECT   b.BOM_Name,  
                           f.fg_type, f.FG_Name, f.fg_application,b.Quantity, f.UOM
                    FROM BOMMaster b
                    INNER JOIN FinishGoodsMaster f ON b.fg_id = f.FGID
                    WHERE b.BOM_id = {BOMID}";


            DataSet ds = gc.GetDataSet(bomMasterQuery);
            return ds;
        }

        public DataSet RawMaterialMaster(int BOMID)
        {
            string bomDetailsQuery = $@"
                    SELECT 
                           r.material_type, r.material_name, r.material_specification,d.QuantityPerUnit, r.UOM
                    FROM BOMDetails d
                    INNER JOIN RawMaterialMaster r ON d.material_id = r.material_id
                    WHERE d.BOM_id = {BOMID}";
            DataSet ds = gc.GetDataSet(bomDetailsQuery);
            return ds;
        }
      


    }
}
