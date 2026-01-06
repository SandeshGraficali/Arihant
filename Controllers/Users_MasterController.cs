using Arihant.Models.Client;
using Arihant.Models.Company_Master;
using Arihant.Models.IP_Master;
using Arihant.Models.Rights_Master;
using Arihant.Models.User_Master;
using Arihant.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;

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
        public JsonResult SaveClient(string jsonData)
        {
            string ModifiedBy = HttpContext.Session.GetString("UserName") ?? "";
            string result = _user.AddClientDetails(jsonData , ModifiedBy);

            if (result == "1")
                return Json(new { success = true, message = "User Added successfully!" });
            else
                return Json(new { success = false, message = result });
           
        }

        public IActionResult AllClientMasterList()
        {
            List<ClientProfileModel> list = _user.GetAllClientMasters();

            return View(list);
        }


        [HttpPost]
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
        public IActionResult ClientMaster(int? id)
        {
           ViewBag.countrylist = _user.GetC_MasterList();
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
        public IActionResult UpdateCompanyProfile(string data)
        {
            try
            {
                string createdBy = HttpContext.Session.GetString("UserName") ?? "System";
                string result = _user.UpdateCompanyDetails(data , createdBy);
             
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


        public IActionResult GetCompanyList()
        {
            List<CompanyProfileModel>  list = _user.GetAllCompanyMasters();    
            return View(list);
        }

 
        [HttpGet]
        public IActionResult EditCompany(int id)
        {
            ViewBag.countrylist = _user.GetC_MasterList();
            var list = _user.GetAllCompanyMasters();
            var companyDetail = list.FirstOrDefault(x => x.CompanyID == id);

            if (companyDetail == null) return NotFound();

            return View("CompanyForm", companyDetail); 
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
        public IActionResult SaveCompanyProfile(string data)
        {
            try
            {
                string createdBy = HttpContext.Session.GetString("UserName") ?? "System";
                string result= _user.SaveCompanyDetails(data , createdBy);

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

        public IActionResult CompanyMaster()
        {
            ViewBag.countrylist = _user.GetC_MasterList();
            return View("CompanyForm", new CompanyProfileModel()); 
        }

        public IActionResult CompanyForm()
        {
          
            return View();
        }




        [HttpPost]
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
        public JsonResult UpdateRolePermissions([FromBody] SaveRoleDTO model)
        {
            try
            {
                string createdBy = HttpContext.Session.GetString("UserName") ?? "System";
                string menuIdsString = model.MenuIDs != null ? string.Join(",", model.MenuIDs) : "";
                string result = _user.UpdateRoleWithRights(model.RoleName, menuIdsString, createdBy , model.RoleID.ToString());
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
        public JsonResult SaveRole(string RoleName, string MenuIDs)
        {
            try
            {
                string createdBy = HttpContext.Session.GetString("UserName") ?? "System";        
                string result = _user.InsertRoleWithRights(RoleName, MenuIDs, createdBy);
                if (result == "1")
                {
                    return Json(new { success = true, message = "Role Added successfully!"  , result = result });
                }
                else if (result == "0")
                {
                    return Json(new { success = false, message = "Role is already exists."  , result  = result });
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
        public ActionResult EditRole(int id)
        {
            List<MenuRightViewModel> allMenus = _user.GetMenuRights();
            List<DTO_MenuRight> allAssigned = _user.ManageRights();
            var particularRights = allAssigned.Where(x => x.RoleID == id).ToList();
            ViewBag.RoleName = particularRights.FirstOrDefault()?.RoleName ?? "New Role";
            ViewBag.RoleID = id;

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
        public IActionResult Role_Master()
        {
            List<MenuRightViewModel> allRights = _user.GetMenuRights();
            var groupedRights = allRights.GroupBy(x => x.Module).ToList();

            return View(groupedRights);
        }

        public IActionResult Manage_Role_Master()
        {
            List<DTO_MenuRight> allRights = _user.ManageRights();

            return View(allRights);
        }

        [HttpPost]
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
        public IActionResult GetUsersByLocation(int locationId)
        {
            List<IP_Users> users = _user.GetIPMasterUserList(locationId);

            return Json(new { success = true, users });
        }


        [HttpPost]
        public JsonResult AddIPMaster(IP_Master_Model model)
        {

            try
            {
                string ModifiedBy = HttpContext.Session.GetString("UserName") ?? "";
                 string result = _user.AddIPAddress(model , ModifiedBy);
               
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


        public IActionResult IP_Master()
        {
            List<IP_Master_Display> IP_Masters = _user.GetIPMasterList();
            return View(IP_Masters);
        }

        public IActionResult Dashboard()
        {
            return View();
        }
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
        public IActionResult UpdateUser([FromBody] UserUpdateModel model)
        {
            try
            {
                if (model == null )
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
                    expiryDate = user.ExpiryDate?.ToString("yyyy-MM-dd"),
                    accessType = user.AccessType,
                    roleIDs = user.RoleIDs, 
                    locationIDs = user.LocationIDs, 
                    isDirectAccess = user.IsDirectAccess,
                    selectedMenus = user.SelectedMenuRights
                }
            });
        }

        [HttpPost]
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
