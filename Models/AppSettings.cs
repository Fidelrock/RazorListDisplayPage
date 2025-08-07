namespace RazorTableDemo.Models
{
    public class AppSettings
    {
        public string ClientCode { get; set; } = "CARLTD";
        public int DefaultPageSize { get; set; } = 10;
        public int MaxPageSize { get; set; } = 100;
        public bool EnableCaching { get; set; } = false;
        public int CacheTimeoutMinutes { get; set; } = 30;
    }
} 