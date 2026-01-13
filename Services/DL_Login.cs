using Arihant.Models.Menu;
using Azure.Core;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Arihant.Services
{
    public class DL_Login
    {
        private readonly IConfiguration _configuration;
        private readonly GraficaliClasses.GraficaliClasses gc;
        public DL_Login(GraficaliClasses.GraficaliClasses _gc, IConfiguration configuration)
        {
            gc = _gc;
            _configuration = configuration;
        }
        public string CheckIP(string email , string IP)
        {

           
            var outParam = new SqlParameter("@result", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };
            var parameters = new Dictionary<string, SqlParameter>
                    {
                         { "Operation", new SqlParameter("@Operation", "ValidateIP") },
                         { "IPAddress", new SqlParameter("@IPAddress", IP) },
                         { "UserID", new SqlParameter("@UserID", email) },
                         { "result", outParam }
                    };

            var response = gc.ExecuteStoredProcedure("SP_Login", parameters);
            string finalResult = response.OutputParameters["@result"]?.ToString();

            return finalResult;

        }

        public string CheckUserIsValid(string UserID, string Password)
        {

            string encryptionKey = _configuration["EncryptionSettings:Key"];
            string encPassword = gc.Encrypt(Password, encryptionKey);
            var outParam = new SqlParameter("@result", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };
            var parameters = new Dictionary<string, SqlParameter>
                    {
                         { "Operation", new SqlParameter("@Operation", "CheckLogIN") },
                         { "ID", new SqlParameter("@UserID", UserID) },
                         { "Passworrd", new SqlParameter("@Passworrd", encPassword) },
                         { "result", outParam }
                    };

            var response = gc.ExecuteStoredProcedure("SP_Login", parameters);
            string finalResult = response.OutputParameters["@result"]?.ToString();

            return finalResult;
        }

        public List<MenuViewModel> GetUserAccess(string UserID)
        {
      
            var parameters = new Dictionary<string, SqlParameter>
            {
               
                { "UserID", new SqlParameter("@UserID", UserID) }
     
            };

            DataSet ds = gc.ExecuteStoredProcedureGetDataSet("Sp_GetUserMenus", parameters.Values.ToArray());

            List<MenuViewModel> menuList = new List<MenuViewModel>();

            if (ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    
                    if (dr["RightAllowed"].ToString() == "Y")
                    {
                        menuList.Add(new MenuViewModel
                        {
                            Module = dr["Module"].ToString(),
                            RightName = dr["RightName"].ToString(),
                            UrlPage = dr["UrlPage"].ToString(),
                            ParentMenuID = Convert.ToInt32(dr["ParentMenuID"]),
                            MenuID = Convert.ToInt32(dr["MenuID"])
                        });
                    }
                }
            }
            return menuList;
        }

    }
}
