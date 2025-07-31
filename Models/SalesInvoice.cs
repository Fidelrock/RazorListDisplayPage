using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RazorTableDemo.Models
{
    public class SalesInvoice
    {
        public int ETRTrxID { get; set; }
        
        [Required]
        [StringLength(16)]
        public string ClientCode { get; set; } = string.Empty;
        
        [Required]
        [StringLength(32)]
        public string DocNumber { get; set; } = string.Empty;
        
        public int DocUniqueID { get; set; }
        
        [StringLength(32)]
        public string? RefInvoiceNum { get; set; }
        
        [StringLength(16)]
        public string? DocType { get; set; }
        
        public DateTime DocDate { get; set; }
        
        [StringLength(2)]
        public string? SourceApp { get; set; }
        
        public int ETRInvoiceNum { get; set; }
        
        [StringLength(12)]
        public string? CustomerNum { get; set; }
        
        [StringLength(100)]
        public string? CustomerName { get; set; }
        
        [StringLength(24)]
        public string? CustomerPIN { get; set; }
        
        [StringLength(64)]
        public string? ETRSerialNum { get; set; }
        
        public int? ETRStatus { get; set; }
        
        [StringLength(512)]
        public string? Remark { get; set; }
        
        [StringLength(3)]
        public string? DocSourceCurr { get; set; }
        
        [StringLength(3)]
        public string? DocHomeCurr { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal DocRate { get; set; }
        
        public DateTime DocRateDate { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal SourceAmtWTX { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal HomeAmtWTX { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal? SourceDiscWTX { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal? HomeDiscWTX { get; set; }
        
        [StringLength(32)]
        public string? CUNumber { get; set; }
        
        [StringLength(32)]
        public string? RefCUNumber { get; set; }
        
        public DateTime? QRTime { get; set; }
        
        [StringLength(256)]
        public string? QRText { get; set; }
        
        public byte[]? QRImage { get; set; }
        
        [StringLength(2048)]
        public string? ReqPayload { get; set; }
        
        [StringLength(2048)]
        public string? RespPayload { get; set; }
        
        public DateTime CreatedOn { get; set; }
        
        [Required]
        [StringLength(40)]
        public string CreatedBy { get; set; } = "SYS-ADMIN";
        
        public DateTime? UpdatedOn { get; set; }
        
        [StringLength(40)]
        public string? UpdatedBy { get; set; }
    }
} 