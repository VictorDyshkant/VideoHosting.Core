using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoHosting.Services.DTO
{
    public class ComentaryDTO
    {
        public int Id { get; set; }
        [Required]
        public string Content { get; set; }
        public DateTime DayofCreation { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public int VideoId { get; set; }

        public UserDTO User { get; set; }
    }
}
