using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.DataTransferObjects
{
    public class UserForUpdateDto
    {
        [Required(ErrorMessage = "UserId is required")]
        [StringLength(100, ErrorMessage = "UserId can't be longer than 100 characters")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, ErrorMessage = "Password can't be longer than 100 characters")]
        public string Password { get; set; }
    }
}
