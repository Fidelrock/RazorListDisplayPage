using System.ComponentModel.DataAnnotations;

namespace RazorTableDemo.Models
{
    public class ETIMSEntityAttribute
    {
        [Required]
        [StringLength(16)]
        public string ClientCode { get; set; } = string.Empty;
        
        public int AttributeID { get; set; }
        
        [Required]
        [StringLength(32)]
        public string EntityType { get; set; } = string.Empty;
        
        [Required]
        [StringLength(32)]
        public string SearchKey { get; set; } = string.Empty;
        
        [Required]
        [StringLength(32)]
        public string EntityKey { get; set; } = string.Empty;
        
        [Required]
        [StringLength(32)]
        public string Title { get; set; } = string.Empty;
        
        [StringLength(64)]
        public string? ExtraKey { get; set; }
        
        [StringLength(64)]
        public string? ExtraValue { get; set; }
        
        public int SortOrder { get; set; } = 0;
    }
} 