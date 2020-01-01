using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoHosting.Services.Entities
{
    public class Video
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        
        public DateTime DayofCreation { get; set; }
        public int Views { get; set; }

        public virtual UserProfile UserProfile { get; set; }
        public virtual List<Comentary> Comentaries { get; set; }

        public virtual List<VideoUser> Likes { get; set; }
        public virtual List<VideoUser> Dislikes { get; set; }

        public string PhotoPath { get; set; }
        public string VideoPath { get; set; }

        public Video()
        {
            Comentaries = new List<Comentary>();
            Likes = new List<VideoUser>();
            Dislikes = new List<VideoUser>();
        }

        public void AddComentary(Comentary comentary)
        {
            Comentaries.Add(comentary);
        }
        public void DeleteComentary(Comentary comentary)
        {
            Comentaries.Remove(comentary);
        }
    }
}
