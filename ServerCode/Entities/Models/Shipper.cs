using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Entities.Models
{
    [Table("shipper")]
    public class Shipper
    {
        [Column("ShipperId")]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Value is required")]
        public int Value { get; set; }
        [Required(ErrorMessage = "Text is required")]
        [StringLength(60, ErrorMessage = "Text can't be longer than 60 characters")]
        public string Text { get; set; }

    }
}
