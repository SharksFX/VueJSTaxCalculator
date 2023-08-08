using System.ComponentModel.DataAnnotations;

namespace webapi.Models
{
    public class TaxableExemptions
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int TaxYear { get; set; }

        [Required]
        public string TaxExemptionDescription { get; set; }

        public double TaxBaseAmount { get; set; }

        public double TaxBaseMultiplier { get; set; }

        public double TaxBaseAdditional { get; set; }

        public bool TaxExemptionEnabled { get; set; } = false;
    }
}
