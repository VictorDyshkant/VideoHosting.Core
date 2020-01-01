using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoHosting.Services.Entities
{
    public class Country
    {
        public int Id { get; set; }
        public string CountryName { get; set; }
        public virtual List<UserProfile> Users{ get; set; }
        public Country()
        {
            Users = new List<UserProfile>();
        }
    }
}
