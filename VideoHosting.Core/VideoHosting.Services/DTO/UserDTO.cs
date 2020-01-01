using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoHosting.Services.DTO
{
    public class UserDTO
    {
        public const string Path = "http://localhost:52556/Content/UsersPhotos/";

        public string Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        
        public bool Admin { get; set; }
        public bool DoSubscribed { get; set; }

        public int Subscribers { get; set; }
        public string PhotoPath { get; set; }

        public DateTime Birthday { get; set; }
        public DateTime DateOfCreation { get; set; }

        public string Country { get; set; }
        public bool Sex { get; set; }
        public int Subscriptions { get; set; }
    }
}
