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
using OfficeOpenXml;

namespace XT.Web.Controllers
{
    //[XTAuthorizeAdmin]//Không để lên đây vì 1 controller chỉ sử dụng được 1 authorize
    [XTAuthorizeFinance]
    public partial class ManageFinanceController : AdminBaseController
    {
        #region FeePlan
        public ActionResult ManageFeePlan(int? page)
        {
            return ManageModel(new FeePlan(), page,
                breadcrumbpartial_name: "_partial_BreadCrumb_FeePlan",
                script: "~/Scripts/Admin/ManageFeePlan");
        }

        [HttpPost]
        public ActionResult AddFeePlan(FeePlanModel model)
        {
            return AddModel(model);
        }

        [HttpPost]
        public ActionResult EditFeePlan(FeePlan model)
        {
            return EditModel(model);
        }

        [HttpPost]
        public ActionResult DeleteFeePlan(int id)
        {
            return DeleteModel(id);
        }
        #endregion FeePlan

        #region [XTAuthorizeMod] Import FeePlan
        private Student_FeePlan_Installment UpdateStudentFeeplanInstallment(
            Student_FeePlan_Installment detail, ExcelWorksheet ws, int row, int col, int year)
        {
            var Amount_Actual = GetCellValue_Int(ws, row, col + 1);
            if (Amount_Actual > 0)
            {
                var month = 8;
                var day = 1;
                var MONTH_ROW = 6;

                var month_row = GetCellValue(ws, MONTH_ROW, col);
                month_row = month_row.Replace("T", "").Replace("-Plan", "").Trim();
                month = int.Parse(month_row);
                Console.WriteLine("Month = " + month);

                var Amount_Planning = GetCellValue_Int(ws, row, col);
                var Date_Actual = GetCellValue_DateTime(ws, row, col + 2);
                var Date_Planning = new DateTime(year, month, day);

                detail.Amount_Planning = Amount_Planning;
                detail.Amount_Actual = Amount_Actual;
                detail.Date_Planning = Date_Planning;
                detail.Date_Actual = Date_Actual;
                detail.Installment_Status = (int)InstallmentStatusEnum.Finished;
            }

            return detail;
        }

        private FeePlan GetFeePlanByPriceAndType(IEnumerable<FeePlan> company_feeplans, int price, FeePlanTypeEnum type)
        {
            var count = company_feeplans.Count(f => f.FeePlan_Price == price);
            if (count == 1)
                return company_feeplans.FirstOrDefault(f => f.FeePlan_Price == price);// && f.FeePlan_Type == (int)feeplan_type);
            return company_feeplans.FirstOrDefault(f => f.FeePlan_Price == price && f.FeePlan_Type == (int)type);
        }

        [HttpPost]
        [XTAuthorizeMod]
        public ActionResult ImportFeePlan(HttpPostedFileBase file)
        {
            if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
            {
                if (!file.FileName.EndsWith("xlsx"))
                    return Error("File import phải là .xlsx!");

                using (var package = new ExcelPackage(file.InputStream))
                {
                    var currentSheet = package.Workbook.Worksheets;
                    var ws = currentSheet.First();
                    var noOfCol = ws.Dimension.End.Column;
                    var noOfRow = ws.Dimension.End.Row;

                    var DEFAULT_FEEPLAN = IoCConfig.Service<IFeePlanService>().FindAllValid().FirstOrDefault();
                    if (DEFAULT_FEEPLAN == null)
                    {
                        return null;// Error();
                    }
                    var YEAR_ROW = 2;
                    var MONTH_ROW = 7;
                    var START_DATA_ROW = 8;
                    var START_FEEPLAN_COL = 94;//CP
                    var START_YEAR = 2016;//2012

                    //data row
                    var STUDENT_ID = 4;
                    var NORMINAL_COURSE_FEE = 13;
                    var INVOICE_DATE = 6;
                    var REMARKS = 20;
                    var ACTUAL_COURSE_FEE = 16;
                    var DISCOUNT = 15;

                    //var company = IoCConfig.Service<ICompany_TypeService>().FindById(COMPANY_TYPE);
                    var center = IoCConfig.Service<ICompanyService>().FindById(CURRENT_COMPANY);
                    var company_feeplans = center.Company_Type.FeePlans.Valid();

                    //test feeplan
                    //ExcelPackage pck = new ExcelPackage();
                    //var ws_out = pck.Workbook.Worksheets.Add("FeePlan List");
                    var feeplan_list = new List<MyDictionary>();

                    var count = 0;

                    for (int row = START_DATA_ROW; row <= noOfRow; row++)
                    {
                        //debug
                        //if (row == 196)
                        //{
                        //    var ii = 0;
                        //}
                        try
                        {
                            var student_enroll = GetCellValue(ws, row, STUDENT_ID).ToLower();//Student Id
                            var student = IoCConfig.Service<IStudentService>().FindByCriteria(s => s.Student_EnrollNumber.ToLower() == student_enroll);
                            if(IsValidModel(student) && student.Student_FeePlan_List.Count() == 0)
                            {
                                var nominal_course_fee = GetCellValue_Int(ws, row, NORMINAL_COURSE_FEE);//Nominal Course Fee
                                var remarks = GetCellValue(ws, row, REMARKS);//Remarks

                                feeplan_list.Add(new MyDictionary { Count = nominal_course_fee, Name = remarks });

                                var feeplan_type = remarks == "Lumpsum" ? FeePlanTypeEnum.Lumpsum : FeePlanTypeEnum.Installment;
                                var feeplan = GetFeePlanByPriceAndType(company_feeplans, nominal_course_fee, feeplan_type);

                                if (IsValidModel(feeplan))
                                {
                                    var invoice_date = GetCellValue_DateTime(ws, row, INVOICE_DATE);//invoice_date
                                    if (!invoice_date.HasValue)
                                        invoice_date = student.Student_Application_Date;
                                    var actual_fee = GetCellValue_Int(ws, row, ACTUAL_COURSE_FEE);//actual_fee
                                    var discount = GetCellValue_Int(ws, row, DISCOUNT);//discount

                                    var student_feeplan = new Student_FeePlan
                                    {
                                        Student_Id = student.Id,
                                        FeePlan_Id = feeplan.Id,
                                        FeePlan_StartDate = invoice_date.Value,
                                        Nominal_Course_Fee = nominal_course_fee,
                                        Actual_Course_Fee = actual_fee,
                                        Discount_Amount = discount,

                                        Created_Date = DateTime.Now,
                                        Status = (int)EntityStatus.Visible,
                                    };

                                    //student_feeplan.SetupFeePlanInstallments(feeplan);

                                    //feeplan installments
                                    
                                    var i = 0;
                                    var FeePlan_Detail_Id = feeplan.FeePlan_Details.First().Id;
                                    var current_year = START_YEAR;
                                    //var max_i = student_feeplan.Student_FeePlan_Installments.Count;

                                    var Amount_Planning = 0;
                                    var Amount_Actual = 0;
                                    //DateTime Date_Planning = DateTime.Today;
                                    //DateTime? Date_Actual = DateTime.Today;
                                    Student_FeePlan_Installment current_install = null;
                                    for (int col = START_FEEPLAN_COL; col <= noOfCol; col++)
                                    {
                                        var year_row = GetCellValue(ws, YEAR_ROW, col);
                                        if (!string.IsNullOrEmpty(year_row))
                                        {
                                            year_row = year_row.Replace("Năm", "").Trim();
                                            current_year = int.Parse(year_row);
                                            Console.WriteLine("Year = " + current_year);
                                        }

                                        try
                                        {
                                            var data = GetCellValue(ws, row, col);
                                            if (!string.IsNullOrWhiteSpace(data))
                                            {
                                                var amount = GetCellValue_Int(ws, row, col);
                                                if (amount > 0)
                                                {
                                                    var date = GetCellValue(ws, MONTH_ROW, col);
                                                    if (!string.IsNullOrEmpty(date))
                                                    {
                                                        date = date.Trim().ToLower().Remove_Khoang_Trang();
                                                        if (date.Contains("plan"))
                                                        {
                                                            Amount_Planning = amount;
                                                            var this_month = date.Replace("-plan", "").Replace("t", "").Trim();
                                                            var Date_Planning = new DateTime(current_year, int.Parse(this_month), 1);

                                                            //add install
                                                            if (current_install != null)
                                                            {
                                                                student_feeplan.Student_FeePlan_Installments.Add(current_install);
                                                                current_install = null;
                                                            }

                                                            current_install = new Student_FeePlan_Installment
                                                            {
                                                                FeePlan_Detail_Id = FeePlan_Detail_Id,
                                                                Amount_Planning = Amount_Planning,
                                                                Date_Planning = Date_Planning,

                                                                Installment_Status = (int)InstallmentStatusEnum.Planned,
                                                                Status = (int)EntityStatus.Visible,
                                                            };
                                                        }
                                                        else if (date.Contains("actual"))
                                                        {
                                                            Amount_Actual = amount;
                                                            //AMMHN
                                                            //var Date_Actual = GetCellValue_DateTime_Full(ws, row, col + 1);
                                                            //if (Date_Actual == null)
                                                            //{
                                                            //    var this_month = date.Replace("-actual", "").Replace("t", "").Trim();
                                                            //    Date_Actual = new DateTime(current_year, int.Parse(this_month), 1);
                                                            //}
                                                            //AMMHCM
                                                            var this_month = date.Replace("-actual", "").Replace("t", "").Trim();
                                                            var Date_Actual = new DateTime(current_year, int.Parse(this_month), 1);

                                                            if (current_install == null)
                                                            {
                                                                current_install = new Student_FeePlan_Installment
                                                                {
                                                                    FeePlan_Detail_Id = FeePlan_Detail_Id,
                                                                    Amount_Planning = Amount_Actual,
                                                                    Date_Planning = Date_Actual,

                                                                    Installment_Status = (int)InstallmentStatusEnum.Planned,
                                                                    Status = (int)EntityStatus.Visible,
                                                                };
                                                            }
                                                            else
                                                            {
                                                                if (current_install.Amount_Planning == 0)
                                                                {
                                                                    current_install.Amount_Planning = Amount_Actual;
                                                                    current_install.Date_Planning = Date_Actual;
                                                                }
                                                            }

                                                            current_install.Amount_Actual = Amount_Actual;
                                                            current_install.Date_Actual = Date_Actual;
                                                            current_install.Installment_Status = (int)InstallmentStatusEnum.Finished;

                                                            //add install
                                                            if (current_install != null)
                                                            {
                                                                student_feeplan.Student_FeePlan_Installments.Add(current_install);
                                                                current_install = null;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            //throw ex;
                                            return Error(ex.Message);
                                        }
                                    }

                                    //add install
                                    if (current_install != null)
                                    {
                                        student_feeplan.Student_FeePlan_Installments.Add(current_install);
                                        current_install = null;
                                    }

                                    try
                                    {
                                        //check feeplan
                                        if (student_feeplan.IsFinishAllFees)
                                        {
                                            foreach (var ins in student_feeplan.Student_FeePlan_Installments.Valid())
                                            {
                                                if (ins.Installment_Status != (int)InstallmentStatusEnum.Finished)
                                                {
                                                    ins.Status = (int)EntityStatus.Invisible;
                                                    //student_feeplan.Student_FeePlan_Installments.Remove(ins);
                                                }
                                            }
                                        }

                                        IoCConfig.Service<IStudent_FeePlanService>().Add(student_feeplan);
                                        count++;
                                    }
                                    catch (Exception ex)
                                    {
                                        //throw ex;
                                        return Error(ex.Message);
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            //throw ex;
                            return Error(ex.Message);
                        }
                    }

                    //test feeplan
                    //var r = 1;
                    feeplan_list = feeplan_list.DistinctBy(f => f.Count).ToList();
                    //foreach (var f in feeplan_list)
                    //{
                    //    ws_out.Cells[r, 1].Value = f.Count;
                    //    ws_out.Cells[r, 2].Value = f.Name;
                    //    r++;
                    //}

                    //var stream = new MemoryStream(pck.GetAsByteArray());

                    //return File(stream,
                    //    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    //    "FeePlanList.xlsx");

                    return MyContent("Upload successfully! = " + count);
                }
            }

            return Error();
        }
        #endregion

        #region FeePlan_Detail
        public ActionResult ManageFeePlan_Detail(int? page, int id = 0)
        {
            //_partial_AddEdit_Course
            //_partial_Search_Course
            //return ManageModel(new CourseFamily(), null);

            IEnumerable<FeePlan_Detail> list = null;
            var currentParentName = "";
            if (id == 0)
            {
                list = IoCConfig.Service<IFeePlan_DetailService>().FindAllValid();
            }
            else
            {
                var feeplan = IoCConfig.Service<IFeePlanService>().FindById(id);
                if (feeplan == null || !feeplan.IsValid())
                    return RedirectToError();
                list = feeplan.FeePlan_Details.Valid();
                currentParentName = feeplan.FeePlan_Name;
            }

            return ManageModel(new FeePlan_Detail { FeePlan_Id = id }, page,
                list: list, entityName: "FeePlan Detail", 
                currentParentName: currentParentName,
                currentParentId: id,
                filterSearch: SearchModelEnum.None);
        }

        [HttpPost]
        public ActionResult AddFeePlan_Detail(FeePlan_Detail model)
        {
            return AddModel(model);
        }

        [HttpPost]
        public ActionResult EditFeePlan_Detail(FeePlan_Detail model)
        {
            return EditModel(model);
        }

        [HttpPost]
        public ActionResult DeleteFeePlan_Detail(int id)
        {
            return DeleteModel(id);
        }
        #endregion FeePlan_Detail

        #region Manage Installments
        private IEnumerable<Student_FeePlan> GetDueInstallments(DateTime start_Date, DateTime end_Date)
        {
            var items = IoCConfig.Service<IStudent_FeePlanService>()
                .FindAllValidByCriteria(f => f.Remain_FeeUntilDate(start_Date, end_Date) > 0)
                .OrderByDescending(f => f.Student.Student_FullName);
                //.OrderByDescending(f => f.Remain_FeeUntilDate(start_Date, end_Date));

            return items;
            //return IoCConfig.Service<IStudent_FeePlanService>()
            //    .FindAllValidByCriteria(f => f.Remain_FeeUntilDate(date) > 0)
            //    .OrderByDescending(s => s.FeePlan_StartDate);
        }

        public ActionResult ManageFeePlan_Installment(int? page)
        {
            var date = DateTime.Today;
            var start_Date = date.StartOfMonth();
            var end_Date = date.EndOfMonth();
            ViewBag.Start_Date = start_Date;
            ViewBag.End_Date = end_Date;

            var feeplans = GetDueInstallments(start_Date, end_Date);

            ViewBag.Total_Due = feeplans.Sum(f => f.Remain_FeeUntilDate(start_Date, end_Date));

            return ManageModel(
                new Student_FeePlan(), page,
                list: feeplans,
                entityName: "Due Installment",
                entityFilter: "Installment",
                canAdd: false,
                filterSearch: SearchModelEnum.ByOthers);
        }

        [HttpPost]
        public ActionResult FilterInstallment(
            int? page,
            int? page_size,
            int pageChange,
            string entity,

            string entityFilter,
            int Student_Status = 0,
            int Company_Id = 0,
            DateTime? End_Date = null,

            string Model_Name = "",
            string sort_target = "Module_Name",
            bool sort_rank = false)
        {
            int pageSize = (page_size ?? PAGE_SIZE_LARGE_20);
            int pageNumber = (page ?? 1);
            if (pageChange == 0)
                pageNumber = 1;

            var date = End_Date.HasValue ? End_Date.Value : DateTime.Today;
            var start_Date = date.StartOfMonth();
            var end_Date = date.EndOfMonth();
            ViewBag.Start_Date = start_Date;
            ViewBag.End_Date = end_Date;

            var items = GetDueInstallments(start_Date, end_Date);
            if (!string.IsNullOrEmpty(Model_Name))
            {
                pageNumber = 1;

                //var name = Model_Name;
                //name = name.ToLower().Convert_Chuoi_Khong_Dau();
                //items = items.Where(c => c.Student.Student_FullName.ToLower().Convert_Chuoi_Khong_Dau().Contains(name));
                var name = Model_Name;
                name = name.ToLower();

                if (name.IsNumber())
                {
                    items = items.Where(c => c.Student.GetPhone().Contains(name) || c.Student.GetEnrollNumber().Contains(name));
                }
                else
                {
                    if (name.StartsWith(PREFIX_STUDENT.ToLower()) || name.StartsWith(PREFIX_NONSTUDENT.ToLower()))
                    {
                        items = items.Where(c => c.Student.GetEnrollNumber().ToLower().Contains(name));
                    }
                    else
                    {
                        name = name.Replace(PREFIX_STUDENT.ToLower(), "").Replace(PREFIX_NONSTUDENT.ToLower(), "");
                        name = name.Convert_Chuoi_Khong_Dau();
                        items = items.Where(c => c.Student.Student_FullName.ToLower().Convert_Chuoi_Khong_Dau().Contains(name));
                    }
                }
            }
            //others
            if (Student_Status != 0)
            {
                items = items.Where(c => c.Student.Student_Status == Student_Status);
            }
            if (Company_Id != 0)
            {
                items = items.Where(c => c.Student.Class.Company_Id == Company_Id);
            }

            ViewBag.Total_Due = items.Sum(f => f.Remain_FeeUntilDate(start_Date, end_Date));
            ViewBag.sort_target = sort_target;
            ViewBag.sort_rank = sort_rank ? "asc" : "desc";

            return ReturnPartialView(entity, items, pageNumber, pageSize, entityFilter);
        }

        #endregion
    }
}