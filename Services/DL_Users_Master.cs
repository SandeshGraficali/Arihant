using Arihant.Models.Company_Master;
using Arihant.Models.IP_Master;
using Arihant.Models.Rights_Master;
using Arihant.Models.User_Master;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Reflection;

namespace Arihant.Services
{
    public class DL_Users_Master
    {

        private readonly GraficaliClasses.GraficaliClasses gc;
        public DL_Users_Master(GraficaliClasses.GraficaliClasses _gc)
        {
            gc = _gc;
        }

        public string DeactiveConpany(string ID)
        {
            var outParam = new SqlParameter("@result", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };
            var parameters = new Dictionary<string, SqlParameter>
                {
                     { "Operation", new SqlParameter("@Operation", "DeactivateComapany") },
                     { "ID", new SqlParameter("@ID", ID) },
                     { "result", outParam }
                };

            var response = gc.ExecuteStoredProcedure("SP_Update_Company", parameters);

            string finalResult = response.OutputParameters["@result"]?.ToString();

            return finalResult;
        }
        public string ActiveAndDeactiveConpany(int ID )
        {
            var outParam = new SqlParameter("@result", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };
            var parameters = new Dictionary<string, SqlParameter>
                {
                     { "Operation", new SqlParameter("@Operation", "ActiveDeactivateComapany") },
                     { "ID", new SqlParameter("@ID", ID) },
                     { "result", outParam }
                };

            var response = gc.ExecuteStoredProcedure("SP_Update_Company", parameters);

            string finalResult = response.OutputParameters["@result"]?.ToString();

            return finalResult;
        }

        public List<CompanyProfileModel> GetAllCompanyMasters()
        {
            List<CompanyProfileModel> list = new List<CompanyProfileModel>();
            var parameters = new Dictionary<string, SqlParameter>
                {
                    { "Operation", new SqlParameter("@Operation", "Get_Company_Master") }
                 
                };

            DataSet ds = gc.ExecuteStoredProcedureGetDataSet("SP_Update_Company", parameters.Values.ToArray());
            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    list.Add(new CompanyProfileModel
                    {
                        CompanyID = Convert.ToInt32(dr["CompanyID"]),
                        CompanyName = dr["CompanyName"].ToString(),
                        GSTNo = dr["GSTNo"].ToString(),
                        Central_Excise_Registration = dr["Central_Excise_Registration"].ToString(),
                        Central_Excise_Range = dr["Central_Excise_Range"].ToString(),
                        TIN_NO = dr["TIN_NO"].ToString(),
                        VAT_NO = dr["VAT_NO"].ToString(),
                        CSTNO = dr["CSTNO"].ToString(),
                        STNO = dr["STNO"].ToString(),
                        ECCNO = dr["ECCNO"].ToString(),
                        CompanyemailID = dr["CompanyemailID"].ToString(),
                        IsActive = dr["IsActive"].ToString(),

                        // Contact
                        Email = dr["Email"].ToString(),
                        Website = dr["Website"].ToString(),
                        Telephone = dr["Telephone"].ToString(),
                        ContactPersonName = dr["ContactPersonName"].ToString(),
                        OfficeMobileNO = dr["OfficeMobileNO"].ToString(),
                        WhatsAppNO = dr["WhatsAppNO"].ToString(),
                        AlternetEmail = dr["AlternetEmail"].ToString(),
                        MobileNumber = dr["MobileNumber"].ToString(),
                        MobileNumber2 = dr["MobileNumber2"].ToString(),

                        // Bank
                        BankName = dr["BankName"].ToString(),
                        AccountNo = dr["AccountNo"].ToString(),
                        AccountHolderName = dr["AccountHolderName"].ToString(),
                        AccountType = dr["AccountType"].ToString(),
                        IFSCCode = dr["IFSCCode"].ToString(),

                        // Company Address Mapping
                        Company_Address1 = dr["Company_Address1"].ToString(),
                        Company_Address2 = dr["Company_Address2"].ToString(),
                        Company_Landmark = dr["Company_Landmark"].ToString(),
                        Company_PinCode = dr["Company_PinCode"].ToString(),
                        Company_City = dr["Company_City"].ToString(),
                        Company_State = dr["Company_State"].ToString(),
                        Company_Country = dr["Company_Country"].ToString(),

                        // CE Address Mapping
                        CE_Address1 = dr["CE_Address1"].ToString(),
                        CE_Address2 = dr["CE_Address2"].ToString(),
                        CE_Landmark = dr["CE_Landmark"].ToString(),
                        CE_PinCode = dr["CE_PinCode"].ToString(),
                        CE_City = dr["CE_City"].ToString(),
                        CE_State = dr["CE_State"].ToString(),
                        CE_Country = dr["CE_Country"].ToString(),

                        // Division Address Mapping
                        Div_Address1 = dr["Div_Address1"].ToString(),
                        Div_Address2 = dr["Div_Address2"].ToString(),
                        Div_Landmark = dr["Div_Landmark"].ToString(),
                        Div_PinCode = dr["Div_PinCode"].ToString(),
                        Div_City = dr["Div_City"].ToString(),
                        Div_State = dr["Div_State"].ToString(),
                        Div_Country = dr["Div_Country"].ToString(),

                        // Commissionerate Mapping
                        Comm_Address1 = dr["Comm_Address1"].ToString(),
                        Comm_Address2 = dr["Comm_Address2"].ToString(),
                        Comm_Landmark = dr["Comm_Landmark"].ToString(),
                        Comm_PinCode = dr["Comm_PinCode"].ToString(),
                        Comm_City = dr["Comm_City"].ToString(),
                        Comm_State = dr["Comm_State"].ToString(),
                        Comm_Country = dr["Comm_Country"].ToString(),

                        // Bank Branch Mapping
                        Bank_Address1 = dr["Bank_Address1"].ToString(),
                        Bank_Address2 = dr["Bank_Address2"].ToString(),
                        Bank_Landmark = dr["Bank_Landmark"].ToString(),
                        Bank_PinCode = dr["Bank_PinCode"].ToString(),
                        Bank_City = dr["Bank_City"].ToString(),
                        Bank_State = dr["Bank_State"].ToString(),
                        Bank_Country = dr["Bank_Country"].ToString()
                    });
                }
            }
                return list;

        }

        public string SaveCompanyDetails(string data , string CreatedBy)
        {
            try
            {
                var model = System.Text.Json.JsonSerializer.Deserialize<CompanyViewModel>(data);

               
                var mobiles = model.MobileNumbers ?? new List<string>();
                var outParam = new SqlParameter("@result", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };
                var parameters = new Dictionary<string, SqlParameter>
        {
   
            { "CompanyName", new SqlParameter("@CompanyName", model.CompanyName ?? (object)DBNull.Value) },
            { "CEegistration", new SqlParameter("@CEegistration", model.CEegistration ?? (object)DBNull.Value) },
            { "CERange", new SqlParameter("@CERange", model.CERange ?? (object)DBNull.Value) },
            { "TINNO", new SqlParameter("@TINNO", model.TINNO ?? (object)DBNull.Value) },
            { "VATNO", new SqlParameter("@VATNO", model.VATNO ?? (object)DBNull.Value) },
            { "CSTNO", new SqlParameter("@CSTNO", model.CSTNO ?? (object)DBNull.Value) },
            { "STNO", new SqlParameter("@STNO", model.STNO ?? (object)DBNull.Value) },
            { "ECCNO", new SqlParameter("@ECCNO", model.ECCNO ?? (object)DBNull.Value) },
            { "GSTNo", new SqlParameter("@GSTNo", model.GSTNo ?? (object)DBNull.Value) },
            
            // --- Contact & Professional Info ---
            { "Email", new SqlParameter("@Email", model.Email ?? (object)DBNull.Value) },
            { "AlternateEmail", new SqlParameter("@AlternateEmail", model.AlternateEmail ?? (object)DBNull.Value) },
            { "ContactPersonName", new SqlParameter("@ContactPersonName", model.ContactPersonName ?? (object)DBNull.Value) },
            { "OffMobileNO", new SqlParameter("@OffMobileNO", model.OffMobileNO ?? (object)DBNull.Value) },
            { "WhatsappNo", new SqlParameter("@WhatsappNo", model.WhatsappNo ?? (object)DBNull.Value) },
            { "Website", new SqlParameter("@Website", model.Website ?? (object)DBNull.Value) },
            { "Telephone", new SqlParameter("@Telephone", model.Telephone ?? (object)DBNull.Value) },

            // --- Mobile Numbers (List Mapping) ---
            { "Mobile1", new SqlParameter("@Mobile1", mobiles.Count > 0 ? mobiles[0] : (object)DBNull.Value) },
            { "Mobile2", new SqlParameter("@Mobile2", mobiles.Count > 1 ? mobiles[1] : (object)DBNull.Value) },
            { "Mobile3", new SqlParameter("@Mobile3", mobiles.Count > 2 ? mobiles[2] : (object)DBNull.Value) },
              { "Mobile4", new SqlParameter("@Mobile4", mobiles.Count > 2 ? mobiles[2] : (object)DBNull.Value) },
                { "Mobile5", new SqlParameter("@Mobile5", mobiles.Count > 2 ? mobiles[2] : (object)DBNull.Value) },

            // --- Company Registered Address ---
            { "Comp_Addr1", new SqlParameter("@Comp_Addr1", model.CompanyAddr?.AddressLine1 ?? (object)DBNull.Value) },
            { "Comp_Addr2", new SqlParameter("@Comp_Addr2", model.CompanyAddr?.AddressLine2 ?? (object)DBNull.Value) },
            { "Comp_Landmark", new SqlParameter("@Comp_Landmark", model.CompanyAddr?.Landmark ?? (object)DBNull.Value) },
            { "Comp_Pin", new SqlParameter("@Comp_Pin", model.CompanyAddr?.Pincode ?? (object)DBNull.Value) },
            { "Comp_City", new SqlParameter("@Comp_City", model.CompanyAddr?.City ?? (object)DBNull.Value) },
            { "Comp_State", new SqlParameter("@Comp_State", model.CompanyAddr?.State ?? (object)DBNull.Value) },
            { "Comp_Country", new SqlParameter("@Comp_Country", model.CompanyAddr?.Country ?? (object)DBNull.Value) },

            { "CE_Addr1", new SqlParameter("@CE_Addr1", model.Central_ExciseAddr?.AddressLine1 ?? (object)DBNull.Value) },
            { "CE_Addr2", new SqlParameter("@CE_Addr2", model.Central_ExciseAddr?.AddressLine2 ?? (object)DBNull.Value) },
            { "CE_Landmark", new SqlParameter("@CE_Landmark", model.Central_ExciseAddr?.Landmark ?? (object)DBNull.Value) },
            { "CE_Pin", new SqlParameter("@CE_Pin", model.Central_ExciseAddr?.Pincode ?? (object)DBNull.Value) },
            { "CE_City", new SqlParameter("@CE_City", model.Central_ExciseAddr?.City ?? (object)DBNull.Value) },
            { "CE_State", new SqlParameter("@CE_State", model.Central_ExciseAddr?.State ?? (object)DBNull.Value) },
            { "CE_Country", new SqlParameter("@CE_Country", model.Central_ExciseAddr?.Country ?? (object)DBNull.Value) },

            // --- Division Address ---
            { "Div_Addr1", new SqlParameter("@Div_Addr1", model.DivisionAddr?.AddressLine1 ?? (object)DBNull.Value) },
            { "Div_Addr2", new SqlParameter("@Div_Addr2", model.DivisionAddr?.AddressLine2 ?? (object)DBNull.Value) },
            { "Div_Landmark", new SqlParameter("@Div_Landmark", model.DivisionAddr?.Landmark ?? (object)DBNull.Value) },
            { "Div_Pin", new SqlParameter("@Div_Pin", model.DivisionAddr?.Pincode ?? (object)DBNull.Value) },
            { "Div_City", new SqlParameter("@Div_City", model.DivisionAddr?.City ?? (object)DBNull.Value) },
            { "Div_State", new SqlParameter("@Div_State", model.DivisionAddr?.State ?? (object)DBNull.Value) },
            { "Div_Country", new SqlParameter("@Div_Country", model.DivisionAddr?.Country ?? (object)DBNull.Value) },

            // --- Commissionerate Address ---
            { "Comm_Addr1", new SqlParameter("@Comm_Addr1", model.CommissionerateAddr?.AddressLine1 ?? (object)DBNull.Value) },
            { "Comm_Addr2", new SqlParameter("@Comm_Addr2", model.CommissionerateAddr?.AddressLine2 ?? (object)DBNull.Value) },
            { "Comm_Landmark", new SqlParameter("@Comm_Landmark", model.CommissionerateAddr?.Landmark ?? (object)DBNull.Value) },
            { "Comm_Pin", new SqlParameter("@Comm_Pin", model.CommissionerateAddr?.Pincode ?? (object)DBNull.Value) },
            { "Comm_City", new SqlParameter("@Comm_City", model.CommissionerateAddr?.City ?? (object)DBNull.Value) },
            { "Comm_State", new SqlParameter("@Comm_State", model.CommissionerateAddr?.State ?? (object)DBNull.Value) },
            { "Comm_Country", new SqlParameter("@Comm_Country", model.CommissionerateAddr?.Country ?? (object)DBNull.Value) },

            // --- Bank Details (Fixed Mappings) ---
            { "BankName", new SqlParameter("@BankName", model.BankName ?? (object)DBNull.Value) },
            { "AccountNo", new SqlParameter("@AccountNo", model.AccountNo ?? (object)DBNull.Value) },
            { "AccountHolderName", new SqlParameter("@AccountHolderName", model.AccountHolderName ?? (object)DBNull.Value) },
            { "AccountType", new SqlParameter("@AccountType", model.AccountType ?? (object)DBNull.Value) },
            { "IFSCCode", new SqlParameter("@IFSCCode", model.IFSCCode ?? (object)DBNull.Value) },
            { "BankAddress", new SqlParameter("@BankAddress", model.BankAddressLine1 ?? (object)DBNull.Value) },
            { "B_Landmark", new SqlParameter("@B_Landmark", model.BankLandmark ?? (object)DBNull.Value) },
            { "B_PinCode", new SqlParameter("@B_PinCode", model.BankPincode ?? (object)DBNull.Value) },
            { "B_City", new SqlParameter("@B_City", model.BankCity ?? (object)DBNull.Value) },
            { "B_State", new SqlParameter("@B_State", model.BankState ?? (object)DBNull.Value) },
            { "B_Country", new SqlParameter("@B_Country", model.BankCountry ?? (object)DBNull.Value) },
            
            // --- Metadata ---
            { "CreatedBy", new SqlParameter("@CreatedBy", CreatedBy) },

            { "result", outParam }
        };
                var response = gc.ExecuteStoredProcedure("sp_SaveCompanyProfile", parameters);
               
                string finalResult = response.OutputParameters["@result"]?.ToString();

                return finalResult;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


        public string UpdateCompanyDetails(string data , string Modifiedby)
        {
            try
            {
                var model = System.Text.Json.JsonSerializer.Deserialize<CompanyViewModel>(data);


                var mobiles = model.MobileNumbers ?? new List<string>();
                var outParam = new SqlParameter("@result", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };
                var parameters = new Dictionary<string, SqlParameter>
        {
            { "companyID", new SqlParameter("@companyID", model.CompanyID) },
            { "CompanyName", new SqlParameter("@CompanyName", model.CompanyName ?? (object)DBNull.Value) },
            { "CEegistration", new SqlParameter("@CEegistration", model.CEegistration ?? (object)DBNull.Value) },
            { "CERange", new SqlParameter("@CERange", model.CERange ?? (object)DBNull.Value) },
            { "TINNO", new SqlParameter("@TINNO", model.TINNO ?? (object)DBNull.Value) },
            { "VATNO", new SqlParameter("@VATNO", model.VATNO ?? (object)DBNull.Value) },
            { "CSTNO", new SqlParameter("@CSTNO", model.CSTNO ?? (object)DBNull.Value) },
            { "STNO", new SqlParameter("@STNO", model.STNO ?? (object)DBNull.Value) },
            { "ECCNO", new SqlParameter("@ECCNO", model.ECCNO ?? (object)DBNull.Value) },
            { "GSTNo", new SqlParameter("@GSTNo", model.GSTNo ?? (object)DBNull.Value) },
            { "CompanyemailID", new SqlParameter("@CompanyemailID", model.CompanyemailID ?? (object)DBNull.Value) },

            // Contact Details
            { "Email", new SqlParameter("@Email", model.Email ?? (object)DBNull.Value) },
            { "AlternateEmail", new SqlParameter("@AlternateEmail", model.AlternateEmail ?? (object)DBNull.Value) },
            { "ContactPersonName", new SqlParameter("@ContactPersonName", model.ContactPersonName ?? (object)DBNull.Value) },
            { "OffMobileNO", new SqlParameter("@OffMobileNO", model.OffMobileNO ?? (object)DBNull.Value) },
            { "WhatsappNo", new SqlParameter("@WhatsappNo", model.WhatsappNo ?? (object)DBNull.Value) },
            { "Website", new SqlParameter("@Website", model.Website ?? (object)DBNull.Value) },
            { "Telephone", new SqlParameter("@Telephone", model.Telephone ?? (object)DBNull.Value) },

            // Mobiles
            { "Mobile1", new SqlParameter("@Mobile1", mobiles.Count > 0 ? mobiles[0] : (object)DBNull.Value) },
            { "Mobile2", new SqlParameter("@Mobile2", mobiles.Count > 1 ? mobiles[1] : (object)DBNull.Value) },
            { "Mobile3", new SqlParameter("@Mobile3", mobiles.Count > 2 ? mobiles[2] : (object)DBNull.Value) },
            { "Mobile4", new SqlParameter("@Mobile4", mobiles.Count > 3 ? mobiles[3] : (object)DBNull.Value) },
            { "Mobile5", new SqlParameter("@Mobile5", mobiles.Count > 4 ? mobiles[4] : (object)DBNull.Value) },

            // Addresses
            { "Comp_Addr1", new SqlParameter("@Comp_Addr1", model.CompanyAddr?.AddressLine1 ?? (object)DBNull.Value) },
            { "Comp_Addr2", new SqlParameter("@Comp_Addr2", model.CompanyAddr?.AddressLine2 ?? (object)DBNull.Value) },
            { "Comp_Landmark", new SqlParameter("@Comp_Landmark", model.CompanyAddr?.Landmark ?? (object)DBNull.Value) },
            { "Comp_Pin", new SqlParameter("@Comp_Pin", model.CompanyAddr?.Pincode ?? (object)DBNull.Value) },
            { "Comp_City", new SqlParameter("@Comp_City", model.CompanyAddr?.City ?? (object)DBNull.Value) },
            { "Comp_State", new SqlParameter("@Comp_State", model.CompanyAddr?.State ?? (object)DBNull.Value) },
             { "Comp_Contry", new SqlParameter("@Comp_Contry", model.CompanyAddr.Country ?? (object)DBNull.Value) },

            { "CE_Addr1", new SqlParameter("@CE_Addr1", model.Central_ExciseAddr?.AddressLine1 ?? (object)DBNull.Value) },
            { "CE_Addr2", new SqlParameter("@CE_Addr2", model.Central_ExciseAddr?.AddressLine2 ?? (object)DBNull.Value) },
            { "CE_Landmark", new SqlParameter("@CE_Landmark", model.Central_ExciseAddr?.Landmark ?? (object)DBNull.Value) },
            { "CE_Pin", new SqlParameter("@CE_Pin", model.Central_ExciseAddr?.Pincode ?? (object)DBNull.Value) },
            { "CE_City", new SqlParameter("@CE_City", model.Central_ExciseAddr?.City ?? (object)DBNull.Value) },
            { "CE_State", new SqlParameter("@CE_State", model.Central_ExciseAddr?.State ?? (object)DBNull.Value) },
             { "CE_Contry", new SqlParameter("@CE_Contry", model.Central_ExciseAddr?.Country ?? (object)DBNull.Value) },

            { "Div_Addr1", new SqlParameter("@Div_Addr1", model.DivisionAddr?.AddressLine1 ?? (object)DBNull.Value) },
            { "Div_Addr2", new SqlParameter("@Div_Addr2", model.DivisionAddr?.AddressLine2 ?? (object)DBNull.Value) },
            { "Div_Landmark", new SqlParameter("@Div_Landmark", model.DivisionAddr?.Landmark ?? (object)DBNull.Value) },
            { "Div_Pin", new SqlParameter("@Div_Pin", model.DivisionAddr?.Pincode ?? (object)DBNull.Value) },
            { "Div_City", new SqlParameter("@Div_City", model.DivisionAddr?.City ?? (object)DBNull.Value) },
            { "Div_State", new SqlParameter("@Div_State", model.DivisionAddr?.State ?? (object)DBNull.Value) },
            { "Div_Contry", new SqlParameter("@Div_Contry", model.DivisionAddr?.Country ?? (object)DBNull.Value) },

            { "Comm_Addr1", new SqlParameter("@Comm_Addr1", model.CommissionerateAddr?.AddressLine1 ?? (object)DBNull.Value) },
            { "Comm_Addr2", new SqlParameter("@Comm_Addr2", model.CommissionerateAddr?.AddressLine2 ?? (object)DBNull.Value) },
            { "Comm_Landmark", new SqlParameter("@Comm_Landmark", model.CommissionerateAddr?.Landmark ?? (object)DBNull.Value) },
            { "Comm_Pin", new SqlParameter("@Comm_Pin", model.CommissionerateAddr?.Pincode ?? (object)DBNull.Value) },
            { "Comm_City", new SqlParameter("@Comm_City", model.CommissionerateAddr?.City ?? (object)DBNull.Value) },
            { "Comm_State", new SqlParameter("@Comm_State", model.CommissionerateAddr?.State ?? (object)DBNull.Value) },
             { "Comm_Contry", new SqlParameter("@Comm_Contry", model.CommissionerateAddr?.Country ?? (object)DBNull.Value) },

            // Bank
            { "BankName", new SqlParameter("@BankName", model.BankName ?? (object)DBNull.Value) },
            { "AccountNo", new SqlParameter("@AccountNo", model.AccountNo ?? (object)DBNull.Value) },
            { "AccountType", new SqlParameter("@AccountType", model.AccountType ?? (object)DBNull.Value) },
            { "AccountHolderName", new SqlParameter("@AccountHolderName", model.AccountHolderName ?? (object)DBNull.Value) },
            { "IFSCCode", new SqlParameter("@IFSCCode", model.IFSCCode ?? (object)DBNull.Value) },
            { "BankAddress", new SqlParameter("@BankAddress", model.BankAddressLine1 ?? (object)DBNull.Value) },
            { "BankAddress2", new SqlParameter("@BankAddress2", model.BankAddressLine2 ?? (object)DBNull.Value) },
            { "B_Landmark", new SqlParameter("@B_Landmark", model.BankLandmark ?? (object)DBNull.Value) },
            { "B_PinCode", new SqlParameter("@B_PinCode", model.BankPincode ?? (object)DBNull.Value) },
            { "B_City", new SqlParameter("@B_City", model.BankCity ?? (object)DBNull.Value) },
            { "B_State", new SqlParameter("@B_State", model.BankState ?? (object)DBNull.Value) },
            { "B_Contry", new SqlParameter("@B_Contry", model.BankCountry ?? (object)DBNull.Value) },
             { "ModifiedBy", new SqlParameter("@ModifiedBy", Modifiedby ?? (object)DBNull.Value) },

            { "result", outParam }
        };
                var response = gc.ExecuteStoredProcedure("sp_Update_CompanyProfile", parameters);

                string finalResult = response.OutputParameters["@result"]?.ToString();

                return finalResult;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


        public DataSet GetC_MasterCityList(string ID)
        {
              var parameters = new Dictionary<string, SqlParameter>
                {
                    { "Operation", new SqlParameter("@Operation", "Get_C_MastersCity") },
                     { "ID", new SqlParameter("@ID", ID) }
                };

            DataSet ds = gc.ExecuteStoredProcedureGetDataSet("sp_Get_C_Master", parameters.Values.ToArray());
            return ds;
        }
        public DataSet GetC_MasterStateList(string ID)
        {
            var parameters = new Dictionary<string, SqlParameter>
                {
                    { "Operation", new SqlParameter("@Operation", "Get_C_MastersState") },
                     { "ID", new SqlParameter("@ID", ID) }
                };

            DataSet ds = gc.ExecuteStoredProcedureGetDataSet("sp_Get_C_Master", parameters.Values.ToArray());
            return ds;
        }


        public List<C_Master> GetC_MasterList()
        {
            List<C_Master> list = new List<C_Master>();
                var parameters = new Dictionary<string, SqlParameter>
                {
                    { "Operation", new SqlParameter("@Operation", "Get_C_Masters") }
                };

                 DataSet ds = gc.ExecuteStoredProcedureGetDataSet("sp_Get_C_Master", parameters.Values.ToArray());
                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];

                    foreach (DataRow row in dt.Rows)
                    {
                        C_Master obj = new C_Master
                        {
                            ID = row["ID"].ToString(),
                            Name = row["Name"].ToString()
                        };

                        list.Add(obj);
                    }
                }
            return list;
        }
        public string UpdateRoleWithRights(string roleName, string menuIDs, string createdBy, string roleID)
        {
            try
            {
                var outParam = new SqlParameter("@result", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };
                var parameters = new Dictionary<string, SqlParameter>
                {
                    { "Operation", new SqlParameter("@Operation", "Update_Role") },
                    { "RoleName", new SqlParameter("@RoleName", roleName) },
                    { "RoleID", new SqlParameter("@RoleID", roleID) },
                    { "MenuIDs", new SqlParameter("@MenuIDs", menuIDs) },
                    { "CreatedBy", new SqlParameter("@CreatedBy", createdBy) },
                    { "result", outParam }
                };

                var response = gc.ExecuteStoredProcedure("sp_Rights_Master", parameters);
                var finalResult = response.OutputParameters["@result"]?.ToString();
                return string.IsNullOrEmpty(finalResult) ? "Error: No response from database logic." : finalResult;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string InsertRoleWithRights(string roleName, string menuIDs, string createdBy)
        {
            try
            {
                var outParam = new SqlParameter("@result", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };
                var parameters = new Dictionary<string, SqlParameter>
                {
                    { "Operation", new SqlParameter("@Operation", "Save_Role") },
                    { "RoleName", new SqlParameter("@RoleName", roleName) },
                    { "MenuIDs", new SqlParameter("@MenuIDs", menuIDs) },
                    { "CreatedBy", new SqlParameter("@CreatedBy", createdBy) },
                    { "result", outParam }
                };

                var response = gc.ExecuteStoredProcedure("sp_Rights_Master", parameters);
                return response.OutputParameters["@result"]?.ToString() ?? "0";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public List<MenuRightViewModel> GetMenuRights()
        {
            List<MenuRightViewModel> rightsList = new List<MenuRightViewModel>();
            try
            {
                var parameters = new Dictionary<string, SqlParameter>
                {
                    { "Operation", new SqlParameter("@Operation", "Get_All_Rights") }
                };

                DataSet ds = gc.ExecuteStoredProcedureGetDataSet("sp_Rights_Master", parameters.Values.ToArray());

                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        rightsList.Add(new MenuRightViewModel
                        {
                            MenuID = Convert.ToInt32(dr["MenuID"]),
                            Module = dr["Module"].ToString(),
                            RightName = dr["RightID"].ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return rightsList;
        }


        public List<DTO_MenuRight> ManageRights()
        {
            List<DTO_MenuRight> rightsList = new List<DTO_MenuRight>();
            try
            {
                var parameters = new Dictionary<string, SqlParameter>
                {
                    { "Operation", new SqlParameter("@Operation", "Manage_Rights") }
                };

                DataSet ds = gc.ExecuteStoredProcedureGetDataSet("sp_Rights_Master", parameters.Values.ToArray());

                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        rightsList.Add(new DTO_MenuRight
                        {
                            RoleID = Convert.ToInt32(dr["RoleID"]),
                            RoleName = dr["RoleName"].ToString(),
                            MenuID = Convert.ToInt32(dr["MenuID"]),
                            RightName = dr["RightID"].ToString(),
                            Module = dr["Module"].ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return rightsList;
        }

        public string AddIPAddress(IP_Master_Model model, string ModifiedBy)
        {
            try
            {
                var outParam = new SqlParameter("@result", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };
                var parameters = new Dictionary<string, SqlParameter>
                {
                    { "ID", new SqlParameter("@ID", model.ID) },
                    { "IP_Name", new SqlParameter("@IP_Name", model.IP_Name) },
                    { "IPAdd", new SqlParameter("@IPAdd", model.IPAdd) },
                    { "ModifiedBy", new SqlParameter("@ModifiedBy", ModifiedBy) },
                    { "Operation", new SqlParameter("@Operation", "ADD_IP_ADD") },
                    { "result", outParam }
                };

                var response = gc.ExecuteStoredProcedure("sp_IP_Master", parameters);
                string result = response.OutputParameters["@result"]?.ToString();
                return result;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string UpdateIPAddress(IP_Master_Model model, string ModifiedBy)
        {
            try
            {
                var outParam = new SqlParameter("@result", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };
                var parameters = new Dictionary<string, SqlParameter>
                {
                    { "ID", new SqlParameter("@ID", model.ID) },
                    { "IP_Name", new SqlParameter("@IP_Name", model.IP_Name) },
                    { "IPAdd", new SqlParameter("@IPAdd", model.IPAdd) },
                    { "ModifiedBy", new SqlParameter("@ModifiedBy", ModifiedBy) },
                    { "Operation", new SqlParameter("@Operation", "Update_IP_ADD") },
                    { "result", outParam }
                };

                var response = gc.ExecuteStoredProcedure("sp_IP_Master", parameters);
                string result = response.OutputParameters["@result"]?.ToString();
                return result;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string DeleteUser(string UserID)
        {
            try
            {
                var outParam = new SqlParameter("@result", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };
                var parameters = new Dictionary<string, SqlParameter>
                {
                    { "UserID", new SqlParameter("@UserID", UserID) },
                    { "Operation", new SqlParameter("@Operation", "Delete_User") },
                    { "result", outParam }
                };

                var response = gc.ExecuteStoredProcedure("sp_User_Master", parameters);
                string result = response.OutputParameters["@result"]?.ToString();
                return result;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


        public string ActiveUser(string UserID)
        {
            try
            {
                var outParam = new SqlParameter("@result", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };
                var parameters = new Dictionary<string, SqlParameter>
                {
                    { "ID", new SqlParameter("@ID", UserID) },
                    { "Operation", new SqlParameter("@Operation", "Active_Users") },
                    { "result", outParam }
                };

                var response = gc.ExecuteStoredProcedure("sp_IP_Master", parameters);
                string result = response.OutputParameters["@result"]?.ToString();
                return result;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


        public string DeactiveRole(string RoleID)
        {
            try
            {
                var outParam = new SqlParameter("@result", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };
                var parameters = new Dictionary<string, SqlParameter>
                {
                    { "@RoleID", new SqlParameter("@RoleID", RoleID) },
                    { "Operation", new SqlParameter("@Operation", "Deactive_Role") },
                    { "result", outParam }
                };

                var response = gc.ExecuteStoredProcedure("sp_Rights_Master", parameters);
                string result = response.OutputParameters["@result"]?.ToString();
                return result;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }



        public string DeleteIP_ADD(string UserID)
        {
            try
            {
                var outParam = new SqlParameter("@result", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };
                var parameters = new Dictionary<string, SqlParameter>
                {
                    { "ID", new SqlParameter("@ID", UserID) },
                    { "Operation", new SqlParameter("@Operation", "Delete_IP") },
                    { "result", outParam }
                };

                var response = gc.ExecuteStoredProcedure("sp_IP_Master", parameters);
                string result = response.OutputParameters["@result"]?.ToString();
                return result;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }



        public List<IP_Master_Model> GetIPMasterList()
        {
            List<IP_Master_Model> ipList = new List<IP_Master_Model>();
            try
            {
                var parameters = new Dictionary<string, SqlParameter>
                    {
                        { "Operation", new SqlParameter("@Operation", "Get_IP_Master") }

                    };

                DataSet ds = gc.ExecuteStoredProcedureGetDataSet("sp_IP_Master", parameters.Values.ToArray());

                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        ipList.Add(new IP_Master_Model
                        {
                            ID = dr["IPAddrID"].ToString(),
                            IPAdd = dr["IPAddress"].ToString(),
                            IP_Name = dr["IP_Name"].ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return ipList;
        }

        public List<IPViewModel> GetIPList()
        {
            List<IPViewModel> ipList = new List<IPViewModel>();
            try
            {
                var parameters = new Dictionary<string, SqlParameter>
                    {
                        { "Operation", new SqlParameter("@Operation", "Get_IP_List") }

                    };

                DataSet ds = gc.ExecuteStoredProcedureGetDataSet("sp_User_Master", parameters.Values.ToArray());

                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        ipList.Add(new IPViewModel
                        {
                            IPAddrID = dr["IPAddrID"].ToString(),
                            IPAddress = dr["IPAddress"].ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return ipList;
        }
        public List<IPViewModel_withIPS> GetAllUsers()
        {
            List<IPViewModel_withIPS> users = new List<IPViewModel_withIPS>();
            try
            {
                var parameters = new Dictionary<string, SqlParameter>
                    {
                        { "Operation", new SqlParameter("@Operation", "Get_All_Users") }
                    };

                DataSet ds = gc.ExecuteStoredProcedureGetDataSet("sp_User_Master", parameters.Values.ToArray());

                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        users.Add(new IPViewModel_withIPS
                        {
                            UserID = dr["UserID"]?.ToString(),
                            UserName = dr["UserName"]?.ToString(),
                            ContactNo = dr["ContactNo"]?.ToString(),
                            EmailID = dr["EmailID"]?.ToString(),
                            AssignedIP = dr["IPAddress"]?.ToString(),
                            AssignedIPs = dr["AssignedIPs"]?.ToString(),
                            IsActive = dr["IsActive"]?.ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return users;
        }


        public string UpdateUser(UserUpdateModel user, string CreatedBy)
        {
            string isIPAssigned = (user.IPAddrID != null && user.IPAddrID.Count > 0) ? "Y" : "N";
            string message = "0";

            try
            {
                var parameters = new Dictionary<string, SqlParameter>
                    {
                        { "UserName", new SqlParameter("@UserName", user.UserName ?? (object)DBNull.Value) },
                        { "UserID", new SqlParameter("@UserID", user.UserID ?? (object)DBNull.Value) },
                        { "ContactNo", new SqlParameter("@ContactNo", user.ContactNo ?? (object)DBNull.Value) },
                        { "AssignedIP", new SqlParameter("@AssignedIP", isIPAssigned) },
                        { "Email", new SqlParameter("@Email", user.EmailID ?? (object)DBNull.Value) },
                        { "Operation", new SqlParameter("@Operation", "Update_User") }
                    };

                var response = gc.ExecuteStoredProcedure("sp_User_Master", parameters);
                foreach (var ipid in user.IPAddrID)
                {
                            var ipParams = new Dictionary<string, SqlParameter>
                            {
                                { "UserID", new SqlParameter("@UserID", user.UserID) },
                                { "IPID", new SqlParameter("@IPID", ipid) },
                                { "Operation", new SqlParameter("@Operation", "Update_Assign_IP") },
                                { "CreatedBy", new SqlParameter("@CreatedBy", CreatedBy) },
                                { "result", new SqlParameter("@result", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output } }
                            };
                            gc.ExecuteStoredProcedure("sp_User_Master", ipParams);
                }
                return "1";

            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        public string AddUser(UserViewModel user, string CreatedBy)
        {
            string isIPAssigned = (user.AssignedIP != null && user.AssignedIP.Count > 0) ? "Y" : "N";
            string message = "0";
            try
            {
                var outParam = new SqlParameter("@result", SqlDbType.NVarChar, 250)
                {
                    Direction = ParameterDirection.Output
                };

                var parameters = new Dictionary<string, SqlParameter>
                {
                    { "UserName", new SqlParameter("@UserName", user.UserName ?? (object)DBNull.Value) },
                    { "UserID", new SqlParameter("@UserID", user.UserID ?? (object)DBNull.Value) },
                    { "ContactNo", new SqlParameter("@ContactNo", user.ContactNo ?? (object)DBNull.Value) },
                    { "AssignedIP", new SqlParameter("@AssignedIP", isIPAssigned?? (object)DBNull.Value) },
                    { "Password", new SqlParameter("@Password", user.Password ?? (object)DBNull.Value) },
                    { "Email", new SqlParameter("@Email", user.EmailID ?? (object)DBNull.Value) },
                    { "Operation", new SqlParameter("@Operation", "Add_User") },
                    { "result", outParam }
                };

                var response = gc.ExecuteStoredProcedure("sp_User_Master", parameters);
                if (response.OutputParameters.ContainsKey("@result"))
                {
                    message = response.OutputParameters["@result"]?.ToString();

                    if (message != "0")
                    {
                        if (user.AssignedIP != null && user.AssignedIP.Count > 0)
                        {
                            foreach (var ipid in user.AssignedIP)
                            {
                                var ipParams = new Dictionary<string, SqlParameter>
                                {
                                    { "UserID", new SqlParameter("@UserID", message) },
                                      { "CreatedBy", new SqlParameter("@CreatedBy", CreatedBy) },
                                    { "IPID", new SqlParameter("@IPID", ipid) },
                                    { "Operation", new SqlParameter("@Operation", "Assign_IP") },
                                    { "result", new SqlParameter("@result", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output } }
                                };
                                gc.ExecuteStoredProcedure("sp_User_Master", ipParams);
                            }
                        }
                        return "1";
                    }
                    else
                    {
                        return "0";
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return message;
        }
    }
}
