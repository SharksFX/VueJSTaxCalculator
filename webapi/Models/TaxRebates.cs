using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;

namespace webapi.Models
{
    public class TaxRebates
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int TaxYear { get; set; }

        [Required]
        public string RebateType { get; set; }

        [Required]
        public double RebateAmount { get; set; }

        [Required]
        public double ThreshHoldAmount { get; set; }

        public string RebateTypeDescription { get; set; }


    }
}
