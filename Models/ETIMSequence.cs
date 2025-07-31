using System.ComponentModel.DataAnnotations;

namespace RazorTableDemo.Models
{
    public class ETIMSequence
    {
        [Required]
        [StringLength(16)]
        public string ClientCode { get; set; } = string.Empty;
        
        public int? ItemSequence { get; set; } = 0;
        
        public int? SaleInvoiceSeq { get; set; } = 0;
        
        public int? PurchInvoiceSeq { get; set; } = 0;
        
        public DateTime CreatedOn { get; set; }
        
        public DateTime? UpdatedOn { get; set; }
    }
} 