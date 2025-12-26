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
                  
                        CompanyID = dr["CompanyID"] != DBNull.Value ? Convert.ToInt32(dr["CompanyID"]) : 0,
                        CompanyName = dr["CompanyName"].ToString(),
                        RegNo = dr["RegNo"].ToString(),
                        PANNo = dr["PANNo"].ToString(),
                        GSTNo = dr["GSTNo"].ToString(),


                        Email = dr["Email"].ToString(),
                        Website = dr["Website"].ToString(),
                        MobileNumber = dr["MobileNumber"].ToString(),
                        MobileNumber2 = dr["MobileNumber2"].ToString(),
                        MobileNumber3 = dr["MobileNumber3"].ToString(),
                        MobileNumber4 = dr["MobileNumber4"].ToString(),
                        MobileNumber5 = dr["MobileNumber5"].ToString(),

                      
                        BankName = dr["BankName"].ToString(),
                        AccountNo = dr["AccountNo"].ToString(),
                        IFSCCode = dr["IFSCCode"].ToString(),

                        Sales_Address = dr["Sales_Address"].ToString(),
                        Sales_State = dr["Sales_State"].ToString(),
                        Sales_City = dr["Sales_City"].ToString(),
                        Sales_PinCode = dr["Sales_PinCode"].ToString(),
                        Sales_Landmark = dr["Sales_Landmark"].ToString(),

                  
                        Purchase_Address = dr["Purchase_Address"].ToString(),
                        Purchase_State = dr["Purchase_State"].ToString(),
                        Purchase_City = dr["Purchase_City"].ToString(),
                        Purchase_PinCode = dr["Purchase_PinCode"].ToString(),
                        Purchase_Landmark = dr["Purchase_Landmark"].ToString(),

                    
                        Bank_Address = dr["Bank_Address"].ToString(),
                        Bank_State = dr["Bank_State"].ToString(),
                        Bank_City = dr["Bank_City"].ToString(),
                        Bank_PinCode = dr["Bank_PinCode"].ToString()
                    });
                }
            }

            return list;

        }

        public string SaveCompanyDetails(string data)
        {
            try
            {
                var model = System.Text.Json.JsonSerializer.Deserialize<CompanyViewModel>(data);

               
                var mobiles = model.MobileNumbers ?? new List<string>();
                var outParam = new SqlParameter("@result", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };
                var parameters = new Dictionary<string, SqlParameter>
                {
                    { "CompanyName", new SqlParameter("@CompanyName", model.CompanyName ?? (object)DBNull.Value) },
                    { "RegNo", new SqlParameter("@RegNo", model.RegNo ?? (object)DBNull.Value) },
                    { "GSTNo", new SqlParameter("@GSTNo", model.GSTNo ?? (object)DBNull.Value) },
                    { "PANNo", new SqlParameter("@PANNo", model.PANNo ?? (object)DBNull.Value) },
                    { "Email", new SqlParameter("@Email", model.Email ?? (object)DBNull.Value) },
                    { "Website", new SqlParameter("@Website", model.Website ?? (object)DBNull.Value) },
                    { "Telephone", new SqlParameter("@Telephone", model.Telephone ?? (object)DBNull.Value) },
            
                    
                    { "Mobile1", new SqlParameter("@Mobile1", mobiles.Count > 0 ? mobiles[0] : (object)DBNull.Value) },
                    { "Mobile2", new SqlParameter("@Mobile2", mobiles.Count > 1 ? mobiles[1] : (object)DBNull.Value) },
                    { "Mobile3", new SqlParameter("@Mobile3", mobiles.Count > 2 ? mobiles[2] : (object)DBNull.Value) },
                    { "Mobile4", new SqlParameter("@Mobile4", mobiles.Count > 3 ? mobiles[3] : (object)DBNull.Value) },
                    { "Mobile5", new SqlParameter("@Mobile5", mobiles.Count > 4 ? mobiles[4] : (object)DBNull.Value) },

                    
                    { "S_Address", new SqlParameter("@S_Address", model.SalesAddress.Address ?? (object)DBNull.Value) },
                    { "S_Landmark", new SqlParameter("@S_Landmark", model.SalesAddress.Landmark ?? (object)DBNull.Value) },
                    { "S_PinCode", new SqlParameter("@S_PinCode", model.SalesAddress.Pincode ?? (object)DBNull.Value) },
                    { "S_City", new SqlParameter("@S_City", model.SalesAddress.City ?? (object)DBNull.Value) },
                    { "S_State", new SqlParameter("@S_State", model.SalesAddress.State ?? (object)DBNull.Value) },


                    { "P_Address", new SqlParameter("@P_Address", model.PurchaseAddress.Address ?? (object)DBNull.Value) },
                    { "P_Landmark", new SqlParameter("@P_Landmark", model.PurchaseAddress.Landmark ?? (object)DBNull.Value) },
                    { "P_PinCode", new SqlParameter("@P_PinCode", model.PurchaseAddress.Pincode ?? (object)DBNull.Value) },
                    { "P_City", new SqlParameter("@P_City", model.PurchaseAddress.City ?? (object)DBNull.Value) },
                    { "P_State", new SqlParameter("@P_State", model.PurchaseAddress.State ?? (object)DBNull.Value) },

                 
                    { "BankName", new SqlParameter("@BankName", model.BankName ?? (object)DBNull.Value) },
                    { "AccountNo", new SqlParameter("@AccountNo", model.AccountNo ?? (object)DBNull.Value) },
                    { "IFSCCode", new SqlParameter("@IFSCCode", model.IFSCCode ?? (object)DBNull.Value) },
                    { "BankAddress", new SqlParameter("@BankAddress", model.BankAddress ?? (object)DBNull.Value) },
                    { "B_Landmark", new SqlParameter("@B_Landmark", model.BankLandmark ?? (object)DBNull.Value) },
                    { "B_PinCode", new SqlParameter("@B_PinCode", model.BankPincode ?? (object)DBNull.Value) },
                    { "B_City", new SqlParameter("@B_City", model.BankCity ?? (object)DBNull.Value) },
                    { "B_State", new SqlParameter("@B_State", model.BankState ?? (object)DBNull.Value) },
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


        public string UpdateCompanyDetails(string data)
        {
            try
            {
                var model = System.Text.Json.JsonSerializer.Deserialize<UpdateCompanyModel>(data);


                var mobiles = model.MobileNumbers ?? new List<string>();
                var outParam = new SqlParameter("@result", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };
                var parameters = new Dictionary<string, SqlParameter>
                {
                    { "CompanyName", new SqlParameter("@CompanyName", model.CompanyName ?? (object)DBNull.Value) },
                     { "companyID", new SqlParameter("@companyID", model.companyID ?? (object)DBNull.Value) },
                    { "RegNo", new SqlParameter("@RegNo", model.RegNo ?? (object)DBNull.Value) },
                    { "GSTNo", new SqlParameter("@GSTNo", model.GSTNo ?? (object)DBNull.Value) },
                    { "PANNo", new SqlParameter("@PANNo", model.PANNo ?? (object)DBNull.Value) },
                    { "Email", new SqlParameter("@Email", model.Email ?? (object)DBNull.Value) },
                    { "Website", new SqlParameter("@Website", model.Website ?? (object)DBNull.Value) },
                    { "Telephone", new SqlParameter("@Telephone", model.Telephone ?? (object)DBNull.Value) },


                    { "Mobile1", new SqlParameter("@Mobile1", mobiles.Count > 0 ? mobiles[0] : (object)DBNull.Value) },
                    { "Mobile2", new SqlParameter("@Mobile2", mobiles.Count > 1 ? mobiles[1] : (object)DBNull.Value) },
                    { "Mobile3", new SqlParameter("@Mobile3", mobiles.Count > 2 ? mobiles[2] : (object)DBNull.Value) },
                    { "Mobile4", new SqlParameter("@Mobile4", mobiles.Count > 3 ? mobiles[3] : (object)DBNull.Value) },
                    { "Mobile5", new SqlParameter("@Mobile5", mobiles.Count > 4 ? mobiles[4] : (object)DBNull.Value) },


                    { "S_Address", new SqlParameter("@S_Address", model.SalesAddress.Address ?? (object)DBNull.Value) },
                    { "S_Landmark", new SqlParameter("@S_Landmark", model.SalesAddress.Landmark ?? (object)DBNull.Value) },
                    { "S_PinCode", new SqlParameter("@S_PinCode", model.SalesAddress.Pincode ?? (object)DBNull.Value) },
                    { "S_City", new SqlParameter("@S_City", model.SalesAddress.City ?? (object)DBNull.Value) },
                    { "S_State", new SqlParameter("@S_State", model.SalesAddress.State ?? (object)DBNull.Value) },


                    { "P_Address", new SqlParameter("@P_Address", model.PurchaseAddress.Address ?? (object)DBNull.Value) },
                    { "P_Landmark", new SqlParameter("@P_Landmark", model.PurchaseAddress.Landmark ?? (object)DBNull.Value) },
                    { "P_PinCode", new SqlParameter("@P_PinCode", model.PurchaseAddress.Pincode ?? (object)DBNull.Value) },
                    { "P_City", new SqlParameter("@P_City", model.PurchaseAddress.City ?? (object)DBNull.Value) },
                    { "P_State", new SqlParameter("@P_State", model.PurchaseAddress.State ?? (object)DBNull.Value) },


                    { "BankName", new SqlParameter("@BankName", model.BankName ?? (object)DBNull.Value) },
                    { "AccountNo", new SqlParameter("@AccountNo", model.AccountNo ?? (object)DBNull.Value) },
                    { "IFSCCode", new SqlParameter("@IFSCCode", model.IFSCCode ?? (object)DBNull.Value) },
                    { "BankAddress", new SqlParameter("@BankAddress", model.BankAddress ?? (object)DBNull.Value) },
                    { "B_Landmark", new SqlParameter("@B_Landmark", model.BankLandmark ?? (object)DBNull.Value) },
                    { "B_PinCode", new SqlParameter("@B_PinCode", model.BankPincode ?? (object)DBNull.Value) },
                    { "B_City", new SqlParameter("@B_City", model.BankCity ?? (object)DBNull.Value) },
                    { "B_State", new SqlParameter("@B_State", model.BankState ?? (object)DBNull.Value) },
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
