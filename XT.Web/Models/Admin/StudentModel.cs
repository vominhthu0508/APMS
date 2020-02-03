using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using XT.Model;
using XT.BusinessService;
using XT.Web.External;

namespace XT.Web.Models
{
    public class StudentModel : Student
    {
        public int Student_Id { get; set; }
        public HttpPostedFileBase uploadFile { get; set; }

        public override IEntity ToModel()//add
        {
            var model = new Student();
            return ToModel(model);
        }

        //Status = (int)EntityStatus.Visible
        public override IEntity ToModel(IEntity _model)//edit: _model = old
        {
            if (uploadFile != null && uploadFile.ContentLength > 0)
            {
                this.Student_Avatar = Helper.SaveAs(AppSettings.UploadUserPhotos, uploadFile);
            }

            var model = _model as Student;//old
            var currentStudentStatus = model.Student_Status;
            var currentStudentStatusDate = model.Student_Status_Date;
            var currentClassId = model.Class_Id;

            model.CopyModel(this);
            //if (uploadFile != null && uploadFile.ContentLength > 0)
            //{
            //    model.Student_Avatar = Helper.SaveAs(AppSettings.UploadImagesAdmin, uploadFile);
            //}

            //Status Change
            if (model.Id == 0 || currentStudentStatus != Student_Status)//add new || change status
            {
                model.Student_AcademicStatuses.Add(new Student_AcademicStatus {
                    Student_Status = Student_Status,
                    Student_Status_Date = Student_Status_Date,
                    Status = (int)EntityStatus.Visible
                });
            }
            else
            {
                if (currentStudentStatusDate != Student_Status_Date)
                {
                    var currStatus = model.Student_AcademicStatuses.FirstOrDefault(h => h.Student_Status == Student_Status);
                    if (currStatus != null)
                    {
                        currStatus.Student_Status_Date = Student_Status_Date;
                        IoCConfig.Service<IStudent_AcademicStatusService>().Update(currStatus);
                    }
                }
            }

            //Class Change
            if (model.Class_Id != currentClassId)
            {
                model.Student_ClassHistories.Add(new Student_ClassHistory
                {
                    Class_Id = Class_Id,
                    Created_Date = DateTime.Now,
                    Status = (int)EntityStatus.Visible,
                    StartDate = DateTime.Now
                });
            }

            return model;
        }
    }
}