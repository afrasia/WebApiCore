using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFCore.Models
{
    public class Car : CarBase
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "VIN Number")]
        public Guid ID { get; set; }
    }
}