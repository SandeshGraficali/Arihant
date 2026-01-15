using Arihant.Models.Client;
using Arihant.Models.Company_Master;
using Arihant.Models.IP_Master;
using Arihant.Models.Rights_Master;
using Arihant.Models.Unit;
using Arihant.Models.User_Master;
using Arihant.Models.Vender;
using Arihant.Services;
using DocumentFormat.OpenXml.EMMA;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Arihant.Controllers
{

    public class Users_MasterController : Controller
    {
        private readonly DL_Users_Master _user;
        public Users_MasterController(DL_Users_Master _master)
        {
            _user = _master;
        }

        [HttpPost]
        public JsonResult SaveUnit(string jsonData)
        {
            try
            {
                var options = new System.Text.Json.JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var model = System.Text.Json.JsonSerializer.Deserialize<UnitMaster>(jsonData, options);

                string ModifiedBy = HttpContext.Session.GetString("UserName") ?? "";
                string result = _user.AddUnitDetails(model, ModifiedBy);

                if (result.StartsWith("Success", StringComparison.OrdinalIgnoreCase))
                {
                    return Json(new { success = true, message = result });
                }
                else
                {
                    return Json(new { success = false, message = result });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }



        public IActionResult UnitMaster()
        {
            var list = _user.GetUnitList();
            return View(list);
        }


        public IActionResult AddUnit(int id=0)
        {
            ViewBag.countrylist = _user.GetC_MasterList();
            ViewBag.Addresslist = _user.GetC_MasterAddresList();
            ViewBag.CompanyList = _user.GetCompany_MasterList();

            if (id > 0)
            {

                var model = _user.GetUNITByID(id);
                ViewBag.IsEdit = true;
                return View(model);
            }
            else
            {
                ViewBag.IsEdit = false;
            }

            return View();
        }

        [HttpPost]

        public JsonResult DeleteUnitStatus(int UinitId, int status, string operation = "TOGGLE_STATUS")
        {
            string ModifiedBy = HttpContext.Session.GetString("UserName") ?? "";
          
            _user.deleteUnitService(UinitId, ModifiedBy, operation);
            return Json(new { success = true, message = "Unit Deleted Sucessfully" });

        }


        [HttpPost]
        [Authorize(Policy = "Vendors Master")]
        public JsonResult UpdateVendorStatusNew(int vendorId, int status, string operation = "TOGGLE_STATUS")
        {
            string ModifiedBy = HttpContext.Session.GetString("UserName") ?? "";
            bool isActive = (status == 1);
            string result = _user.UpdateVendorStatusService(vendorId, isActive, ModifiedBy, operation);

            if (result.StartsWith("Success"))
            {
                return Json(new { success = true, message = result });
            }
            return Json(new { success = false, message = result });
        }

        [Authorize(Policy = "Vendors Master")]
        public IActionResult EditVendor(int id)
        {
            var model = _user.GetVendorByID(id);
            if (model == null) return NotFound();

            ViewBag.IsEdit = true;
            return View("Add_Vender", model);
        }


        [HttpPost]
        [Authorize(Policy = "Vendors Master")]
        public JsonResult SaveVendor(string jsonData)
        {
            try
            {
                var options = new System.Text.Json.JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var model = System.Text.Json.JsonSerializer.Deserialize<VendorMasterVM>(jsonData, options);

                string ModifiedBy = HttpContext.Session.GetString("UserName") ?? "";
                string result = _user.AddVendorDetails(model, ModifiedBy);

                if (result.StartsWith("Success", StringComparison.OrdinalIgnoreCase))
                {
                    return Json(new { success = true, message = result });
                }
                else
                {
                    return Json(new { success = false, message = result });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


        [Authorize(Policy = "Vendors Master")]
        public IActionResult Vender_Master()
        {
            try
            {
                var list = _user.GetVendorList();
                return View(list);
            }
            catch (Exception ex)
            {
                return View();
            }


        }

        [HttpPost]
        [Authorize(Policy = "Vendors Master")]
        public JsonResult UpdateVendorStatus(int vendorId, int status)
        {

            bool isActive = (status == 1);
            string ModifiedBy = HttpContext.Session.GetString("UserName") ?? "";
            string result = _user.UpdateVendorStatusService(vendorId, isActive, ModifiedBy);

            if (result.StartsWith("Success"))
            {
                return Json(new { success = true, message = result });
            }
            return Json(new { success = false, message = result });
        }


        [HttpPost]

        public JsonResult UpdateUnitStatus(int unitId, int status)
        {

            bool isActive = (status == 1);
            string ModifiedBy = HttpContext.Session.GetString("UserName") ?? "";
            _user.UpdateUnitService(unitId, isActive, ModifiedBy);
            return Json(new { success = true, message = "Unit Status Sucessfully Chnage" });

        }

        [Authorize(Policy = "Vendors Master")]
        public IActionResult Add_Vender(int id = 0)
        {
            ViewBag.countrylist = _user.GetC_MasterList();
            ViewBag.Addresslist = _user.GetC_MasterAddresList();
            ViewBag.PurchaseList = _user.GetC_MasterPurchaseList();
            Arihant.Models.Vender.VendorMasterVM model = new Arihant.Models.Vender.VendorMasterVM();

            if (id > 0)
            {

                model = _user.GetVendorByID(id);
                ViewBag.IsEdit = true;
            }
            else
            {
                ViewBag.IsEdit = false;
            }

            return View(model);
        }


        [HttpPost]
        [Authorize(Policy = "Customer Master")]
        public JsonResult SaveClient(string jsonData)
        {
            string ModifiedBy = HttpContext.Session.GetString("UserName") ?? "";
            string result = _user.AddClientDetails(jsonData, ModifiedBy);

            if (result == "1")
                return Json(new { success = true, message = "User Added successfully!" });
            else
                return Json(new { success = false, message = result });

        }

        [Authorize(Policy = "Customer Master")]
        public IActionResult AllClientMasterList()
        {
            List<ClientProfileModel> list = _user.GetAllClientMasters();

            return View(list);
        }


        [HttpPost]
        [Authorize(Policy = "Customer Master")]
        public JsonResult UpdateClientStatus(int id)
        {
            try
            {
                string result = _user.ActiveDeactiveClient(id);
                if (result == "1")
                    return Json(new { success = true, message = "Client De-Activetd successfully!" });
                else
                    return Json(new { success = false, message = "Client failed. Please try again." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


        [HttpPost]
        [Authorize(Policy = "Customer Master")]
        public JsonResult DeleteClient(int id)
        {
            try
            {

                string result = _user.DeleteClientbyID(id);
                if (result == "1")
                    return Json(new { success = true, message = "Client De-Activetd successfully!" });
                else
                    return Json(new { success = false, message = "Client failed. Please try again." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [Authorize(Policy = "Customer Master")]
        public IActionResult ClientMaster(int? id)
        {
            ViewBag.countrylist = _user.GetC_MasterList();
            ViewBag.Addresslist = _user.GetC_MasterAddresList();
            ViewBag.ownerlist = _user.GetOwnerList();
            if (id.HasValue && id.Value > 0)
            {
                var clientData = _user.GetClientById(id.Value);
                ViewBag.IsEdit = true;
                return View(clientData);
            }
            ViewBag.IsEdit = false;
            return View(new ClientSubmissionViewModel());
        }

        [HttpPost]
        public JsonResult UpdateStatus(int id, bool status)
        {
            try
            {
                string result = _user.ActiveAndDeactiveConpany(id);

                if (result == "1")
                    return Json(new { success = true, message = "Company De-Activetd successfully!" });
                else
                    return Json(new { success = false, message = "Deactivation failed. Please try again." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Policy = "Company")]
        public JsonResult DeleteCompany(int CompanyID)
        {
            try
            {
                string result = _user.DeactiveConpany(CompanyID.ToString());

                if (result == "1")
                    return Json(new { success = true, message = "Company De-Activetd successfully!" });
                else
                    return Json(new { success = false, message = "Deactivation failed. Please try again." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Policy = "Company")]
        public IActionResult UpdateCompanyProfile(string data)
        {
            try
            {
                string createdBy = HttpContext.Session.GetString("UserName") ?? "System";
                string result = _user.UpdateCompanyDetails(data, createdBy);

                if (result == "1")
                    return Json(new { success = true, message = "Company Updated successfully!" });
                else
                    return Json(new { success = false, message = "Company profile Details Alredy Exist Please try Other." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [Authorize(Policy = "Company")]
        public IActionResult GetCompanyList()
        {
            List<CompanyProfileModel> list = _user.GetAllCompanyMasters();
            return View(list);
        }


        [HttpGet]
        [Authorize(Policy = "Company")]
        public IActionResult EditCompany(int id)
        {
            ViewBag.Addresslist = _user.GetC_MasterAddresList();
            ViewBag.countrylist = _user.GetC_MasterList();
            var data = _user.getCompanyByID(id.ToString());
            var model = data.FirstOrDefault();
            return View("CompanyForm", model);
        }

        [HttpGet]
        public IActionResult GetCitiesByState(string stateId)
        {
            DataSet ds = _user.GetC_MasterCityList(stateId);
            var cities = new List<object>();

            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (System.Data.DataRow row in ds.Tables[0].Rows)
                {
                    cities.Add(new
                    {
                        id = row["ID"].ToString(),
                        name = row["Name"].ToString()
                    });
                }
            }

            return Json(cities);
        }

        [HttpGet]
        public IActionResult GetState(string countryid)
        {
            DataSet ds = _user.GetC_MasterStateList(countryid);
            var cities = new List<object>();

            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (System.Data.DataRow row in ds.Tables[0].Rows)
                {
                    cities.Add(new
                    {
                        id = row["ID"].ToString(),
                        name = row["Name"].ToString()
                    });
                }
            }

            return Json(cities);
        }

        [HttpPost]
        [Authorize(Policy = "Company")]
        public IActionResult SaveCompanyProfile(string data)
        {
            try
            {
                string createdBy = HttpContext.Session.GetString("UserName") ?? "System";
                string result = _user.SaveCompanyDetails(data, createdBy);

                if (result == "1")
                    return Json(new { success = true, message = "Company created successfully!" });
                else
                    return Json(new { success = false, message = "Company profile Details Alredy Exist Please try Other." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


        [Authorize(Policy = "Company")]
        public IActionResult CompanyMaster()
        {
            ViewBag.Addresslist = _user.GetC_MasterAddresList();
            ViewBag.countrylist = _user.GetC_MasterList();
            return View("CompanyForm", new CompanyProfileModel());
        }

        [Authorize(Policy = "Company")]
        public IActionResult CompanyForm()
        {

            return View();
        }




        [HttpPost]
        [Authorize(Policy = "RoleMaster")]
        public JsonResult DeleteRole(int id)
        {
            try
            {
                string result = _user.DeactiveRole(id.ToString());
                if (result == "1")
                    return Json(new { success = true, message = "Role deleted successfully!\"" });
                else
                    return Json(new { success = false, message = "Could not delete role. It might be in use." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Policy = "RoleMaster")]
        public JsonResult UpdateRolePermissions([FromBody] SaveRoleDTO model)
        {
            try
            {
                string createdBy = HttpContext.Session.GetString("UserName") ?? "System";
                string menuIdsString = model.MenuIDs != null ? string.Join(",", model.MenuIDs) : "";
                string result = _user.UpdateRoleWithRights(model.RoleName, menuIdsString, createdBy, model.RoleID.ToString());
                if (result == "1")
                {
                    return Json(new { success = true, message = "Menu  Added successfully!", result = result });
                }
                else if (result == "0")
                {
                    return Json(new { success = false, message = "Role is already exists.", result = result });
                }
                else
                {
                    return Json(new { success = false, message = "Error: " + result });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Policy = "RoleMaster")]
        public JsonResult SaveRole(string RoleName, string MenuIDs)
        {
            try
            {
                string createdBy = HttpContext.Session.GetString("UserName") ?? "System";
                string result = _user.InsertRoleWithRights(RoleName, MenuIDs, createdBy);
                if (result == "1")
                {
                    return Json(new { success = true, message = "Role Added successfully!", result = result });
                }
                else if (result == "0")
                {
                    return Json(new { success = false, message = "Role is already exists.", result = result });
                }
                else
                {
                    return Json(new { success = false, message = "Error: " + result });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        [Authorize(Policy = "RoleMaster")]
        public ActionResult EditRole(int id)
        {
            List<MenuRightViewModel> allMenus = _user.GetMenuRights();
            List<DTO_MenuRight> allAssigned = _user.ManageRights();
            var particularRights = allAssigned.Where(x => x.RoleID == id).ToList();
            ViewBag.RoleName = particularRights.FirstOrDefault()?.RoleName ?? "New Role";
            ViewBag.RoleID = id;
            var groupedRights = allMenus.GroupBy(x => x.Module).ToList();
            ViewBag.GroupedMenuRights = groupedRights;
            var model = allMenus.Select(m => new EditRoleViewModel
            {
                MenuID = m.MenuID,
                Module = m.Module,
                RightName = m.RightName,
                IsAssigned = particularRights.Any(pr => pr.MenuID == m.MenuID)
            })
            .GroupBy(x => x.Module)
            .ToList();
            return View(model);
        }

        [Authorize(Policy = "RoleMaster")]
        public IActionResult Role_Master()
        {
            List<MenuRightViewModel> allRights = _user.GetMenuRights();
            var groupedRights = allRights.GroupBy(x => x.Module).ToList();

            ViewBag.GroupedMenuRights = groupedRights;
            return View(groupedRights);
        }


        [Authorize(Policy = "RoleMaster")]
        public IActionResult Manage_Role_Master()
        {
            List<DTO_MenuRight> allRights = _user.ManageRights();

            return View(allRights);
        }

        [HttpPost]
        [Authorize(Policy = "IP Master")]
        public JsonResult UpdateFullLocation(IP_Master_Model model)
        {
            try
            {
                string user = HttpContext.Session.GetString("UserName") ?? "System";
                string result = _user.UpdateLocation(model, user);

                if (result == "1")
                {
                    return Json(new { success = true, message = "IP details updated successfully!" });
                }
                else if (result == "0")
                {
                    return Json(new { success = false, message = "Data already exists." });
                }
                else
                {
                    return Json(new { success = false, message = "Error: " + result });
                }

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        [Authorize(Policy = "IP Master")]
        public IActionResult GetUsersByLocation(int locationId)
        {
            List<IP_Users> users = _user.GetIPMasterUserList(locationId);

            return Json(new { success = true, users });
        }


        [HttpPost]
        [Authorize(Policy = "IP Master")]
        public JsonResult AddIPMaster(IP_Master_Model model)
        {

            try
            {
                string ModifiedBy = HttpContext.Session.GetString("UserName") ?? "";
                string result = _user.AddIPAddress(model, ModifiedBy);

                if (result == "1")
                {
                    return Json(new { success = true, message = "IP details updated successfully!" });
                }
                else if (result == "0")
                {
                    return Json(new { success = false, message = "Data already exists." });
                }
                else
                {
                    return Json(new { success = false, message = "Error: " + result });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });

            }

        }

        [HttpPost]
        [Authorize(Policy = "IP Master")]
        public JsonResult SaveIPMaster(IPRowADd model)
        {
            try
            {
                string ModifiedBy = HttpContext.Session.GetString("UserName") ?? "";
                string result = _user.UpdateIPAddress(model, ModifiedBy);

                if (result == "1")
                {
                    return Json(new { success = true, message = "IP details updated successfully!" });
                }
                else if (result == "0")
                {
                    return Json(new { success = false, message = "IP  already exists." });
                }
                else
                {
                    return Json(new { success = false, message = "Error: " + result });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });

            }


        }

        [HttpPost]
        [Authorize(Policy = "IP Master")]
        public JsonResult DeactiveteLocationMaster(string id)
        {
            try
            {
                string result = _user.DeactivateLocation(id);
                if (result == "1")
                    return Json(new { success = true, message = "IP DeActiveted successfully" });
                else
                    return Json(new { success = false, message = "IP not DeActiveted" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


        [HttpPost]
        [Authorize(Policy = "IP Master")]
        public JsonResult DeleteIPMaster(string id)
        {
            try
            {
                string result = _user.DeleteIP_ADD(id);
                if (result == "1")
                    return Json(new { success = true, message = "IP deleted successfully" });
                else
                    return Json(new { success = false, message = "IP not found or already deleted" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Policy = "Users")]
        public JsonResult DeleteUser(string UserID)
        {
            try
            {
                string result = _user.DeleteUser(UserID);
                if (result == "1")
                    return Json(new { success = true, message = "User deleted successfully" });
                else
                    return Json(new { success = false, message = "User not found or already deleted" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


        [HttpPost]
        [Authorize(Policy = "Users")]
        public JsonResult ResetPassword(string UserID)
        {
            try
            {


                string ModifiedBy = HttpContext.Session.GetString("UserName") ?? "";
                string result = _user.ResetPassword(UserID, ModifiedBy);

                if (result == "1")
                    return Json(new { success = true });
                else
                    return Json(new { success = false, message = "Reset failed" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


        [HttpPost]
        [Authorize(Policy = "Users")]
        public JsonResult ActiveUser(string UserID)
        {
            try
            {
                string result = _user.ActiveUser(UserID);
                if (result == "1")
                    return Json(new { success = true, isActive = true, message = "User Activated successfully" });
                else
                    return Json(new { success = true, isActive = false, message = "User De-Activated successfully" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


        [HttpGet]


        public JsonResult GetLocationDetails(int locationId)
        {
            var data = _user.GetLocationDetails(locationId);
            return Json(new { success = true, ips = data.IPs, users = data.Users });
        }

        [Authorize(Policy = "IP Master")]
        public IActionResult IP_Master()
        {
            List<IP_Master_Display> IP_Masters = _user.GetIPMasterList();
            return View(IP_Masters);
        }

        public IActionResult Dashboard()
        {
            return View();
        }


        [Authorize(Policy = "Users")]
        public IActionResult User_Master_Dashboard()
        {

            ViewBag.LocationList = _user.GetLocationList();
            ViewBag.RoleList = _user.GetRoleList();
            List<IPViewModel_withIPS> result = _user.GetAllUsers();
            List<MenuRightViewModel> allRights = _user.GetMenuRights();
            var groupedRights = allRights.GroupBy(x => x.Module).ToList();
            ViewBag.GroupedMenuRights = groupedRights;
            return View(result);
        }

        [HttpPost]
        [Authorize(Policy = "Users")]
        public IActionResult UpdateUser([FromBody] UserUpdateModel model)
        {
            try
            {
                if (model == null)
                {
                    return Json(new { success = false, message = "Invalid data provided." });
                }

                string modifiedBy = HttpContext.Session.GetString("UserName") ?? "System";
                string result = _user.UpdateUser(model, modifiedBy);

                if (result == "1")
                {
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false, message = result });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Internal Server Error: " + ex.Message });
            }
        }

        [HttpGet]
        [Authorize(Policy = "Users")]
        public IActionResult GetUserDetails(string id)
        {

            var user = _user.GetUserByID(id);
            if (user == null) return NotFound();

            return Json(new
            {
                success = true,
                data = new
                {
                    userID = user.UserID,
                    userName = user.UserName,
                    contactNo = user.ContactNo,
                    emailID = user.EmailID,
                    expiryDate = user.ExpiryDate,
                    accessType = user.AccessType,
                    roleIDs = user.RoleIDs,
                    locationIDs = user.LocationIDs,
                    isDirectAccess = user.IsDirectAccess,
                    selectedMenus = user.SelectedMenuRights
                }
            });
        }

        [HttpPost]
        [Authorize(Policy = "Users")]
        public IActionResult AddUser(UserSaveViewModel model)
        {
            try
            {
                string ModifiedBy = HttpContext.Session.GetString("UserName") ?? "System";

                string result = _user.AddUser(model, ModifiedBy);

                if (result == "1")
                {
                    return Json(new { success = true, message = "User added successfully!" });
                }
                else if (result == "0")
                {
                    return Json(new { success = false, message = "User Email ID already exists." });
                }
                else
                {
                    return Json(new { success = false, message = "Error: " + result });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

    }
}
