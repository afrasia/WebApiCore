using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiCore.Models
{
    public class Car
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "VIN Number")]
        [SwaggerSchema(ReadOnly = true)]
        public Guid ID { get; set; }

        [StringLength(50, MinimumLength = 1)]
        [Required]
        public string Make { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        public decimal Price { get; set; }
    }
}
