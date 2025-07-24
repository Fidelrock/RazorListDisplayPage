namespace RazorTableDemo.Models
{
    public class UserProfile
    {
        public int Id { get; set; }
        public bool SignificantEconomicPresenceTax { get; set; }
        public string Citizenship { get; set; } = string.Empty;
        public bool RegisterForTOT { get; set; }
        public string MajorGroup { get; set; } = string.Empty;
        public string SubGroup { get; set; } = string.Empty;
        public string MinorGroup { get; set; } = string.Empty;
        public string SMSNotification { get; set; } = string.Empty;
        public bool DataPrivacyAccepted { get; set; }
        public bool AlternativeAddress { get; set; }
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
