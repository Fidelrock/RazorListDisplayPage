namespace RazorTableDemo.Models
{
    public class S300TaxAuthority
    {
        public string ClientCode { get; set; }
        public string AuthorityKey { get; set; }
        public string Currency { get; set; }
        public bool Active { get; set; }
        public int TaxType { get; set; }
        public int TaxBase { get; set; }
        public DateTime LastMaintained { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
