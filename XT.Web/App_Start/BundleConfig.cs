using System.Web;
using System.Web.Optimization;

namespace XT.Web
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            BundleTable.EnableOptimizations = AppSettings.BundleEnableOptimizations;

            #region Preload
            ///////////////////////////////////////////////////////
            bundles.Add(new ScriptBundle("~/Scripts/PreScript").Include(
                "~/Scripts/Global/PreScript.js"
            ));
            bundles.Add(new ScriptBundle("~/Scripts/PreLoad").Include(
                "~/Scripts/Global/PreLoad.js"
            ));
            //////////////////////////////////////////////////////////
            #endregion

            #region Layout

            #region Client
            //////////////////////////////////////////////////////////////////////////////
            //////////////////////////////////////////////////////////////////////////////
            // Layout
            //Styles
            bundles.Add(new StyleBundle("~/Content/Stylesheets/primary").Include(
                "~/Content/css/bootstrap.css",
                //apps
                "~/Content/css/Global/*.css"//css của riêng website
                //"~/Content/css/Global/fontello/css/*.css"
               ));
            bundles.Add(new StyleBundle("~/Content/Stylesheets/layout").Include(
                "~/Content/css/animate.css",
                "~/Content/css/bootstrap-dialog.css",
                "~/Content/css/font-awesome.css",
                "~/Content/css/slider-pro.css",
                //apps
                //"~/Content/css/Global/*.css",
                "~/Content/css/Global/fontello/css/*.css"
               ));
            bundles.Add(new ScriptBundle("~/Scripts/layout").Include(
                //vendors
                "~/Scripts/jquery-1.11.2.js",
                "~/Scripts/jquery-ui-1.10.4.custom.js",//typehead/autocomplete: home, search, compare
                "~/Scripts/bootstrap.js",
                "~/Scripts/bootstrap-dialog.js",
                "~/Scripts/typeahead.js",//typehead: home, search, compare
                "~/Scripts/jquery.raty.js",
                "~/Scripts/jquery.ddslick.js",//search, addnewproject
                "~/Scripts/jquery.lazyload.js",
                "~/Scripts/parallax.js",//search, hostlist
                "~/Scripts/icheck.js",//search, addnewproject
                //apps
                //-common
                "~/Scripts/Global/General.js",
                "~/Scripts/Global/Dictionary.js",
                "~/Scripts/Global/Cookie.js",
                "~/Scripts/Global/Dialog.js",
                "~/Scripts/Global/Form.js",
                "~/Scripts/Global/Account.js",
                //-project
                "~/Scripts/Global/Layout_LoadGoogleMap.js",
                "~/Scripts/Global/LayoutFrame.js",//tất cả những hàm chung nhất cho Layout chính và Layout Frame
                "~/Scripts/Global/Layout.js"
                 ));
            #endregion

            #region Admin
            //////////////////////////////////////////////////////////////////////////////
            //////////////////////////////////////////////////////////////////////////////
            // Admin/Layout
            bundles.Add(new StyleBundle("~/Content/Stylesheets/layoutadmin").Include(
                //vendors
                "~/Content/css/*.css",
                "~/Scripts/Admin/app/plugins/select2/select2.css",
                "~/Scripts/Admin/app/plugins/datepicker/datepicker3.css",
                "~/Scripts/Admin/app/plugins/daterangepicker/daterangepicker-bs3.css",
                "~/Scripts/Admin/app/plugins/timepicker/bootstrap-timepicker.css",
                //apps
                "~/Content/css/Admin_NewDesign/*.css",
                "~/Content/css/Admin_NewDesign/skins/_all-skins.css"
                ));
            bundles.Add(new ScriptBundle("~/Scripts/layoutadmin").Include(
                //vendors
                    "~/Scripts/vendors/jquery-1.11.2.js",
                    "~/Scripts/vendors/jquery-ui-1.9.2.js",
                    "~/Scripts/vendors/bootstrap.js",
                    "~/Scripts/vendors/bootstrap-dialog.js",
                    "~/Scripts/vendors/typeahead.js",
                    "~/Scripts/Admin/app/plugins/select2/select2.full.js",
                    "~/Scripts/Admin/app/plugins/sparkline/jquery.sparkline.js",
                    "~/Scripts/Admin/app/plugins/datepicker/bootstrap-datepicker.js",
                    "~/Scripts/Admin/app/plugins/daterangepicker/moment.js",
                    "~/Scripts/Admin/app/plugins/daterangepicker/daterangepicker.js",
                    "~/Scripts/Admin/app/plugins/input-mask/jquery.inputmask.js",
                    "~/Scripts/Admin/app/plugins/input-mask/jquery.inputmask.date.extensions.js",
                    "~/Scripts/Admin/app/plugins/timepicker/bootstrap-timepicker.js",
                //apps
                   "~/Scripts/Admin/app/layout.js",
                    "~/Scripts/Admin/app/app.js",
                    "~/Scripts/Admin/app/Form.js",
                    "~/Scripts/Admin/app/Sort.js",
                    "~/Scripts/Admin/app/ManageDataTable.js"
                 ));
            #endregion

            #endregion Layout

            #region Client
            #region Home
            //////////////////////////////////////////////////////////////////////////////
            //////////////////////////////////////////////////////////////////////////////
            // Home/Index
            bundles.Add(new StyleBundle("~/Content/Stylesheets/Home/Index").Include(
                "~/Content/css/slider-pro.css",
                "~/Content/css/Home/Index.css"
                ));

            bundles.Add(new ScriptBundle("~/Scripts/Home/Index").Include(
                "~/Scripts/jquery.sliderPro.js",
                "~/Scripts/Home/AutoCompleteSearch.js",
                "~/Scripts/Home/Index.js"
                ));

            //////////////////////////////////////////////////////////////////////////////
            //////////////////////////////////////////////////////////////////////////////
            // Home/Login
            bundles.Add(new StyleBundle("~/Content/Stylesheets/Home/login").Include(
                "~/Content/css/Global/Login.css"
                ));

            
            #endregion
            #endregion Client

            #region Admin
            bundles.Add(new ScriptBundle("~/Scripts/Admin/Index").Include(
                    "~/Scripts/Admin/app/plugins/jvectormap/jquery-jvectormap-1.2.2.js",
                    "~/Scripts/Admin/app/plugins/jvectormap/jquery-jvectormap-world-mill-en.js",
                    "~/Scripts/Admin/app/plugins/chartjs/Chart.js",
                    "~/Scripts/Admin/Index.js"
                  ));

            bundles.Add(new ScriptBundle("~/Scripts/Admin/ManageSchedule_ClassSession_Calendar").Include(
                    "~/Scripts/Admin/app/plugins/fullcalendar/*.js",
                    "~/Scripts/Admin/ManageSchedule_ClassSession_Calendar.js"
                  ));

            bundles.Add(new StyleBundle("~/Content/Admin/ManageSchedule_ClassSession_Calendar").Include(
                    "~/Scripts/Admin/app/plugins/fullcalendar/*.css"
                ));

            bundles.Add(new ScriptBundle("~/Scripts/Admin/ManageSchedule_ClassModule").Include(
                    "~/Scripts/Admin/ManageSchedule_ClassModule.js"
                ));

            bundles.Add(new ScriptBundle("~/Scripts/Admin/ManageFeePlan").Include(
                    "~/Scripts/Admin/ManageFeePlan.js"
                ));

            bundles.Add(new ScriptBundle("~/Scripts/Admin/ManageFeePlan_Student").Include(
                    "~/Scripts/Admin/ManageFeePlan_Student.js"
                ));

            bundles.Add(new ScriptBundle("~/Scripts/Admin/Report").Include(
                    "~/Scripts/Admin/Report.js"
                ));

            #endregion

            // If optimizations aren't enabled
            if (!BundleTable.EnableOptimizations)
            {
                // Iterate over each bundle
                foreach (var b in bundles)
                {
                    // And strip out any transformations (minify)
                    b.Transforms.Clear();
                }
            }
        }
    }
}