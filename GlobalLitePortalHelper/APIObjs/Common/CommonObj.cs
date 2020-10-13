namespace GlobalLitePortalHelper.APIObjs
{
    public struct AppSource
    {
        public const int None = 0;
        public const int Android = 1;
        public const int IOS = 2;
        public const int Web = 3;
        public const int Windows = 4;
    }

    public struct ServiceType
    {
        public const int LR_SMS_Result = 1;
        public int LR_Push_Result => 2;
        public int LR_SMS_Forecast => 3;
        public int LR_Push_Forecast => 4;
        public int LR_Chart => 5;
        public int LR_Transfer => 6;
    }

    public class ItemUpdateSearchObj
    {
        public int AppSourceType { get; set; }
        public int AppCode { get; set; }
        public int CurrentVersion { get; set; }

    }

    public enum LookupItem
    {
        AlertItem = 1,
        AlertProvider,
        AlertProviderItem,
        AlertSchedule,
        AlertType,
        AlertNetwork,
        AlertRoute,
        SMSProvider,
        SMSRoute,
    }
}
