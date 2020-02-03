using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XT.Model;
using XT.BusinessService;
using XT.Web.External;
using System.Web.Script.Serialization;
using System.IO;
using PagedList;
using PagedList.Mvc;
using XT.Web.Models;
using XT.Web.External.MVCAttributes;
using System.Configuration;

namespace XT.Web.Controllers
{
    [XTAuthorizeCenterHead]
    public partial class ManageCompanyController : AdminBaseController
    {
        #region Company_Type
        public ActionResult ManageCompanyType(int? page)
        {
            return ManageModel(new Company_Type(), page, entityName: "Company Type", filterSearch: SearchModelEnum.None);
        }

        [HttpPost]
        public ActionResult AddCompany_Type(Company_Type model)
        {
            return AddModel(model);
        }

        [HttpPost]
        public ActionResult EditCompany_Type(Company_Type model)
        {
            return EditModel(model);
        }

        [HttpPost]
        public ActionResult DeleteCompany_Type(int id)
        {
            return DeleteModel(id);
        }
        #endregion Company_Type

        #region Company
        private IEnumerable<Company> GetCompaniesByType(ref Company_Type currentParent, int id = 0)
        {
            IEnumerable<Company> list = null;
            if (id != 0)
            {
                currentParent = IoCConfig.Service<ICompany_TypeService>().FindById(id);
                if (currentParent != null && currentParent.IsValid())
                {
                    list = currentParent.Companies;
                }
            }
            else
            {
                list = IoCConfig.Service<ICompanyService>().FindAllValid();
            }

            return list;
        }

        public ActionResult ManageCompany(int? page, int id = 0)//id: company type id
        {
            var currentParent = new Company_Type { Status = (int)EntityStatus.Visible };
            var list = GetCompaniesByType(ref currentParent, id);
            if (currentParent == null || !currentParent.IsValid())
                return RedirectToError("Class is not valid!");

            //return ManageModel(new Company
            //{
            //    Company_Type_Id = currentParent.Id
            //}, page, list: list,
            //currentParentId: currentParent.Id, currentParentName: currentParent.Company_Type_Name,
            //filterpartial_name: "_partial_Filter_SearchModel");
            return ManageModel(new Company
            {
                Company_Type_Id = currentParent.Id
            }, page, list: list,
            entityName: "Center",
            currentParentId: currentParent.Id, currentParentName: currentParent.Company_Type_Name,
            filterSearch: SearchModelEnum.ByNameWithParent);
        }

        [HttpPost]
        public ActionResult AddCompany(CompanyModel model)
        {
            return AddModel(model);
        }

        [HttpPost]
        public ActionResult EditCompany(CompanyModel model)
        {
            return EditModel(model);
        }

        [HttpPost]
        public ActionResult DeleteCompany(int id)
        {
            return DeleteModel(id);
        }

        [HttpPost]
        public ActionResult FilterCompany(//dùng model để generic
            int? page,
            int? page_size,
            int pageChange,
            string entity,

            int id = 0,

            string Model_Name = "",
            string sort_target = "",
            bool sort_rank = false)
        {
            int pageSize = (page_size ?? PAGE_SIZE_LARGE_20);
            int pageNumber = (page ?? 1);
            if (pageChange == 0)
                pageNumber = 1;

            var currentParent = new Company_Type { Status = (int)EntityStatus.Visible };
            var items = GetCompaniesByType(ref currentParent, id);
            if (currentParent == null || !currentParent.IsValid())
                return RedirectToError("Company Type is not valid!");

            //var items = IoCConfig.Service<IClassService>().FindAllValid();//sửa
            if (Model_Name != "")
            {
                pageNumber = 1;
                var name = Model_Name;
                name = name.ToLower().Convert_Chuoi_Khong_Dau();
                items = items.Where(c => c.Company_Name.ToLower().Convert_Chuoi_Khong_Dau().Contains(name));
            }
            else//sửa
            {
                
            }

            ViewBag.sort_target = sort_target;
            ViewBag.sort_rank = sort_rank ? "asc" : "desc";

            return ReturnPartialView(entity, items, pageNumber, pageSize);
        }
        #endregion Company

        #region Resource
        private IEnumerable<Resource> GetResourcesByCompany(ref Company currentParent, int id = 0)
        {
            IEnumerable<Resource> list = null;
            if (id != 0)
            {
                currentParent = IoCConfig.Service<ICompanyService>().FindById(id);
                if (currentParent != null && currentParent.IsValid())
                {
                    list = currentParent.Resources;
                }
            }
            else
            {
                list = IoCConfig.Service<IResourceService>().FindAllValid();
            }

            return list;
        }


        public ActionResult ManageResource(int? page, int id = 0)//id: company id
        {
            var currentParent = new Company { Status = (int)EntityStatus.Visible };
            var list = GetResourcesByCompany(ref currentParent, id);
            if (currentParent == null || !currentParent.IsValid())
                return RedirectToError("Class is not valid!");

            //return ManageModel(new Resource
            //{
            //    Company_Id = currentParent.Id
            //}, page, list: list,
            //currentParentId: currentParent.Id, currentParentName: currentParent.Company_Name,
            //filterpartial_name: "_partial_Filter_SearchModel");
            return ManageModel(new Resource
            {
                Company_Id = currentParent.Id
            }, page, list: list,
            currentParentId: currentParent.Id, currentParentName: currentParent.Company_Name,
            filterSearch: SearchModelEnum.ByOthers);
        }

        [HttpPost]
        public ActionResult AddResource(Resource model)
        {
            return AddModel(model);
        }

        [HttpPost]
        public ActionResult EditResource(Resource model)
        {
            return EditModel(model);
        }

        [HttpPost]
        public ActionResult DeleteResource(int id)
        {
            return DeleteModel(id);
        }

        [HttpPost]
        public ActionResult FilterResource(//dùng model để generic
            int? page,
            int? page_size,
            int pageChange,
            string entity,

            int id = 0,
            int Company_Id = 0,

            string Model_Name = "",
            string sort_target = "",
            bool sort_rank = false)
        {
            int pageSize = (page_size ?? PAGE_SIZE_LARGE_20);
            int pageNumber = (page ?? 1);
            if (pageChange == 0)
                pageNumber = 1;

            var currentParent = new Company { Status = (int)EntityStatus.Visible };
            var items = GetResourcesByCompany(ref currentParent, id);
            if (currentParent == null || !currentParent.IsValid())
                return RedirectToError("Class is not valid!");

            //var items = IoCConfig.Service<IClassService>().FindAllValid();//sửa
            if (!string.IsNullOrEmpty(Model_Name))
            {
                pageNumber = 1;
                var name = Model_Name;
                name = name.ToLower().Convert_Chuoi_Khong_Dau();
                items = items.Where(c => c.Resource_Name.ToLower().Convert_Chuoi_Khong_Dau().Contains(name));
            }
            //others
            if (Company_Id > 0)
            {
                items = items.Where(r => r.Company_Id == Company_Id);
            }


            ViewBag.sort_target = sort_target;
            ViewBag.sort_rank = sort_rank ? "asc" : "desc";

            return ReturnPartialView(entity, items, pageNumber, pageSize);
        }
        #endregion Resource

        #region Employee

        #region ManageEmployee
        private IEnumerable<User_Profile> GetEmployees()
        {
            return IoCConfig.Service<IUserProfileService>()
                .FindAllValidByCriteria(e => 
                    e.Role_Type_Id != (int)RoleTypeEnum.Admin)
                .OrderByDescending(a => a.Created_Date);
        }

        public ActionResult ManageEmployee(int? page)
        {
            return ManageModel(new User_Profile { 
                User_Type_Id = (int)UserTypeEnum.User
            }, page,
                filterSearch: SearchModelEnum.ByOthers,
                list: GetEmployees(),
                entityName: "Employee");
        }

        [HttpPost]
        public ActionResult AddUser_Profile(EmployeeModel model)
        {
            model.User_Profile_Avatar = AppSettings.DefaultAccountAvatar;

            var user = (User_Profile)model.ToModel();
            user = AddUserProfileToDB(user);

            return user == null ? Error() : Success();
        }

        [HttpPost]
        public ActionResult EditUser_Profile(User_Profile model)
        {
            model = AddUserProfileToDB(model);

            return model == null ? Error() : Success();
        }

        [HttpPost]
        public ActionResult DeleteUser_Profile(int id)
        {
            var service = IoCConfig.Service<IUserProfileService>();
            if (service.DeleteUser(id))
            {
                return Success();
            }

            return ErrorNotExist();
        }

        [HttpPost]
        public ActionResult FilterUser_Profile(//dùng model để generic
            int? page,
            int? page_size,
            int pageChange,
            string entity,

            int Role_Type_Id = 0,
            int Company_Id = 0,

            string Model_Name = "",
            string sort_target = "",
            bool sort_rank = false)
        {
            int pageSize = (page_size ?? PAGE_SIZE_LARGE_20);
            int pageNumber = (page ?? 1);
            if (pageChange == 0)
                pageNumber = 1;

            var items = GetEmployees();

            if (Model_Name != "")
            {
                pageNumber = 1;
                var name = Model_Name;
                name = name.ToLower().Convert_Chuoi_Khong_Dau();

                items = items.Where(c => c.Name.ToLower().Convert_Chuoi_Khong_Dau().Contains(name));
            }
            if (Role_Type_Id > 0)
            {
                items = items.Where(c => c.Role_Type_Id == Role_Type_Id);
            }
            if (Company_Id > 0)
            {
                items = items.Where(c => c.HasCompany(Company_Id));
            }

            //items = items.OrderByDescending(p => p.Created_Date);

            ViewBag.sort_target = sort_target;
            ViewBag.sort_rank = sort_rank ? "asc" : "desc";

            return ReturnPartialView(entity, items, pageNumber, pageSize);
        }
        #endregion

        #region User_Company
        public ActionResult ManageUser_Company(int? page, int id)
        {
            var user = IoCConfig.Service<IUserProfileService>().FindById(id);
            if (user == null || !user.IsValid())
                return RedirectToError();

            //return ManageModel(new Faculty_Module { Faculty_Id = id }, null,
            //    hasFilter: false,
            //    list: faculty.Faculty_Modules, entityName: "Skill");
            return ManageModel(new User_Company { User_Id = id }, page,
                list: user.User_Companies_List, entityName: "Company",
                currentParentName: user.Name,
                currentParentId: id,
                filterSearch: SearchModelEnum.None);
        }

        [HttpPost]
        public ActionResult AddUser_Company(User_Company model)
        {
            return AddModel(model);
        }

        [HttpPost]
        public ActionResult EditUser_Company(User_Company model)
        {
            return EditModel(model);
        }

        [HttpPost]
        public ActionResult DeleteUser_Company(int id)
        {
            return DeleteModel(id);
        }
        #endregion

        #endregion

        #region AddUserProfileToDB
        private User_Profile AddUserProfileToDB(User_Profile user)
        {
            var service = IoCConfig.Service<IUserProfileService>();
            //Check exist by phone
            var existed = service.FindValidByCriteria(
                a => a.User_Profile_Phone != null && a.User_Profile_Phone.Equals(user.User_Profile_Phone)
                    && a.User_Profile_Name != null && a.User_Profile_Name.Equals(user.User_Profile_Name)
                    && a.Id != user.Id);
            if (existed != null)
            {
                SetCustomError("Thông tin tài khoản không tồn tại hoặc bị trùng tên và số điện thoại!");
                return null;//Trung phone va trung name thi khong cho add/edit
            }

            if (user.Id != 0)//Edit
            {
                user = service.Update(user);

                //update Account by User Profile
                IoCConfig.Service<IAccountService>().UpdateAccount(user);
            }
            else//Add
            {
                //add user
                user = service.Add(user);

                //add account
                var accountService = IoCConfig.Service<IAccountService>();
                var account = accountService.Create(user.Email,
                    PasswordEncryptManager.EncryptPassword(AppSettings.DefaultPassword),
                    user.Obj_Id, user);
                account.HasSetPassword = true;
                account.Status = user.Status;

                //Add new account
                account = accountService.Add(account);
                if (account != null)
                {
                    //accountService.Active(account);
                    //GetService().Active(u);//user profile khi add đã active sẵn rồi

                    //start sending email
                    //EmailHelper.SendMail_RegisterActiveSuccess(this.ControllerContext, account);
                    //EmailHelper.SendMail_InformUser(this.ControllerContext, user);
                    //end sending email
                }
            }

            return user;
        }

        private User_Profile AddUserToDB(User_ProfileModel model)
        {
            //update User Profile
            var service = IoCConfig.Service<IUserProfileService>();

            var id = model.Id;
            var user = new User_Profile();
            if (id != 0)
            {
                user = service.FindById(id);
                if (user == null)
                {
                    return null;
                }
            }
            user = model.ToModel(user);
            if (user == null)
            {
                SetCustomError(model.ErrorMessage);
                return null;//Trung phone va trung name thi khong cho add/edit
            }

            return AddUserProfileToDB(user);
        }
        #endregion ChangeProfileInfoAccount
    }
}