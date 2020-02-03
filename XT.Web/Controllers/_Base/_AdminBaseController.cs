using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using XT.BusinessService;
using XT.Model;
using XT.Web.External;
using PagedList;
using PagedList.Mvc;
using XT.Web.Models;
using OfficeOpenXml;
using XT.Web.External.MVCAttributes;
using System.Globalization;

namespace XT.Web.Controllers
{
    [XTAuthorize]//Không để lên đây vì 1 controller chỉ sử dụng được 1 authorize
    public class AdminBaseController : BaseController
    {
        public int PAGE_SIZE = 20;//5;
        public int PAGE_SIZE_LARGE = 10;
        public int PAGE_SIZE_LARGE_20 = 20;
        public int PAGE_SIZE_LARGE_100 = 100;

        public const string PREFIX_STUDENT = "Student";
        public const string PREFIX_NONSTUDENT = "NonStudent";

        //public AdminBaseController()
        //{
        //    XT.Web.External.CultureHelper.Lang_Id = (int)LanguageEnum.vi;
        //}

        #region ManageModel
        public ActionResult ManageModel(IEntity model, int? page,
            string modal_size = "",
            string title = "",
            string breadcrumbpartial_name = "",
            string filterpartial_name = "",
            SearchModelEnum filterSearch = SearchModelEnum.ByName,
            IEnumerable<IEntity> list = null, 
            string entityName = "", string entityFilter = "",
            int currentParentId = 0, 
            string currentParentName = "", 
            IEntity currentParentModel = null,
            bool noPaging = false, 
            bool noSearchBox = false,
            bool canAdd = true,
            string script = "")
        {
            int pageSize = PAGE_SIZE;// PAGE_SIZE_LARGE_20;
            int pageNumber = (page ?? 1);

            //entity
            var entity = model.GetType().Name;
            var _entityFilter = entityFilter;
            if (entityFilter == "")
            {
                entityFilter = entity;
            }
            if (entityName == "")
            {
                entityName = model.GetType().Name;
            }

            //filter
            var FILTERBYNAME = "FilterByName";
            var filterAction = FILTERBYNAME;
            if (_entityFilter != "")//filter khác
                filterAction = "Filter" + _entityFilter;
            else
                filterAction = "Filter" + entityFilter;
            if (filterpartial_name == "")
            {
                switch (filterSearch)
                {
                    //case SearchModelEnum.None:
                    //    {
                    //        filterpartial_name = "../Admin/_partial_Filter_None";
                    //        break;
                    //    }
                    case SearchModelEnum.None:
                        {
                            if (_entityFilter == "")
                                filterAction = FILTERBYNAME;
                            break;
                        }
                    case SearchModelEnum.ByName:
                        {
                            filterpartial_name = "../Admin/_partial_Filter_SearchName";
                            if (_entityFilter == "")
                                filterAction = FILTERBYNAME;
                            break;
                        }
                    //case SearchModelEnum.ByNameWithParent:
                    //    {
                    //        filterpartial_name = "../Admin/_partial_Filter_SearchName_WithParent";
                    //        break;
                    //    }
                    case SearchModelEnum.ByOthers:
                        {
                            filterpartial_name = "Filter_Partial/_partial_Filter_" + entityFilter;
                            break;
                        }
                }
            }
            else
            {
                filterpartial_name = "Filter_Partial/" + filterpartial_name;
            }
            var searchpartial_name = "_partial_Search_" + entity;

            //lstItems
            var lstItems = list;
            if (lstItems == null)
            {
                lstItems = IoCConfig.ServiceDynamic(entity).FindAllValid() as IEnumerable<IEntity>;
            }
            var count = lstItems.Count();
            if (noPaging)
            {
                pageSize = count;
            }
            pageSize = pageSize == 0 ? 1 : pageSize;

            //ViewBag
            ViewBag.Entity = entity;//ajax _partial_Search_Model
            ViewBag.EntityFilter = entityFilter;
            ViewBag.EntityName = entityName;//ajax _partial_Search_Model
            ViewBag.NoPaging = noPaging;//ajax _partial_Search_Model

            ViewBag.NewEntity = model;//(IEntity)Activator.CreateInstance(model.GetType());// new Course();
            ViewBag.Title = title;
            ViewBag.FilterAction = filterAction;
            ViewBag.FilterPartial = filterpartial_name;
            ViewBag.SearchPartial = searchpartial_name;
            ViewBag.BreadCrumbPartial = breadcrumbpartial_name;
            ViewBag.PageSize = pageSize;
            ViewBag.ModalSize = modal_size;
            ViewBag.CurrentParentId = currentParentId;
            ViewBag.CurrentParentName = currentParentName;
            ViewBag.CurrentParentModel = currentParentModel;
            ViewBag.CanAdd = canAdd;
            ViewBag.NoSearchBox = noSearchBox;
            ViewBag.IsAjax = false;
            ViewBag.HasData = count > 0;
            
            ViewBag.Script = script;

            //The method 'Skip' is only supported for sorted input in LINQ to Entities. The method 'OrderBy' must be called before the method 'Skip'.
            return View("../Admin/ManageModel", lstItems.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult AddModel(dynamic model, Func<object, bool> pred = null)
        {
            if (ModelState.IsValid)
            {
                var managementItem = IoCConfig.Invoke_EntityManagementService(model);

                var adding_model = model.ToModel();
                //nếu gọi code trong hàm cha (AddDeal) thì phải cast sang kiểu con (Deal)
                //vd: var adding_model = model.ToModel() as Deal;
                if (adding_model == null)
                {
                    var error_msg = model.ErrorMessage != null ? model.ErrorMessage : "";
                    return Error(error_msg);
                }

                //return managementItem.Add(adding_model);
                var res = managementItem.Add(adding_model);
                if (pred != null)
                {
                    pred(adding_model);
                }

                return res;
            }

            return Error();
        }

        public ActionResult EditModel(dynamic model, Func<object, bool> pred = null)//model is new
        {
            if (ModelState.IsValid)
            {
                var managementItem = IoCConfig.Invoke_EntityManagementService(model);
                var service = managementItem.GetService();
                var old = service.FindById(model.Id);
                if (old == null)
                {
                    return ErrorNotExist();
                }
                var adding_model = model.ToModel(old);
                //nếu gọi code trong hàm cha (AddDeal) thì phải cast sang kiểu con (Deal)
                //vd: var adding_model = model.ToModel() as Deal;
                if (adding_model == null)
                {
                    var error_msg = model.ErrorMessage != null ? model.ErrorMessage : "";
                    return Error(error_msg);
                }

                //return managementItem.CheckAndEdit(adding_model);//update new to old
                var res = managementItem.CheckAndEdit(adding_model);
                if (pred != null)
                {
                    pred(adding_model);
                }

                return res;
            }

            return Error();
        }

        public ActionResult DeleteModel(int id)
        {
            var modelName = this.ControllerContext.RouteData.Values["action"].ToString().Replace("Delete", "");
            var managementItem = IoCConfig.Invoke_EntityManagementServiceByName(modelName);

            //Không sửa
            return managementItem.CheckAndDelete(id);
        }

        public ActionResult ReturnPartialView(
            string entity,
            IEnumerable<IEntity> list, int pageNumber, int pageSize,
            string entityFilter = "", bool noPaging = false)
        {
            if (ViewBag.Entity == null)
                ViewBag.Entity = entity;
            if (entityFilter == "")
                entityFilter = entity;
            if (ViewBag.EntityFilter == null)
                ViewBag.EntityFilter = entityFilter;
            if (ViewBag.EntityName == null)
                ViewBag.EntityName = entity;
            if (ViewBag.NoPaging == null)
                ViewBag.NoPaging = noPaging;
            ViewBag.IsAjax = true;

            var view = "../Admin/_partial_Search_Model";

            if (list == null)
            {
                return PartialView(view, list);
            }
            return PartialView(view, ((IEnumerable<IEntity>)list).ToPagedList(pageNumber, pageSize));
        }

        [HttpPost]
        public ActionResult FilterByName(string entity, string entityFilter, int? page, 
            string Model_Name = "", string modal_size = "", int parentId = 0)
        {
            int pageSize = PAGE_SIZE;
            int pageNumber = (page ?? 1);
            var service = IoCConfig.ServiceDynamic(entity);
            var list = service.FindByName(Model_Name, parentId) as IEnumerable<IEntity>;

            ViewBag.ModalSize = modal_size;

            return ReturnPartialView(entity, list, pageNumber, pageSize, entityFilter);
        }
        #endregion

        #region Import
        protected string GetCellValue(ExcelWorksheet workSheet, int row, int col, bool isText = true)
        {
            object val = null;
            if (isText)
            {
                val = workSheet.Cells[row, col].Text;
            }
            else
            {
                val = workSheet.Cells[row, col].Value;
            }
            return val != null ? val.ToString().Trim() : "";
            //object val = workSheet.Cells[row, col].Value;
            //if (val == null || val.GetType() == typeof(string))
            //{
            //    val = workSheet.Cells[row, col].Text;
            //    return val != null ? val.ToString().Trim() : "";
            //}
            //else
            //{
            //    return val;
            //}

            //return val == null ? "" : val;
        }

        protected object GetCellValue_Number(ExcelWorksheet workSheet, int row, int col, bool isText = true)
        {
            object val = workSheet.Cells[row, col].Value;
            return val == null ? "0" : val;

            //return GetCellValue(workSheet, row, col, isText);
            //object val = workSheet.Cells[row, col].Value;

            //return val == null ? 0 : val;
        }

        //private string GetCellValue_Number(ExcelWorksheet workSheet, int row, int col, bool isText = true)
        //{
        //    var val = GetCellValue(workSheet, row, col, true);
        //    if (!string.IsNullOrEmpty(val))
        //        return val;//.Replace(",", "").Replace(".", ",");

        //    return "0";
        //}

        //private string GetCellValue_Number_Interget(ExcelWorksheet workSheet, int row, int col, bool isText = true)
        //{
        //    return GetCellValue_Number(workSheet, row, col, true).Replace(",", "").Replace(".", "");
        //}

        protected int GetCellValue_Int(ExcelWorksheet workSheet, int row, int col, bool isText = true)
        {
            var val = GetCellValue_Number(workSheet, row, col, isText);
            try
            {
                return Convert.ToInt32(val);
            }
            catch (Exception ex)
            {
                return 0;
            }
            //if (val.GetType() == typeof(string))
            //{
            //    string val_num = val.ToString().Trim();
            //    int num = 0;
            //    int.TryParse(val_num, out num);


            //    return num;
            //}
            //else
            //{
            //    return (int)val;
            //}
            //int val = 0;
            //int.TryParse(GetCellValue_Number_Interget(workSheet, row, col, isText), out val);

            //return val;
        }

        protected long GetCellValue_Long(ExcelWorksheet workSheet, int row, int col, bool isText = true)
        {
            var val = GetCellValue_Number(workSheet, row, col, isText);
            try
            {
                return Convert.ToInt64(val);
            }
            catch (Exception ex)
            {
                return 0;
            }
            //return (long)GetCellValue_Number(workSheet, row, col, isText);
            //long val = 0;
            //long.TryParse(GetCellValue_Number_Interget(workSheet, row, col, isText), out val);

            //return val;
        }

        protected float GetCellValue_Float(ExcelWorksheet workSheet, int row, int col, bool isText = true)
        {
            var val = GetCellValue_Number(workSheet, row, col, isText);
            try
            {
                return Convert.ToSingle(val);
            }
            catch (Exception ex)
            {
                return 0;
            }
            //return (float)GetCellValue_Number(workSheet, row, col, isText);
            //float val = 0;
            //string num = GetCellValue_Number(workSheet, row, col, isText);
            //float.TryParse(num, out val);

            //return val;
        }

        protected DateTime? GetCellValue_DateTime(ExcelWorksheet workSheet, int row, int col, bool isText = true, string format = "")
        {
            var date = DateTime.Today;
            var date_str = GetCellValue(workSheet, row, col, isText);
            if (DateTime.TryParse(date_str, out date))
            {
                return date;
            }

            return null;
        }

        /// <summary>
        /// If null return DateTime.Today
        /// </summary>
        /// <param name="workSheet"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="isText"></param>
        /// <returns></returns>
        protected DateTime GetCellValue_DateTime_Full(ExcelWorksheet workSheet, int row, int col, bool isText = true, string format = "")
        {
            var date = GetCellValue_DateTime(workSheet, row, col, isText, format);
            return date.HasValue ? date.Value : DateTime.Today;
        }

        protected string GetCellValue_String(ExcelWorksheet workSheet, int row, int col, bool isText = true)
        {
            var str = GetCellValue(workSheet, row, col, isText);
            str = str.Split(':').Last().Trim();

            return str;
        }
        #endregion Import
    }
}
