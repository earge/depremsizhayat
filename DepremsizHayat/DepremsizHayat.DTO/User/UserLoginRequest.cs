using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepremsizHayat.DTO.User
{
    public class UserLoginRequest
    {
        [Required]
        public string E_MAIL { get; set; }
        [Required]
        public string PASSWORD { get; set; }
    }
}
