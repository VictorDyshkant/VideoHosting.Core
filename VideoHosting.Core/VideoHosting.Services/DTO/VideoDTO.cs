using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoHosting.Services.DTO
{
    public class VideoDTO
    {
        public const string PhotoPat = "http://localhost:52556/Content/VideosPhotos/";
        public const string VideoPat = "http://localhost:52556/Content/UsersVideos/";

        public int Id { get; set; }
        public string UserId { get; set; }

        [Required]
        public string Name { get; set; }

        public DateTime DayofCreation { get; set; }
        public int Views { get; set; }

        [Required]
        public string VideoPath { get; set; }

        [Required]
        public string PhotoPath { get; set; }

        public int Likes { get; set; }
        public int Dislikes { get; set; }

        public string Description { get; set; }
        public bool Liked { get; set; }
        public bool Disliked { get; set; }

        public UserDTO User { get; set; }
    }
}
