using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepremsizHayat.DTO.User
{
    public class CreateUserRequest
    {
        [Required]
        public string FIRST_NAME { get; set; }
        [Required]
        public string LAST_NAME { get; set; }
        [Required]
        public string E_MAIL { get; set; }
        [Required]
        public string PASSWORD { get; set; }
        public string PROFILE_IMAGE { get; set; }
    }
}
