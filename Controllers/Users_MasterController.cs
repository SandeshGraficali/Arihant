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
                    return Json(new { success = false, message = "Failed to save company profile. Please try again." });
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
                    return Json(new { success = false, message = "IPCould not delete role. It might be in use." });
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
        public JsonResult AddIPMaster(IP_Master_Model model)
        {

            try
            {
                string ModifiedBy = HttpContext.Session.GetString("UserName") ?? "";
                string result = _user.AddIPAddress(model , ModifiedBy);

                if (result == "1")
                {
                    return Json(new { success = true, message = "User details updated successfully!" });
                }
                else if (result == "0")
                {
                    return Json(new { success = false, message = "User ID already exists." });
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
        public JsonResult SaveIPMaster(IP_Master_Model model)
        {
            try
            {
                string ModifiedBy = HttpContext.Session.GetString("UserName") ?? "";
                string result = _user.UpdateIPAddress(model , ModifiedBy);

                if (result == "1")
                {
                    return Json(new { success = true, message = "User details updated successfully!" });
                }
                else if (result == "0")
                {
                    return Json(new { success = false, message = "User ID already exists." });
                }
                else
                {
                    return Json(new { success = false, message = "Error: " + result });
                }
            }
            catch(Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
                
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
        public JsonResult ActiveUser(string UserID)
        {
            try {
               string result= _user.ActiveUser(UserID);
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

        public IActionResult IP_Master()
        {
            List < IP_Master_Model > IP_Masters = _user.GetIPMasterList();
            return View(IP_Masters);
        }

        public IActionResult Dashboard()
        {
            return View();
        }
        public IActionResult User_Master_Dashboard()
        {
           
            ViewBag.IPList = _user.GetIPList();   
            List<IPViewModel_withIPS> result= _user.GetAllUsers();
            return View(result);
        }

        [HttpPost]
        public JsonResult UpdateUser(UserUpdateModel model)
        {
            try
            {

                if (string.IsNullOrEmpty(model.UserID))
                    return Json(new { success = false, message = "User ID is missing" });
                string ModifiedBy = HttpContext.Session.GetString("UserName") ?? "";

                string result = _user.UpdateUser(model , ModifiedBy);
             
                if (result == "1")
                {
                    return Json(new { success = true, message = "User details updated successfully!" });
                }
                else if (result == "0")
                {
                    return Json(new { success = false, message = "User ID already exists." });
                }
                else
                {
                    return Json(new { success = false, message = "Error: " + result });
                }

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Server Error: " + ex.Message });
            }
        }

        [HttpPost]
        public IActionResult AddUser(UserViewModel model)
        {
            try
            {
                string ModifiedBy = HttpContext.Session.GetString("UserName") ?? "";

                string result = _user.AddUser(model, ModifiedBy);

                if (result == "1")
                {
                    return Json(new { success = true, message = "User added successfully!" });
                }
                else if (result == "0")
                {
                    return Json(new { success = false, message = "User ID already exists." });
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
