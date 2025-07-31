using System.ComponentModel.DataAnnotations;

namespace RazorTableDemo.Models
{
    public class ICItemMap
    {
        [Required]
        [StringLength(16)]
        public string ClientCode { get; set; } = string.Empty;

        [Required]
        [StringLength(24)]
        public string ItemNumber { get; set; } = string.Empty;

        public int ItemIndex { get; set; }

        public DateTime? SourceStamp { get; set; }

        [Required]
        [StringLength(16)]
        public string EtimItemCode { get; set; } = string.Empty;

        [StringLength(16)]
        public string? ItemClassCode { get; set; }

        [StringLength(8)]
        public string? PkgUnitCode { get; set; }

        [StringLength(8)]
        public string? QtyUnitCode { get; set; }

        public bool IsSaved { get; set; }

        [StringLength(128)]
        public string? Remark { get; set; }

        [StringLength(2048)]
        public string? ReqPayload { get; set; }

        [StringLength(1024)]
        public string? RespPayload { get; set; }

        public DateTime? CreatedOn { get; set; }

        [Required]
        [StringLength(40)]
        public string CreatedBy { get; set; } = "SYS-ADMIN";

        public DateTime? UpdatedOn { get; set; }

        [StringLength(40)]
        public string? UpdatedBy { get; set; }
    }
} 