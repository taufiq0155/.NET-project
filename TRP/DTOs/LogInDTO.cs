using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TRP.DTOs
{
    public class LogInDTO
    {
        [Required]
        [StringLength(50, ErrorMessage = "Please Provide User Name.")]
        public string UserName { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Please Provide PAssword.")]
        public string Password { get; set; }
    }
}