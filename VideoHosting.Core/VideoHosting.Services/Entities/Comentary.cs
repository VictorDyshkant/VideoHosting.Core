using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoHosting.Services.Entities
{
    public class Comentary
    {
        public int Id { get; set; }
        public string Content { get; set; }

        public DateTime DayofCreation { get; set; }

        public virtual Video Video { get; set; }
        public virtual UserProfile UserProfile { get; set; }
    }
}
