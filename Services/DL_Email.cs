using Microsoft.Data.SqlClient;
using System.Data;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Text;

namespace Arihant.Services
{
    public class DL_Email
    {


        private readonly GraficaliClasses.GraficaliClasses gc;
        private readonly IConfiguration _configuration;
        public DL_Email(GraficaliClasses.GraficaliClasses _gc, IConfiguration configuration)
        {
            gc = _gc;
            _configuration = configuration;
        }

        public bool SendEmail(string EmailID , string bodyContent)
        {
            try
            {
                var parameters = new Dictionary<string, SqlParameter>
                {
                    { "Operation", new SqlParameter("@Operation", "GetEmailCredential") },
                    { "Mastername", new SqlParameter("@Mastername", "SendEmail") }
                };

                DataSet ds = gc.ExecuteStoredProcedureGetDataSet("SP_Email_Integration", parameters.Values.ToArray());

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];

                    string fromEmail = dr["CMID"].ToString();
                    string password = dr["CMName"].ToString();
                    string smtpHost = dr["Param1"].ToString();
                    int port = Convert.ToInt32(dr["Param2"]);

                 

                    using (MailMessage mail = new MailMessage())
                    {
                        mail.From = new MailAddress(fromEmail, "Arihant Gold Plast");
                        mail.To.Add(EmailID);
                        mail.Subject = "Reset Password OTP";

                     
                        mail.Body = EmailFormatWithHeader(bodyContent);
                        mail.IsBodyHtml = true;

                        using (SmtpClient smtp = new SmtpClient(smtpHost, port))
                        {
                            smtp.UseDefaultCredentials = false;
                            smtp.Credentials = new NetworkCredential(fromEmail, password);
                            smtp.EnableSsl = true;

                            ServicePointManager.ServerCertificateValidationCallback =
                                delegate (object s, System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                                System.Security.Cryptography.X509Certificates.X509Chain chain,
                                SslPolicyErrors sslPolicyErrors) { return true; };

                            smtp.Send(mail);
                        }
                    }

                    
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void SaveOTP(string EmailID , string OTP)
        {
            var saveParamsnew = new Dictionary<string, SqlParameter>
                    {
                        { "Operation", new SqlParameter("@Operation", "SaveOTP") },
                        { "EmailID", new SqlParameter("@EmailID", EmailID) },
                        { "OTPCode", new SqlParameter("@OTPCode", OTP) }
                    };

            var response = gc.ExecuteStoredProcedure("SP_Email_Integration", saveParamsnew);

        }
        public int UpdateAccountPassword(string email, string password)
        {
            string encryptionKey = _configuration["EncryptionSettings:Key"];
            string EncrPassword = gc.Encrypt(password, encryptionKey);
            try
            {
                var saveParamsnew = new Dictionary<string, SqlParameter>
            {
                { "Operation", new SqlParameter("@Operation", "UpdatePassword") },
                { "EmailID", new SqlParameter("@EmailID", email) },
                { "NewPassword", new SqlParameter("@NewPassword", EncrPassword) }
            };

                var response = gc.ExecuteStoredProcedure("SP_Email_Integration", saveParamsnew);
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }

        }


        public string SetSession(string email)
        {
            var parameters = new Dictionary<string, SqlParameter>
            {
                { "Operation", new SqlParameter("@Operation", "GetUserName") },
                { "EmailID", new SqlParameter("@EmailID", email) }
              
            };

            DataSet ds = gc.ExecuteStoredProcedureGetDataSet("SP_Email_Integration", parameters.Values.ToArray());
            return ds.Tables[0].Rows[0]["UserName"].ToString();
        }

        public string VerifyOTP(string email, string otp)
        {
            var parameters = new Dictionary<string, SqlParameter>
            {
                { "Operation", new SqlParameter("@Operation", "VerifyOTP") },
                { "EmailID", new SqlParameter("@EmailID", email) },
                { "OTPCode", new SqlParameter("@OTPCode", otp) }
            };

            DataSet ds = gc.ExecuteStoredProcedureGetDataSet("SP_Email_Integration", parameters.Values.ToArray());
            return ds.Tables[0].Rows[0]["Status"].ToString();
        }
        public static string EmailFormatWithHeader(string msgBody)
        {

            StringBuilder strmsgbody = new StringBuilder();
            strmsgbody.Append("<table width='600' align='center' cellpadding='0' cellspacing='0' style='border: solid 1px #2c3a40; font-family: Arial, sans-serif;'>");
            strmsgbody.Append("<tr><td bgcolor='#f8f8f8' style='padding: 20px; border-bottom: 1px solid #eeeeee;'>");
            strmsgbody.Append("<table width='100%'><tr>");
            strmsgbody.Append("<td><img src='~/images/logo.png' width='160' alt='Arihant Gold Plast' /></td>");
            strmsgbody.Append("<td align='right' style='font-size: 12px; color: #666;'>Arihant Gold Plast Pvt</td>");
            strmsgbody.Append("</tr></table></td></tr>");
            strmsgbody.Append("<tr><td align='center' style='padding: 15px; background-color: #64b379; color: #ffffff; font-size: 18px; font-weight: bold;'>");
            strmsgbody.Append("Arihant Gold Plast Pvt. Ltd.");
            strmsgbody.Append("</td></tr>");
            strmsgbody.Append("<tr><td bgcolor='#ffffff' style='padding: 30px; line-height: 1.6;'>");
            strmsgbody.Append("<div style='font-size: 14px; color: #333333;'>");
            strmsgbody.Append(msgBody);
            strmsgbody.Append("</div></td></tr>");
            strmsgbody.Append("<tr><td bgcolor='#f1f1f1' style='padding: 20px; text-align: center; font-size: 11px; color: #888888; border-top: 1px solid #dddddd;'>");
            strmsgbody.Append("<p style='margin: 0;'>This is an automated message, please do not reply to this email.</p>");
            strmsgbody.Append("<p style='margin: 5px 0;'>&copy; " + DateTime.Now.Year + " Arihant Gold Plast Pvt. Ltd. All Rights Reserved.</p>");
            strmsgbody.Append("</td></tr>");
            strmsgbody.Append("</table>");

            return strmsgbody.ToString();
        }

    }
}
