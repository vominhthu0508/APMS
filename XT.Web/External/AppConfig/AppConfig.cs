using System.Configuration;
 
namespace XT.Web
{
    public static class AppSettings {
        public static bool PreserveLoginUrl { get { return bool.Parse(ConfigurationManager.AppSettings["PreserveLoginUrl"]); }}
        public static bool ClientValidationEnabled { get { return bool.Parse(ConfigurationManager.AppSettings["ClientValidationEnabled"]); }}
        public static bool UnobtrusiveJavaScriptEnabled { get { return bool.Parse(ConfigurationManager.AppSettings["UnobtrusiveJavaScriptEnabled"]); }}
        public static bool GenerateSampleData { get { return bool.Parse(ConfigurationManager.AppSettings["GenerateSampleData"]); }}
        public static bool BundleEnableOptimizations { get { return bool.Parse(ConfigurationManager.AppSettings["BundleEnableOptimizations"]); }}
        public static bool ShowErrorPage { get { return bool.Parse(ConfigurationManager.AppSettings["ShowErrorPage"]); }}
        public static string DefaultAccountAvatar { get { return ConfigurationManager.AppSettings["DefaultAccountAvatar"]; }}
        public static string DefaultAccountCover { get { return ConfigurationManager.AppSettings["DefaultAccountCover"]; }}
        public static string DefaultIcon { get { return ConfigurationManager.AppSettings["DefaultIcon"]; }}
        public static string DefaultRoomImage { get { return ConfigurationManager.AppSettings["DefaultRoomImage"]; }}
        public static string DefaultNoImage { get { return ConfigurationManager.AppSettings["DefaultNoImage"]; }}
        public static string DefaultPassword { get { return ConfigurationManager.AppSettings["DefaultPassword"]; }}
        public static int RoomValidDate { get { return int.Parse(ConfigurationManager.AppSettings["RoomValidDate"]); }}
        public static int RoomNewDate { get { return int.Parse(ConfigurationManager.AppSettings["RoomNewDate"]); }}
        public static int MapSearchDistrictDistance { get { return int.Parse(ConfigurationManager.AppSettings["MapSearchDistrictDistance"]); }}
        public static int MapSearchCityDistance { get { return int.Parse(ConfigurationManager.AppSettings["MapSearchCityDistance"]); }}
        public static int MapSearchRoomDistance { get { return int.Parse(ConfigurationManager.AppSettings["MapSearchRoomDistance"]); }}
        public static int MapSearchRoomPageSize { get { return int.Parse(ConfigurationManager.AppSettings["MapSearchRoomPageSize"]); }}
        public static int SearchRoomPageSize { get { return int.Parse(ConfigurationManager.AppSettings["SearchRoomPageSize"]); }}
        public static int HostRoomPageSize { get { return int.Parse(ConfigurationManager.AppSettings["HostRoomPageSize"]); }}
        public static int RoomReviewPageSize { get { return int.Parse(ConfigurationManager.AppSettings["RoomReviewPageSize"]); }}
        public static int RoomDetailUtilityCount { get { return int.Parse(ConfigurationManager.AppSettings["RoomDetailUtilityCount"]); }}
        public static int RoomDetailUtilityDistance { get { return int.Parse(ConfigurationManager.AppSettings["RoomDetailUtilityDistance"]); }}
        public static int RoomPriceUnit { get { return int.Parse(ConfigurationManager.AppSettings["RoomPriceUnit"]); }}
        public static string ContactEmail { get { return ConfigurationManager.AppSettings["ContactEmail"]; }}
        public static string ContactCCEmail { get { return ConfigurationManager.AppSettings["ContactCCEmail"]; }}
        public static string ContactPassword { get { return ConfigurationManager.AppSettings["ContactPassword"]; }}
        public static string ContactMailServer { get { return ConfigurationManager.AppSettings["ContactMailServer"]; }}
        public static int ContactMailPort { get { return int.Parse(ConfigurationManager.AppSettings["ContactMailPort"]); }}
        public static string ContactPhone { get { return ConfigurationManager.AppSettings["ContactPhone"]; }}
        public static string EmailAdmin { get { return ConfigurationManager.AppSettings["EmailAdmin"]; }}
        public static string Website { get { return ConfigurationManager.AppSettings["Website"]; }}
        public static bool EmailInUse { get { return bool.Parse(ConfigurationManager.AppSettings["EmailInUse"]); }}
        public static string FacebookAppID { get { return ConfigurationManager.AppSettings["FacebookAppID"]; }}
        public static string recaptchaPublicKey { get { return ConfigurationManager.AppSettings["recaptchaPublicKey"]; }}
        public static string recaptchaPrivateKey { get { return ConfigurationManager.AppSettings["recaptchaPrivateKey"]; }}
        public static bool RegisterCountEnable { get { return bool.Parse(ConfigurationManager.AppSettings["RegisterCountEnable"]); }}
        public static int RegisterCountTime { get { return int.Parse(ConfigurationManager.AppSettings["RegisterCountTime"]); }}
        public static int RegisterCount { get { return int.Parse(ConfigurationManager.AppSettings["RegisterCount"]); }}
        public static int MaxRoomCount { get { return int.Parse(ConfigurationManager.AppSettings["MaxRoomCount"]); }}
        public static int StatisticsRoomFlatsCount { get { return int.Parse(ConfigurationManager.AppSettings["StatisticsRoomFlatsCount"]); }}
        public static int StatisticsUsersCount { get { return int.Parse(ConfigurationManager.AppSettings["StatisticsUsersCount"]); }}
        public static int StatisticsViewsCount { get { return int.Parse(ConfigurationManager.AppSettings["StatisticsViewsCount"]); }}
        public static string SizeRage { get { return ConfigurationManager.AppSettings["SizeRage"]; }}
        public static string PriceRage { get { return ConfigurationManager.AppSettings["PriceRage"]; }}
        public static string PriceRageText { get { return ConfigurationManager.AppSettings["PriceRageText"]; }}
        public static string FlatCountRoom { get { return ConfigurationManager.AppSettings["FlatCountRoom"]; }}
        public static string p12File { get { return ConfigurationManager.AppSettings["p12File"]; }}
        public static bool LandingPage { get { return bool.Parse(ConfigurationManager.AppSettings["LandingPage"]); }}
        public static string UploadRoomPhotos { get { return ConfigurationManager.AppSettings["UploadRoomPhotos"]; }}
        public static string UploadHostPhotos { get { return ConfigurationManager.AppSettings["UploadHostPhotos"]; }}
        public static string UploadUserPhotos { get { return ConfigurationManager.AppSettings["UploadUserPhotos"]; }}
        public static string UploadCommentPhotos { get { return ConfigurationManager.AppSettings["UploadCommentPhotos"]; }}
        public static string UploadImagesAdmin { get { return ConfigurationManager.AppSettings["UploadImagesAdmin"]; }}
        public static string EmailToken { get { return ConfigurationManager.AppSettings["EmailToken"]; }}
        public static string RootPath { get { return ConfigurationManager.AppSettings["RootPath"]; }}
        public static int ImportingCenter { get { return int.Parse(ConfigurationManager.AppSettings["ImportingCenter"]); }}
    }
}