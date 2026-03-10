using DF_WebModule.Classes;
using DF_WebModule.DBModel;
using DF_WebModule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DF_WebModule.FEnum;


namespace DF_WebModule.Controllers
{
    [Authorize]
    public class UserRegistrationController : Controller
    {
        protected SymlLogs _log;
       public UserRegistrationController()
        {
            _log = new SymlLogs(this.GetType().Name);
        }
        Engineering_DBEntities entity = new Engineering_DBEntities();
       
        // GET: UserRegistration
        public ActionResult Index()
        {
            try
            {
                _log.LogDebugMessage($"{nameof(this.Index)}, {LoggerMessage.ProcessStarted}, {DateTime.Now}");

                List<UserDetailsModel> users = new List<UserDetailsModel>();
                var list = entity.UserDetails.ToList();
                if (list.Count > 0)
                {
                    foreach (var item in list)
                    {
                        users.Add(new UserDetailsModel
                        {
                            Id = item.Id,
                            Name = item.Name,
                            Mobile = item.MobileNo,
                            Email = item.Email_Id,
                            Password = item.Password,
                            Role = item.Master_Role.Role,
                            RoleId = item.RoleId,
                            UserName = item.Username,

                        });

                    }
                }
                return View(users);
            }
            catch (Exception ex)
            {
                _log.LogError($"Error Thrown during: {ex.Message}, { nameof(this.Index)}, {DateTime.Now},{ex.StackTrace}");

                return RedirectToAction("RedirectError", "Error");
            }
            finally
            {
                _log.LogDebugMessage($"{nameof(this.Index)}, {LoggerMessage.ProcessEnded}, {DateTime.Now}");

            }

        }

        public ActionResult AddUser()
        {
            try
            {
                _log.LogDebugMessage($"{nameof(this.AddUser)}, {LoggerMessage.ProcessStarted}, {DateTime.Now}");

                UserDetailsModel user = new UserDetailsModel();
                List<Menus> menus = new List<Menus>();
                List<Role> roles = new List<Role>();
                var documentlist = ((EnumDocumentType[])System.Enum.GetValues(typeof(EnumDocumentType))).Select(g => new DocumentListModel
                {
                    Id = (int)g,
                    Name = g.ToString()
                }).ToList();                
                var role = entity.Master_Role.Where(x => x.Id != 1).ToList();
                foreach (var item in role)
                {
                    roles.Add(new Role
                    {
                        Id = item.Id,
                        Name = item.Role
                    });
                }
              
                user.menus = menus;
                user.roles = roles;
                user.documentLists = documentlist;
                return PartialView(user);
            }
            catch(Exception ex)
            {
                _log.LogError($"Error Thrown during: {ex.Message}, { nameof(this.AddUser)}, {DateTime.Now},{ex.StackTrace}");

                return RedirectToAction("RedirectError", "Error");
            }
            finally
            {
                _log.LogDebugMessage($"{nameof(this.AddUser)}, {LoggerMessage.ProcessEnded}, {DateTime.Now}");

            }
        }

        public ActionResult AddNewUser(UserDetailsModel item)
        {
            try
            {
                _log.LogDebugMessage($"{nameof(this.AddNewUser)}, {LoggerMessage.ProcessStarted}, {DateTime.Now}");

                if (ModelState.IsValid)
                {
                    
                    var userdetails = entity.UserDetails.Where(x => x.Username.Equals(item.UserName)).FirstOrDefault();
                    if (userdetails != null)
                    {
                        return Json(new { success = false, responseText = "Provided User Name already exist. please user diferent username" }, JsonRequestBehavior.AllowGet);
                    }
                    UserDetail userDetail = new UserDetail
                    {
                        Name = item.Name,
                        Username = item.UserName,
                        MobileNo = item.Mobile,
                        Email_Id = item.Email,
                        Password = item.Password,
                        RoleId = item.RoleId,
                        CreatedOn = DateTime.Now.ToString("dd/MM/yyyy hh:mm"),
                    };

                    entity.UserDetails.Add(userDetail);
                    entity.SaveChanges();
                   
                    return Json(new { success = true, responseText = "User has been added into the system, please click okay to return to the Item list" }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { success = true, responseText = "Something Missing in your provided detail. please check" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                _log.LogError($"Error Thrown during: {ex.Message}, { nameof(this.AddNewUser)}, {DateTime.Now},{ex.StackTrace}");

                return RedirectToAction("RedirectError", "Error");
            }
            finally
            {
                _log.LogDebugMessage($"{nameof(this.AddNewUser)}, {LoggerMessage.ProcessEnded}, {DateTime.Now}");

            }
        }


        public ActionResult EditUser(int ID)
        {
            try
            {
                _log.LogDebugMessage($"{nameof(this.EditUser)}, {LoggerMessage.ProcessStarted}, {DateTime.Now}");

                List<Role> roles = new List<Role>();
                List<Menus> menus = new List<Menus>();
                List<DocumentListModel> documentListModels = new List<DocumentListModel>();

                var item = entity.UserDetails.FirstOrDefault(x => x.Id == ID);

                if (item != null)
                {
                   

                    var role = entity.Master_Role.ToList();
                    foreach (var items in role)
                    {
                        roles.Add(new Role
                        {
                            Id = items.Id,
                            Name = items.Role
                        });
                    }
                 
                    UserDetailsModel user = new UserDetailsModel
                    {
                       
                        Name = item.Name,
                        Role = item.Master_Role.Role,
                        RoleId = item.RoleId,
                        Email = item.Email_Id,
                        Mobile = item.MobileNo,
                        UserName = item.Username,
                        Password = item.Password,

                    };                   
                    user.menus = menus;
                    user.roles = roles;
                    user.documentLists = documentListModels;
                    return PartialView(user);
                }
                return PartialView();
            }
            catch (Exception ex)
            {
                _log.LogError($"Error Thrown during: {ex.Message}, { nameof(this.EditUser)}, {DateTime.Now},{ex.StackTrace}");

                return RedirectToAction("RedirectError", "Error");
            }
            finally
            {
                _log.LogDebugMessage($"{nameof(this.EditUser)}, {LoggerMessage.ProcessEnded}, {DateTime.Now}");

            }
        }


        public ActionResult EditUserDetail(UserDetailsModel item)
        {
            try
            {
                _log.LogDebugMessage($"{nameof(this.EditUserDetail)}, {LoggerMessage.ProcessStarted}, {DateTime.Now}");

                if (ModelState.IsValid)
                {
                   
                    var partno = entity.UserDetails.Where(x => x.Id.Equals(item.Id)).FirstOrDefault();
                    if (partno != null)
                    {                       
                        partno.Name = item.Name;
                        partno.MobileNo = item.Mobile;
                        partno.Email_Id = item.Email;
                        partno.Password = item.Password;
                        partno.Username = item.UserName;
                        partno.RoleId = item.RoleId;
                       
                        var userid = Convert.ToInt32(partno.Id);
                       
                        entity.SaveChanges();

                    };
                    

                    return Json(new { success = true, responseText = "User Detail has been updated succesfully into the system, please click okay to return to the Item list" }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { success = true, responseText = "Something missing please check" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                _log.LogError($"Error Thrown during: {ex.Message}, { nameof(this.EditUserDetail)}, {DateTime.Now},{ex.StackTrace}");

                return RedirectToAction("RedirectError", "Error");
            }
            finally
            {
                _log.LogDebugMessage($"{nameof(this.EditUser)}, {LoggerMessage.ProcessStarted}, {DateTime.Now}");

            }
        }

    }
}