
using System.ComponentModel.DataAnnotations;

namespace Tung.Result.Sample.Core.DTOs
{ 
    public class ForecastRequestDto
    {
        [Required]
        public string PostalCode { get; set; }
    }
}