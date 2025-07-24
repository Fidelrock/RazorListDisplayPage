namespace RazorTableDemo.Models
{
    public class SessionRecord
    {
        
            public int Id { get; set; }
            public string TrxCode { get; set; } = string.Empty;
            public string Vehicle { get; set; } = string.Empty;
            public string Transporter { get; set; } = string.Empty;
            public double NetWeight { get; set; }
            public DateTime WeightTime { get; set; }
        
    }
}
