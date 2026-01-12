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

        public string GetUserAccess(string UserID)
        {

            var outParam = new SqlParameter("@result", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };
            var parameters = new Dictionary<string, SqlParameter>
                    {
                         { "Operation", new SqlParameter("@Operation", "CheckLogIN") },
                         { "ID", new SqlParameter("@UserID", UserID) },
                        
                         { "result", outParam }
                    };

            var response = gc.ExecuteStoredProcedure("SP_GetLogInAccess", parameters);
            string finalResult = response.OutputParameters["@result"]?.ToString();

            return finalResult;
        }

    }
}
