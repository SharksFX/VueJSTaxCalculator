

using System.ComponentModel.DataAnnotations;

namespace webapi.Models
{
    public class TaxTables
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int TaxYear { get; set; }

        [Required]
        public string TaxPeriodDescription { get; set; }

        [Required]
        public int TaxBracketStart { get; set; }

        [Required]
        public int TaxBracketEnd { get; set; }

        [Required]
        public double TaxBasePercent { get; set; }

        [Required]
        public double TaxBaseAmount { get; set; }
    }
}
