using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Entities.Models
{
    [Table("customer")]
    public class Customer
    {
        [Column("CustomerId")]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "InteralId is required")]
        public string InteralId { get; set; }
        [Required(ErrorMessage = "Name is required")]
        [StringLength(60, ErrorMessage = "Name can't be longer than 60 characters")]
        public string Name { get; set; }
    }
}
