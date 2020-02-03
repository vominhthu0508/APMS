using System;
using System.Drawing;
using System.IO;
using System.Web.Helpers;
using System.Web.Hosting;
using System.Web.Mvc;
using XT.BusinessService;
using XT.Model;
using XT.Web.External;
using System.Linq;
using XT.Web.External.MVCAttributes;

namespace XT.Web.Controllers
{
    public class ImageOutputCache : OutputCacheAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            base.OnResultExecuting(filterContext);
            filterContext.HttpContext.Response.ContentType = "image/jpeg";
        }
    }

    public class ImageController : BaseController
    {
        int SMALL_SIZE = 100;
        int MEDIUM_SIZE = 500;
        int LARGE_SIZE = 1000;
        [ImageOutputCache(Duration = 31536000)]//365 days
        public void GetPhotoThumbnail(string path, int width = 0 , int height = 0)
        {
            GetPhotoLocal(path, width, height);
            //GetPhotoNoImage(path, width, height);
        }

        private void GetPhotoLocal(string path, int width = 0, int height = 0)
        {
            if (path != null)
            {
                string url = HostingEnvironment.MapPath(path);
                if (System.IO.File.Exists(url))
                {
                    string orgFileName = Path.GetFileNameWithoutExtension(url);
                    string orgExt = Path.GetExtension(url);

                    string newFileName = String.Join("_", orgFileName, width.ToString(), height.ToString()) + orgExt;
                    string existImage = Path.GetDirectoryName(url) + "\\" + newFileName;
                    if (!System.IO.File.Exists(existImage))
                    {
                        // Get original image
                        FileStream fs = new FileStream(url, FileMode.Open);
                        Image org_img = Image.FromStream(fs);
                        fs.Close();
                        //width = 0 || height = 0
                        //100, 500, 1000
                        if (width > 0)
                        {
                            if (width <= SMALL_SIZE)
                            {
                                width = SMALL_SIZE;
                            }
                            else if (width <= MEDIUM_SIZE)
                            {
                                width = MEDIUM_SIZE;
                            }
                        }
                        else
                        {
                            width = LARGE_SIZE;
                        }

                        if (height == 0)
                        {
                            height = (org_img.Height * width / org_img.Width);
                        }

                        org_img = ImageUlti.ResizeCropExcess(org_img, width, height);
                        org_img.Save(existImage);
                    }

                    new WebImage(existImage).Write();
                    return;
                }
            }

            var other = Helper.MyUrlContent_DefaultImage(path);
            new WebImage(other).Write();
        }

        private void GetPhotoNoImage(string path, int width = 0, int height = 0)
        {
            var other = HostingEnvironment.MapPath(@"~/Images/no_img.jpg");
            new WebImage(other).Write();
        }
    }
}
