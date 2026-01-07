using Arihant.Models.Client;
using Arihant.Models.Company_Master;
using Arihant.Models.IP_Master;
using Arihant.Models.Rights_Master;
using Arihant.Models.User_Master;
using Azure;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Globalization;
using System.Reflection;
using System.Xml.Linq;
using static System.Net.WebRequestMethods;

namespace Arihant.Services
{
    public class DL_Users_Master
    {

        private readonly GraficaliClasses.GraficaliClasses gc;
        private readonly DL_Email _email;
        private readonly IConfiguration _configuration;
        public DL_Users_Master(GraficaliClasses.GraficaliClasses _gc, DL_Email user, IConfiguration configuration)
        {
            gc = _gc;
            _email = user;
            _configuration = configuration;
        }

        public ClientSubmissionViewModel GetClientById(int clientId)
        {
            ClientSubmissionViewModel model = new ClientSubmissionViewModel();

            var parameters = new Dictionary<string, SqlParameter>
            {
                { "Operation", new SqlParameter("@Operation", "Get_Client_By_ID") },
                { "ClientID", new SqlParameter("@ClientID", clientId) }
            };

            DataSet ds = gc.ExecuteStoredProcedureGetDataSet("SP_Update_Client", parameters.Values.ToArray());

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
   
                DataRow dr = ds.Tables[0].Rows[0];
                model.ClientID = Convert.ToInt32(dr["ClientID"]);
                model.ClientDetails = new ClientDetailModel
                {
                    ClientName = dr["ClientName"].ToString(),
                    ClientType = dr["ClientKey"].ToString(),
                    PANNo = dr["PANNo"].ToString(),
                    GSTNo = dr["GSTNo"].ToString(),
                    TINNO = dr["TINNO"].ToString(),
                    VATNO = dr["VATNO"].ToString(),
                    CSTNO = dr["CSTNO"].ToString(),
                    STNO = dr["STNO"].ToString(),
                    ECCNO = dr["ECCNO"].ToString()
                };

                model.Logistics = new LogisticsModel
                {
                    TransporterName = dr["TransporterName"].ToString(),
                    BookingType = dr["BookingType"].ToString(),
                    FreightType = dr["FreightType"].ToString(),
                    BookedTo = dr["BookedTo"].ToString(),
                    PaymentTerms = dr["PaymentTerms"].ToString(),
                    CreditPeriod = dr["CreditPeriod"].ToString(),
                    ExportType = dr["ExportType"].ToString(),
                    OriginalInvoiceCopy = dr["OriginalInvoiceCopy"].ToString(),
                    OwnerID = dr["Owner"].ToString(),
                    
                    IndicateViaMail = dr["Indicate_Via_Mail"] != DBNull.Value && Convert.ToBoolean(dr["Indicate_Via_Mail"])
                };

              
                if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                {
                    DataRow con = ds.Tables[1].Rows[0];
                    model.ContactDetails = new ContactModel
                    {
                        Email = con["Email"].ToString(),
                        AlternateEmail = con["AlternetEmail"].ToString(),
                        ContactPersonName = con["ContactPersonName"].ToString(),
                        OffMobileNO = con["OfficeMobileNO"].ToString(),
                        WhatsappNo = con["WhatsAppNO"].ToString(),
                        Website = con["Website"].ToString(),
                        Telephone = con["Telephone"].ToString(),
                        // Collect dynamic mobiles into the list
                        MobileNumbers = new List<string>()
                    };

                  
                    string[] mobileCols = { "MobileNumber", "MobileNumber2", "MobileNumber3", "MobileNumber4", "MobileNumber5" };
                    foreach (var col in mobileCols)
                    {
                        string val = con[col]?.ToString();
                        if (!string.IsNullOrEmpty(val)) model.ContactDetails.MobileNumbers.Add(val);
                    }
                }

                if (ds.Tables.Count > 2)
                {
                    foreach (DataRow addr in ds.Tables[2].Rows)
                    {
                        var addressObj = new AddressModel
                        {
                            AddressLine1 = addr["FullAddress"].ToString(),
                            AddressLine2 = addr["AddLine2"].ToString(),
                            Pincode = addr["PinCode"].ToString(),
                            CityID = addr["City"].ToString(),
                            StateID = addr["State"].ToString(),
                            CountryID = addr["Country"].ToString(),
                            Landmark = addr["Landmark"].ToString()
                        };

                        if (addr["AddressType"].ToString() == "Address")
                            model.CompanyAddress = addressObj;
                        else if (addr["AddressType"].ToString() == "Delivery")
                            model.DeliveryAddress = addressObj;
                    }
                }
            }

            return model;
        }

        public List<ClientProfileModel> GetAllClientMasters()
        {
            List<ClientProfileModel> list = new List<ClientProfileModel>();

            var parameters = new Dictionary<string, SqlParameter>
            {
                { "Operation", new SqlParameter("@Operation", "Get_Client_Master") }
            };
           
            DataSet ds = gc.ExecuteStoredProcedureGetDataSet("SP_Update_Client", parameters.Values.ToArray());

            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    list.Add(new ClientProfileModel
                    {

                        ClientID = Convert.ToInt32(dr["ClientID"]),
                        ClientName = dr["ClientName"].ToString(),
                        GSTNo = dr["GSTNo"].ToString(),
                        PANNo = dr["PANNo"].ToString(),
                        TransporterName = dr["TransporterName"].ToString(),
                        IsActive = dr["IsActive"].ToString()

                    });
                }
            }
            return list;
        }

        public string AddClientDetails(string data, string CreatedBy)
        {
            try
            {
                var options = new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var model = System.Text.Json.JsonSerializer.Deserialize<ClientSubmissionViewModel>(data, options);

                if (model == null) return "Error: Data deserialization failed.";

                var mobiles = model.ContactDetails?.MobileNumbers ?? new List<string>();
                var outParam = new SqlParameter("@result", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };

                var parameters = new Dictionary<string, SqlParameter>
                {
                    
                    { "CompanyID", new SqlParameter("@CompanyID", model.ClientID) },

                
                    { "ClientName", new SqlParameter("@ClientName", model.ClientDetails?.ClientName ?? (object)DBNull.Value) },
                    { "ClientType", new SqlParameter("@ClientType", model.ClientDetails?.ClientType ?? (object)DBNull.Value) },
                    { "PANNo", new SqlParameter("@PANNo", model.ClientDetails?.PANNo ?? (object)DBNull.Value) },
                    { "TINNO", new SqlParameter("@TINNO", model.ClientDetails?.TINNO ?? (object)DBNull.Value) },
                    { "VATNO", new SqlParameter("@VATNO", model.ClientDetails?.VATNO ?? (object)DBNull.Value) },
                    { "CSTNO", new SqlParameter("@CSTNO", model.ClientDetails?.CSTNO ?? (object)DBNull.Value) },
                    { "STNO", new SqlParameter("@STNO", model.ClientDetails?.STNO ?? (object)DBNull.Value) },
                    { "ECCNO", new SqlParameter("@ECCNO", model.ClientDetails?.ECCNO ?? (object)DBNull.Value) },
                    { "GSTNo", new SqlParameter("@GSTNo", model.ClientDetails?.GSTNo ?? (object)DBNull.Value) },

                    { "TransporterName", new SqlParameter("@TransporterName", model.Logistics?.TransporterName ?? (object)DBNull.Value) },
                    { "BookingType", new SqlParameter("@BookingType", model.Logistics?.BookingType ?? (object)DBNull.Value) },
                    { "FreightType", new SqlParameter("@FreightType", model.Logistics?.FreightType ?? (object)DBNull.Value) },
                    { "BookedTo", new SqlParameter("@BookedTo", model.Logistics?.BookedTo ?? (object)DBNull.Value) },
                    { "PaymentTerms", new SqlParameter("@PaymentTerms", model.Logistics?.PaymentTerms ?? (object)DBNull.Value) },
                    { "CreditPeriod", new SqlParameter("@CreditPeriod", Convert.ToInt32(model.Logistics?.CreditPeriod ?? "0")) },
                    { "ExportType", new SqlParameter("@ExportType", model.Logistics?.ExportType ?? (object)DBNull.Value) },
                    { "OriginalInvoiceCopy", new SqlParameter("@OriginalInvoiceCopy", model.Logistics?.OriginalInvoiceCopy ?? (object)DBNull.Value) },
                    { "Owner", new SqlParameter("@Owner", model.Logistics?.OwnerID ?? (object)DBNull.Value) },
                    { "Indicate_Via_Mail", new SqlParameter("@Indicate_Via_Mail", (model.Logistics?.IndicateViaMail ?? false) ? "1" : "0") },

             
                    { "Email", new SqlParameter("@Email", model.ContactDetails?.Email ?? (object)DBNull.Value) },
                    { "AlternateEmail", new SqlParameter("@AlternateEmail", model.ContactDetails?.AlternateEmail ?? (object)DBNull.Value) },
                    { "ContactPersonName", new SqlParameter("@ContactPersonName", model.ContactDetails?.ContactPersonName ?? (object)DBNull.Value) },
                    { "OffMobileNO", new SqlParameter("@OffMobileNO", model.ContactDetails?.OffMobileNO ?? (object)DBNull.Value) },
                    { "WhatsappNo", new SqlParameter("@WhatsappNo", model.ContactDetails?.WhatsappNo ?? (object)DBNull.Value) },
                    { "Website", new SqlParameter("@Website", model.ContactDetails?.Website ?? (object)DBNull.Value) },
                    { "Telephone", new SqlParameter("@Telephone", model.ContactDetails?.Telephone ?? (object)DBNull.Value) },
                    { "Mobile1", new SqlParameter("@Mobile1", mobiles.Count > 0 ? mobiles[0] : (object)DBNull.Value) },
                    { "Mobile2", new SqlParameter("@Mobile2", mobiles.Count > 1 ? mobiles[1] : (object)DBNull.Value) },
                    { "Mobile3", new SqlParameter("@Mobile3", mobiles.Count > 2 ? mobiles[2] : (object)DBNull.Value) },
                    { "Mobile4", new SqlParameter("@Mobile4", mobiles.Count > 3 ? mobiles[3] : (object)DBNull.Value) },
                    { "Mobile5", new SqlParameter("@Mobile5", mobiles.Count > 4 ? mobiles[4] : (object)DBNull.Value) },

                    { "Comp_Addr1", new SqlParameter("@Comp_Addr1", model.CompanyAddress?.AddressLine1 ?? (object)DBNull.Value) },
                    { "Comp_Addr2", new SqlParameter("@Comp_Addr2", model.CompanyAddress?.AddressLine2 ?? (object)DBNull.Value) },
                    { "Comp_Pin", new SqlParameter("@Comp_Pin", model.CompanyAddress?.Pincode ?? (object)DBNull.Value) },
                    { "Comp_Country", new SqlParameter("@Comp_Country", model.CompanyAddress?.CountryID ?? (object)DBNull.Value) },
                    { "Comp_Landmark", new SqlParameter("@Comp_Landmark", model.CompanyAddress?.Landmark ?? (object)DBNull.Value) },
                    { "Comp_State", new SqlParameter("@Comp_State", model.CompanyAddress?.StateID ?? (object)DBNull.Value) },
                    { "Comp_City", new SqlParameter("@Comp_City", model.CompanyAddress?.CityID ?? (object)DBNull.Value) },

             
                    { "Del_Addr1", new SqlParameter("@Del_Addr1", model.DeliveryAddress?.AddressLine1 ?? (object)DBNull.Value) },
                    { "Del_Addr2", new SqlParameter("@Del_Addr2", model.DeliveryAddress?.AddressLine2 ?? (object)DBNull.Value) },
                    { "Del_Pin", new SqlParameter("@Del_Pin", model.DeliveryAddress?.Pincode ?? (object)DBNull.Value) },
                    { "Del_Country", new SqlParameter("@Del_Country", model.DeliveryAddress?.CountryID ?? (object)DBNull.Value) },
                    { "Del_Landmark", new SqlParameter("@Del_Landmark", model.DeliveryAddress?.Landmark ?? (object)DBNull.Value) },
                    { "Del_State", new SqlParameter("@Del_State", model.DeliveryAddress?.StateID ?? (object)DBNull.Value) },
                    { "Del_City", new SqlParameter("@Del_City", model.DeliveryAddress?.CityID ?? (object)DBNull.Value) },

                    { "CreatedBy", new SqlParameter("@CreatedBy", CreatedBy) },
                    { "result", outParam }
                };

                var response = gc.ExecuteStoredProcedure("sp_SaveClientProfile", parameters);
                return response.OutputParameters["@result"]?.ToString();
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
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

        public string DeleteClientbyID(int ID)
        {
            var outParam = new SqlParameter("@result", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };
            var parameters = new Dictionary<string, SqlParameter>
                {
                     { "Operation", new SqlParameter("@Operation", "DeleteClient") },
                     { "@ClientID", new SqlParameter("@ClientID", ID) },
                     { "result", outParam }
                };

            var response = gc.ExecuteStoredProcedure("SP_Delete_Client", parameters);

            string finalResult = response.OutputParameters["@result"]?.ToString();

            return finalResult;
        }

        public string ActiveDeactiveClient(int ID)
        {
            var outParam = new SqlParameter("@result", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };
            var parameters = new Dictionary<string, SqlParameter>
                {
                     { "Operation", new SqlParameter("@Operation", "ActiveDeactiveClient") },
                     { "@ClientID", new SqlParameter("@ClientID", ID) },
                     { "result", outParam }
                };

            var response = gc.ExecuteStoredProcedure("SP_Delete_Client", parameters);

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

                 
                        Email = dr["Email"].ToString(),
                        Website = dr["Website"].ToString(),
                        Telephone = dr["Telephone"].ToString(),
                        ContactPersonName = dr["ContactPersonName"].ToString(),
                        OfficeMobileNO = dr["OfficeMobileNO"].ToString(),
                        WhatsAppNO = dr["WhatsAppNO"].ToString(),
                        AlternetEmail = dr["AlternetEmail"].ToString(),
                        MobileNumber = dr["MobileNumber"].ToString(),
                        MobileNumber2 = dr["MobileNumber2"].ToString(),

                   
                        BankName = dr["BankName"].ToString(),
                        AccountNo = dr["AccountNo"].ToString(),
                        AccountHolderName = dr["AccountHolderName"].ToString(),
                        AccountType = dr["AccountType"].ToString(),
                        IFSCCode = dr["IFSCCode"].ToString(),

                 
                        Company_Address1 = dr["Company_Address1"].ToString(),
                        Company_Address2 = dr["Company_Address2"].ToString(),
                        Company_Landmark = dr["Company_Landmark"].ToString(),
                        Company_PinCode = dr["Company_PinCode"].ToString(),
                        Company_City = dr["Company_City"].ToString(),
                        Company_State = dr["Company_State"].ToString(),
                        Company_Country = dr["Company_Country"].ToString(),

                    
                        CE_Address1 = dr["CE_Address1"].ToString(),
                        CE_Address2 = dr["CE_Address2"].ToString(),
                        CE_Landmark = dr["CE_Landmark"].ToString(),
                        CE_PinCode = dr["CE_PinCode"].ToString(),
                        CE_City = dr["CE_City"].ToString(),
                        CE_State = dr["CE_State"].ToString(),
                        CE_Country = dr["CE_Country"].ToString(),

                       
                        Div_Address1 = dr["Div_Address1"].ToString(),
                        Div_Address2 = dr["Div_Address2"].ToString(),
                        Div_Landmark = dr["Div_Landmark"].ToString(),
                        Div_PinCode = dr["Div_PinCode"].ToString(),
                        Div_City = dr["Div_City"].ToString(),
                        Div_State = dr["Div_State"].ToString(),
                        Div_Country = dr["Div_Country"].ToString(),

                       
                        Comm_Address1 = dr["Comm_Address1"].ToString(),
                        Comm_Address2 = dr["Comm_Address2"].ToString(),
                        Comm_Landmark = dr["Comm_Landmark"].ToString(),
                        Comm_PinCode = dr["Comm_PinCode"].ToString(),
                        Comm_City = dr["Comm_City"].ToString(),
                        Comm_State = dr["Comm_State"].ToString(),
                        Comm_Country = dr["Comm_Country"].ToString(),

                       
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
            
          
                    { "Email", new SqlParameter("@Email", model.Email ?? (object)DBNull.Value) },
                    { "AlternateEmail", new SqlParameter("@AlternateEmail", model.AlternateEmail ?? (object)DBNull.Value) },
                    { "ContactPersonName", new SqlParameter("@ContactPersonName", model.ContactPersonName ?? (object)DBNull.Value) },
                    { "OffMobileNO", new SqlParameter("@OffMobileNO", model.OffMobileNO ?? (object)DBNull.Value) },
                    { "WhatsappNo", new SqlParameter("@WhatsappNo", model.WhatsappNo ?? (object)DBNull.Value) },
                    { "Website", new SqlParameter("@Website", model.Website ?? (object)DBNull.Value) },
                    { "Telephone", new SqlParameter("@Telephone", model.Telephone ?? (object)DBNull.Value) },

          
                    { "Mobile1", new SqlParameter("@Mobile1", mobiles.Count > 0 ? mobiles[0] : (object)DBNull.Value) },
                    { "Mobile2", new SqlParameter("@Mobile2", mobiles.Count > 1 ? mobiles[1] : (object)DBNull.Value) },
                    { "Mobile3", new SqlParameter("@Mobile3", mobiles.Count > 2 ? mobiles[2] : (object)DBNull.Value) },
                    { "Mobile4", new SqlParameter("@Mobile4", mobiles.Count > 2 ? mobiles[2] : (object)DBNull.Value) },
                    { "Mobile5", new SqlParameter("@Mobile5", mobiles.Count > 2 ? mobiles[2] : (object)DBNull.Value) },

     
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

        
                    { "Div_Addr1", new SqlParameter("@Div_Addr1", model.DivisionAddr?.AddressLine1 ?? (object)DBNull.Value) },
                    { "Div_Addr2", new SqlParameter("@Div_Addr2", model.DivisionAddr?.AddressLine2 ?? (object)DBNull.Value) },
                    { "Div_Landmark", new SqlParameter("@Div_Landmark", model.DivisionAddr?.Landmark ?? (object)DBNull.Value) },
                    { "Div_Pin", new SqlParameter("@Div_Pin", model.DivisionAddr?.Pincode ?? (object)DBNull.Value) },
                    { "Div_City", new SqlParameter("@Div_City", model.DivisionAddr?.City ?? (object)DBNull.Value) },
                    { "Div_State", new SqlParameter("@Div_State", model.DivisionAddr?.State ?? (object)DBNull.Value) },
                    { "Div_Country", new SqlParameter("@Div_Country", model.DivisionAddr?.Country ?? (object)DBNull.Value) },

         
                    { "Comm_Addr1", new SqlParameter("@Comm_Addr1", model.CommissionerateAddr?.AddressLine1 ?? (object)DBNull.Value) },
                    { "Comm_Addr2", new SqlParameter("@Comm_Addr2", model.CommissionerateAddr?.AddressLine2 ?? (object)DBNull.Value) },
                    { "Comm_Landmark", new SqlParameter("@Comm_Landmark", model.CommissionerateAddr?.Landmark ?? (object)DBNull.Value) },
                    { "Comm_Pin", new SqlParameter("@Comm_Pin", model.CommissionerateAddr?.Pincode ?? (object)DBNull.Value) },
                    { "Comm_City", new SqlParameter("@Comm_City", model.CommissionerateAddr?.City ?? (object)DBNull.Value) },
                    { "Comm_State", new SqlParameter("@Comm_State", model.CommissionerateAddr?.State ?? (object)DBNull.Value) },
                    { "Comm_Country", new SqlParameter("@Comm_Country", model.CommissionerateAddr?.Country ?? (object)DBNull.Value) },

            
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

                  
                    { "Email", new SqlParameter("@Email", model.Email ?? (object)DBNull.Value) },
                    { "AlternateEmail", new SqlParameter("@AlternateEmail", model.AlternateEmail ?? (object)DBNull.Value) },
                    { "ContactPersonName", new SqlParameter("@ContactPersonName", model.ContactPersonName ?? (object)DBNull.Value) },
                    { "OffMobileNO", new SqlParameter("@OffMobileNO", model.OffMobileNO ?? (object)DBNull.Value) },
                    { "WhatsappNo", new SqlParameter("@WhatsappNo", model.WhatsappNo ?? (object)DBNull.Value) },
                    { "Website", new SqlParameter("@Website", model.Website ?? (object)DBNull.Value) },
                    { "Telephone", new SqlParameter("@Telephone", model.Telephone ?? (object)DBNull.Value) },

                    { "Mobile1", new SqlParameter("@Mobile1", mobiles.Count > 0 ? mobiles[0] : (object)DBNull.Value) },
                    { "Mobile2", new SqlParameter("@Mobile2", mobiles.Count > 1 ? mobiles[1] : (object)DBNull.Value) },
                    { "Mobile3", new SqlParameter("@Mobile3", mobiles.Count > 2 ? mobiles[2] : (object)DBNull.Value) },
                    { "Mobile4", new SqlParameter("@Mobile4", mobiles.Count > 3 ? mobiles[3] : (object)DBNull.Value) },
                    { "Mobile5", new SqlParameter("@Mobile5", mobiles.Count > 4 ? mobiles[4] : (object)DBNull.Value) },

              
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

        public List<C_Master> GetOwnerList()
        {
            List<C_Master> list = new List<C_Master>();
            var parameters = new Dictionary<string, SqlParameter>
                {
                    { "Operation", new SqlParameter("@Operation", "Get_Owner") }
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

      
        public string UpdateLocation(IP_Master_Model model, string ModifiedBy)
        {
            try
            {
       
                string ipNames = string.Join(",", model.IPList.Select(x => x.IP_Name.Replace(",", "")));
                string ipAddresses = string.Join(",", model.IPList.Select(x => x.IPAdd));

                var outParam = new SqlParameter("@result", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };

                var parameters = new Dictionary<string, SqlParameter>
                {
                    { "Operation", new SqlParameter("@Operation", "UPDATE_LOCATION_FULL") },
                    { "LocationID", new SqlParameter("@LocationID", model.ID) },
                    { "LocationName", new SqlParameter("@LocationName", model.Location) },
                    { "IP_Name", new SqlParameter("@IP_Name", ipNames) },
                    { "IPAdd", new SqlParameter("@IPAdd", ipAddresses) },
                    { "ModifiedBy", new SqlParameter("@ModifiedBy", ModifiedBy) },
                    { "result", outParam }
                };

                var response = gc.ExecuteStoredProcedure("sp_IP_Master", parameters);
                string dbResult = response.OutputParameters["@result"]?.ToString();

                if (dbResult == "DUPLICATE") return "0";
                return dbResult == "1" ? "1" : dbResult;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

       

        public string AddIPAddress(IP_Master_Model model, string ModifiedBy)
        {
            try
            {

                var xmlElements = model.IPList.Select(i =>new XElement("row",new XElement("IP", i.IPAdd),new XElement("Name", i.IP_Name) ));

                string ipXml = new XElement("root", xmlElements).ToString();
                var resultParam = new SqlParameter("@result", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };
                var parameters = new Dictionary<string, SqlParameter>
                {
                    { "Operation", new SqlParameter("@Operation", "ADD_LOCATION_XML") },
                    { "LocationName", new SqlParameter("@LocationName", model.Location) },
                    { "ModifiedBy", new SqlParameter("@ModifiedBy", ModifiedBy) },
                    { "IPAdd", new SqlParameter("@IPAdd", ipXml) },
                    { "result", resultParam }
                };

              
                var response = gc.ExecuteStoredProcedure("sp_IP_Master", parameters);
                string finalResult = response.OutputParameters["@result"]?.ToString();

                if (finalResult == "DUPLICATE") return "0";
                return finalResult == "1" ? "1" : "Error: " + finalResult;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string UpdateIPAddress(IPRowADd model, string ModifiedBy)
        {
            try
            {
                var outParam = new SqlParameter("@result", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };
                var parameters = new Dictionary<string, SqlParameter>
                {
                  { "@LocationID", new SqlParameter("@LocationID", Convert.ToInt32( model.LocationID)) },
                    { "IP_Name", new SqlParameter("@IP_Name", model.IP_Name) },
                    { "IPAdd", new SqlParameter("@IPAdd", model.IPAdd) },
                    { "ModifiedBy", new SqlParameter("@ModifiedBy", ModifiedBy) },
                    { "Operation", new SqlParameter("@Operation", "Add_New_IP") },
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

        public string GenerateRandomPassword(int length = 8)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random random = new Random();

            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public string ResetPassword(string userId, string ModifiedBy)
        {
         
            try
            {
                string rawPassword = GenerateRandomPassword(10);
                string encryptionKey = _configuration["EncryptionSettings:Key"];
                string EncrPassword = gc.Encrypt(rawPassword, encryptionKey);
                string bodyContent = $"Your Updated password reset is: <b>{rawPassword}</b>";
                bool status= _email.SendEmail(ModifiedBy , bodyContent );
                var outParam = new SqlParameter("@result", SqlDbType.NVarChar, 250)
                {
                    Direction = ParameterDirection.Output
                };

                var parameters = new Dictionary<string, SqlParameter>
                {
                    { "UserID", new SqlParameter("@UserID", userId) },
                    { "Password", new SqlParameter("@Password", EncrPassword) },
                    { "Operation", new SqlParameter("@Operation", "Reset_Password") },
                   
                    { "CreatedBy", new SqlParameter("@CreatedBy", ModifiedBy) },
                    { "result", outParam }
                };


                var response =  gc.ExecuteStoredProcedure("sp_User_Master", parameters);
                string result = response.OutputParameters["@result"]?.ToString();
                return result;
            }
            catch (Exception ex)
            {
                
               return   "ERROR: " + ex.Message;
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


        public string DeleteIP_ADD(string LocationID)
        {
            try
            {
                var outParam = new SqlParameter("@result", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };
                var parameters = new Dictionary<string, SqlParameter>
                {
                    { "ID", new SqlParameter("@LocationID", Convert.ToInt32( LocationID)) },
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


        public string DeactivateLocation(string LocationID)
        {
            try
            {
                var outParam = new SqlParameter("@result", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };
                var parameters = new Dictionary<string, SqlParameter>
                {
                    { "ID", new SqlParameter("@LocationID", Convert.ToInt32( LocationID)) },
                    { "Operation", new SqlParameter("@Operation", "TOGGLE_LOCATION_STATUS") },
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

        public LocationDetailsDTO GetLocationDetails(int locationId)
        {
            var details = new LocationDetailsDTO();

            try
            {
               
                var outParam = new SqlParameter("@result", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };
                var parameters = new Dictionary<string, SqlParameter>
                {
                    { "LocationID", new SqlParameter("@LocationID", locationId) },
                    { "Operation", new SqlParameter("@Operation", "Get_IP_MasterUserList") },
                    { "result", outParam }
                };

          
                DataSet response = gc.ExecuteStoredProcedureGetDataSet("sp_IP_Master", parameters.Values.ToArray());

                if (response.Tables.Count > 0)
                {
                    details.IPs = response.Tables[0].AsEnumerable().Select(r => new IPAddressDTO
                    {
                        IPAddrID = Convert.ToInt32(r["IPAddrID"]),
                        IPAddress = r["IPAddress"].ToString(),
                        IP_Name = r["IP_Name"].ToString()
                    }).ToList();
                }

                if (response.Tables.Count > 1)
                {
                    details.Users = response.Tables[1].AsEnumerable().Select(r => new UserDTO
                    {
                        UserID = r["UserID"].ToString()
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
             
                throw new Exception("Error fetching location details from SP", ex);
            }

            return details;
        }

        public List<IP_Master_Display> GetIPMasterList()
        {
            List<IP_Master_Display> ipList = new List<IP_Master_Display>();
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
                        ipList.Add(new IP_Master_Display
                        {
                            LocationID = dr["LocationID"].ToString(),
                            LocationName = dr["LocationName"].ToString(),
                            TotalIPCount = dr["TotalIPCount"].ToString(),
                            TotalUserCount = dr["TotalUserCount"].ToString(),
                            IsActive = dr["IsActive"].ToString()
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


        public List<IP_Users> GetIPMasterUserList(int id)
        {
            List<IP_Users> ipList = new List<IP_Users>();
            try
            {
                var parameters = new Dictionary<string, SqlParameter>
                    {
                        { "Operation", new SqlParameter("@Operation", "Get_IP_MasterUserListIPAssigned") }
                      , { "@LocationID", new SqlParameter("@LocationID",id) }


                    };

                DataSet ds = gc.ExecuteStoredProcedureGetDataSet("sp_IP_Master", parameters.Values.ToArray());

                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        ipList.Add(new IP_Users
                        {
                            UserName = dr["UserName"].ToString(),
                            EmailID = dr["EmailID"].ToString(),
                            MobileNO = dr["MobileNO"].ToString()
                          
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


        public List<LocationList> GetLocationList()
        {
            List<LocationList> ipList = new List<LocationList>();
            try
            {
                var outParam = new SqlParameter("@result", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };
                var parameters = new Dictionary<string, SqlParameter>
                    {
                        { "Operation", new SqlParameter("@Operation", "Get_Location_List") },
                         { "result", outParam }

                    };

                DataSet ds = gc.ExecuteStoredProcedureGetDataSet("sp_User_Master", parameters.Values.ToArray());

                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        ipList.Add(new LocationList
                        {
                            LocationID = dr["LocationID"].ToString(),
                            LocationName = dr["LocationName"].ToString()
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

        public List<RoleList> GetRoleList()
        {
            List<RoleList> ipList = new List<RoleList>();
            try
            {
                var outParam = new SqlParameter("@result", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };
                var parameters = new Dictionary<string, SqlParameter>
                    {
                        { "Operation", new SqlParameter("@Operation", "Get_Role_List") },
                           { "result", outParam }

                    };

                DataSet ds = gc.ExecuteStoredProcedureGetDataSet("sp_User_Master", parameters.Values.ToArray());

                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        ipList.Add(new RoleList
                        {
                            RoleID = dr["RoleID"].ToString(),
                            RoleName = dr["RoleName"].ToString()
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
                    var outParam = new SqlParameter("@result", SqlDbType.NVarChar, 250)
                    {
                        Direction = ParameterDirection.Output
                    };
                    var parameters = new Dictionary<string, SqlParameter>
                    {
                        { "Operation", new SqlParameter("@Operation", "Get_All_Users") },
                             { "result", outParam }
                    };

                DataSet ds = gc.ExecuteStoredProcedureGetDataSet("sp_User_Master", parameters.Values.ToArray());

                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        users.Add(new IPViewModel_withIPS
                        {
                            ID = dr["ID"]?.ToString(),
                            UserName = dr["UserName"]?.ToString(),
                            EmailID = dr["EmailID"]?.ToString(),
                            ContactNo = dr["ContactNo"]?.ToString(),
                            AccessType = dr["AccessType"]?.ToString(),
                            IsActive = dr["IsActive"]?.ToString(),
                            AssignedLocations = dr["AssignedLocations"]?.ToString(),
                            AssignedRoles = dr["AssignedRoles"]?.ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return users;
        }

        public string UpdateUser(UserUpdateModel user, string modifiedBy)
        {
            string message = "0";
            try
            {
                var outParam = new SqlParameter("@result", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };

                string roleStr = user.RoleIDs != null ? string.Join(",", user.RoleIDs) : "";
                string locStr = user.LocationIDs != null ? string.Join(",", user.LocationIDs) : "";
                string menuStr = user.SelectedMenuRights != null ? string.Join(",", user.SelectedMenuRights) : "";

                var parameters = new Dictionary<string, SqlParameter>
                {
                    { "UserID", new SqlParameter("@UserID", user.UserID) }, 
                    { "UserName", new SqlParameter("@UserName", user.UserName) },
                    { "ContactNo", new SqlParameter("@ContactNo", user.ContactNo) },
                    { "Email", new SqlParameter("@Email", user.EmailID) },
                    { "AccessType", new SqlParameter("@AccessType", user.AccessType) },
                    { "ExpiryDate", new SqlParameter("@ExpiryDate", user.ExpiryDate) },
                    { "CreatedBy", new SqlParameter("@CreatedBy", modifiedBy) },
                    { "RoleIDs", new SqlParameter("@RoleIDs", roleStr) },
                    { "LocationIDs", new SqlParameter("@LocationIDs", locStr) },
                    { "MenuIDs", new SqlParameter("@MenuIDs", menuStr) },
                    { "Operation", new SqlParameter("@Operation", "Update_User") },
                    { "result", outParam }
                };

             var response =   gc.ExecuteStoredProcedure("sp_User_Master", parameters);
                string result = response.OutputParameters["@result"]?.ToString();
                return result;
            }
            catch (Exception ex)
            {
                message = "ERROR: " + ex.Message;
            }
            return message;
        }
        public UserUpdateModel GetUserByID(string id)
        {
            UserUpdateModel user = null;
            try
            {
                var outParam = new SqlParameter("@result", SqlDbType.NVarChar, 250)
                {
                    Direction = ParameterDirection.Output
                };

                var parameters = new Dictionary<string, SqlParameter>
                {
                    { "UserID", new SqlParameter("@UserID", id) },
                    { "Operation", new SqlParameter("@Operation", "Get_User_By_ID") },
                    { "result", outParam }
                };

               
                DataSet ds = gc.ExecuteStoredProcedureGetDataSet("sp_User_Master", parameters.Values.ToArray());

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    user = new UserUpdateModel
                    {
                        
                        ID = dr["ID"] != DBNull.Value ? Convert.ToInt32(dr["ID"]) : (int?)null,
                        UserID = dr["UserID"].ToString(),
                        UserName = dr["UserName"].ToString(),
                        EmailID = dr["EmailID"].ToString(),
                        ContactNo = dr["ContactNo"].ToString(),
                        AccessType = dr["AccessType"].ToString(),
                        ExpiryDate = dr["ExpiryDate"].ToString(),

                        RoleIDs = dr["RoleIDs"] != DBNull.Value ?
                            dr["RoleIDs"].ToString().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList() : new List<int>(),

                        LocationIDs = dr["LocationIDs"] != DBNull.Value ?
                            dr["LocationIDs"].ToString().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList() : new List<int>(),

                        SelectedMenuRights = dr["MenuIDs"] != DBNull.Value ?
                            dr["MenuIDs"].ToString().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList() : new List<int>()
                    };

                 
                    user.IsDirectAccess = user.SelectedMenuRights != null && user.SelectedMenuRights.Any();
                }
            }
            catch (Exception ex)
            {
                
                throw;
            }
            return user;
        }


        public string AddUser(UserSaveViewModel user, string CreatedBy)
        {
           
            string message = "0";
            try
            {
                var outParam = new SqlParameter("@result", SqlDbType.NVarChar, 250)
                {
                    Direction = ParameterDirection.Output
                };

                string roleIdsStr = user.RoleIDs != null ? string.Join(",", user.RoleIDs) : null;
                string locationIdsStr = user.LocationIDs != null ? string.Join(",", user.LocationIDs) : null;
                string menuIdsStr = user.SelectedMenuRights != null ? string.Join(",", user.SelectedMenuRights) : null;


                string encryptionKey = _configuration["EncryptionSettings:Key"];
                string EncrPassword = gc.Encrypt(user.Password, encryptionKey);
                var parameters = new Dictionary<string, SqlParameter>
                    {
                        { "UserName", new SqlParameter("@UserName", user.UserName ?? (object)DBNull.Value) },
                        { "UserID", new SqlParameter("@UserID", user.UserID ?? (object)DBNull.Value) },
                        { "ContactNo", new SqlParameter("@ContactNo", user.ContactNo ?? (object)DBNull.Value) },
                        { "Password", new SqlParameter("@Password", EncrPassword?? (object)DBNull.Value) },
                        { "AccessType", new SqlParameter("@AccessType", user.AccessType ?? (object)DBNull.Value) },
                        { "CreatedBy", new SqlParameter("@CreatedBy", CreatedBy ?? (object)DBNull.Value) },
                        { "Email", new SqlParameter("@Email", user.EmailID ?? (object)DBNull.Value) },
                        { "RoleIDs", new SqlParameter("@RoleIDs", (object)roleIdsStr ?? DBNull.Value) },
                        { "LocationIDs", new SqlParameter("@LocationIDs", (object)locationIdsStr ?? DBNull.Value) },
                        { "MenuIDs", new SqlParameter("@MenuIDs", (object)menuIdsStr ?? DBNull.Value) },
                        { "ExpiryDate", new SqlParameter("@ExpiryDate",user.ExpiryDate) },
                        { "Operation", new SqlParameter("@Operation", "Add_User") },
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
            return message;
        }
    }
}
