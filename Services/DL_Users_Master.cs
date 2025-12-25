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
