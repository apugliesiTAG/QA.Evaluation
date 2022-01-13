using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    [Table("user")]
    public class User
    {
        public Guid UserId { get; set; }
        [Required(ErrorMessage = "UserId is required")]
        [StringLength(100, ErrorMessage = "UserId can't be longer than 100 characters")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, ErrorMessage = "Password can't be longer than 100 characters")]
        public string Password { get; set; }
        public DateTime DateUpdatePassword { get; set; }
        public DateTime DateValidityPassword { get; set; }
        public string Token { get; set; }
        public DateTime DateValidityToken { get; set; }
        public bool State { get; set; }
    }
}
