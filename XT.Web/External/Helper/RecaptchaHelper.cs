using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using System.Xml.Linq;
using Facebook;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using XT.BusinessService;
using XT.Model;
using XT.Web.Controllers;
using XT.Web.Models;

namespace XT.Web.External
{
    public static class RecaptchaHelper
    {
        public static bool VerifyRecaptcha(this BaseController controller, bool isRegister = false)
        {
            var response = controller.HttpContext.Request["g-recaptcha-response"];
            if (!string.IsNullOrEmpty(response))
            {
                var secret = AppSettings.recaptchaPrivateKey;

                var client = new WebClient();
                var reply =
                    client.DownloadString(
                        string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secret, response));

                var captchaResponse = JsonConvert.DeserializeObject<CaptchaResponse>(reply);

                if (captchaResponse.Success)
                {
                    return true;
                }
                else//when response is false check for the error message
                {
                    if (captchaResponse.ErrorCodes.Count <= 0)
                        return false;

                    var error = captchaResponse.ErrorCodes[0].ToLower();
                    switch (error)
                    {
                        case ("missing-input-secret"):
                            //ViewBag.Message = "The secret parameter is missing.";
                            controller.SetCustomError("The secret parameter is missing.");
                            break;
                        case ("invalid-input-secret"):
                            //ViewBag.Message = "The secret parameter is invalid or malformed.";
                            controller.SetCustomError("The secret parameter is missing.");
                            break;

                        case ("missing-input-response"):
                            //ViewBag.Message = "The response parameter is missing.";
                            controller.SetCustomError("The response parameter is missing.");
                            break;
                        case ("invalid-input-response"):
                            //ViewBag.Message = "The response parameter is invalid or malformed.";
                            controller.SetCustomError("The response parameter is invalid or malformed.");
                            break;

                        default:
                            //ViewBag.Message = "Error occured. Please try again";
                            controller.SetCustomError("The secret parameter is missing.");
                            break;
                    }
                }
            }
            else
            {
                //var falseError = "Bạn phải nhập mã xác thực";
                var falseError = "Bạn vui lòng chọn xác thực không phải người máy trước khi thực hiện tác vụ";
                if (isRegister)
                {
                    falseError = "Vui lòng đồng ý với điều khoản trước khi đăng ký!";
                }
                controller.SetCustomError(falseError);
            }

            return false;
        }
    }
}