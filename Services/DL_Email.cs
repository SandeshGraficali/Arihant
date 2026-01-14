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

        public bool SendEmail(string EmailID , string bodyContent,string Subject)
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
                        mail.Subject = Subject;

                     
                        mail.Body = EmailFormatWithHeader(Subject , bodyContent);
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


        public DataSet SetSession(string email)
        {
            var parameters = new Dictionary<string, SqlParameter>
            {
                { "Operation", new SqlParameter("@Operation", "GetUserName") },
                { "EmailID", new SqlParameter("@EmailID", email) }

            };

            DataSet ds = gc.ExecuteStoredProcedureGetDataSet("SP_Email_Integration", parameters.Values.ToArray());
            return ds;

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
        //public static string EmailFormatWithHeader(string msgBody)
        //{

        //    StringBuilder strmsgbody = new StringBuilder();
        //    strmsgbody.Append("<table width='600' align='center' cellpadding='0' cellspacing='0' style='border: solid 1px #2c3a40; font-family: Arial, sans-serif;'>");
        //    strmsgbody.Append("<tr><td bgcolor='#f8f8f8' style='padding: 20px; border-bottom: 1px solid #eeeeee;'>");
        //    strmsgbody.Append("<table width='100%'><tr>");
        //    strmsgbody.Append("<td><img src='https://graficali.co.in/Arihant/images/logo.png' style='display:block;  max-width:100px; height:auto;' alt='Arihant Gold Plast' /></td>");
        //    strmsgbody.Append("<td align='right' style='font-size: 12px; color: #666;'>Arihant Gold Plast Pvt</td>");
        //    strmsgbody.Append("</tr></table></td></tr>");
        //    strmsgbody.Append("<tr><td align='center' style='padding: 15px; background-color: #64b379; color: #ffffff; font-size: 18px; font-weight: bold;'>");
        //    strmsgbody.Append("Arihant Gold Plast Pvt. Ltd.");
        //    strmsgbody.Append("</td></tr>");
        //    strmsgbody.Append("<tr><td bgcolor='#ffffff' style='padding: 30px; line-height: 1.6;'>");
        //    strmsgbody.Append("<div style='font-size: 14px; color: #333333;'>");
        //    strmsgbody.Append(msgBody);
        //    strmsgbody.Append("</div></td></tr>");
        //    strmsgbody.Append("<tr><td bgcolor='#f1f1f1' style='padding: 20px; text-align: center; font-size: 11px; color: #888888; border-top: 1px solid #dddddd;'>");
        //    strmsgbody.Append("<p style='margin: 0;'>This is an automated message, please do not reply to this email.</p>");
        //    strmsgbody.Append("<p style='margin: 5px 0;'>&copy; " + DateTime.Now.Year + " Arihant Gold Plast Pvt. Ltd. All Rights Reserved.</p>");
        //    strmsgbody.Append("</td></tr>");
        //    strmsgbody.Append("</table>");

        //    return strmsgbody.ToString();
        //}

        public static string EmailFormatWithHeader(string emailHeader, string msgBody)
        {
            StringBuilder strmsgbody = new StringBuilder();
            strmsgbody.Append("<table width='750' align='center' cellpadding='0' cellspacing='0' style='border: solid 1px #ccc; border-bottom: #cf270c solid 4px; font-family: Arial, Helvetica, sans-serif;'>");
            strmsgbody.Append("<tr><td bgcolor='#FFFFFF'><table width='100%' align='center' cellpadding='0' cellspacing='0'>");
            strmsgbody.Append("<tr><td height='80' align='center' bgcolor='#fff'>");
            strmsgbody.Append("<table border='0' cellspacing='0' cellpadding='10'><tbody><tr>");
            strmsgbody.Append("<td valign='top'><img src='https://graficali.co.in/Arihant/images/logo.png' width='60' border='0' alt='Arihant Logo'/></td>");
            strmsgbody.Append("<td valign='middle' style='font-size:20px; color:#003970;'><strong>Arihant Gold Plast Pvt. Ltd.</strong></td>");
            strmsgbody.Append("</tr></tbody></table>");
            strmsgbody.Append("</td></tr>");

            strmsgbody.Append("<tr><td><table width='100%' border='0' align='center' cellpadding='4' cellspacing='2'>");
            strmsgbody.Append("<tr><td style='padding: 10px; font-size:18px; color:#FFFFFF; background-color:#003c76;'> &nbsp;&nbsp;" + emailHeader + "</td></tr>");
            strmsgbody.Append("<tr><td>");
            strmsgbody.Append("<table width='100%' border='0' cellpadding='15'>");
            strmsgbody.Append("<tr><td style='font-size:12px; border-bottom:#ccc solid 1px; color: #333; line-height: 1.6;'>");

            strmsgbody.Append(msgBody); 

            strmsgbody.Append("</td></tr></table>");
            strmsgbody.Append("</td></tr></table></td></tr>");
            strmsgbody.Append("</table><br /></td></tr></table>");

            return strmsgbody.ToString();
        }

    }
}
