using Microsoft.Data.SqlClient;
using System.Data;

namespace Arihant.Services
{
    public class DL_Login
    {

        private readonly GraficaliClasses.GraficaliClasses gc;
        public DL_Login(GraficaliClasses.GraficaliClasses _gc)
        {
            gc = _gc;
        }

            public string CheckUserIsValid(string UserID , string Password)
            {
                var outParam = new SqlParameter("@result", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };
                var parameters = new Dictionary<string, SqlParameter>
                    {
                         { "Operation", new SqlParameter("@Operation", "CheckLogIN") },
                         { "ID", new SqlParameter("@UserID", UserID) },
                         { "Passworrd", new SqlParameter("@Passworrd", Password) },
                         { "result", outParam }
                    };

                var response = gc.ExecuteStoredProcedure("SP_Login", parameters);
                string finalResult = response.OutputParameters["@result"]?.ToString();

                return finalResult;
            }
    }
}
