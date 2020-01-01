using System;
using System.Collections.Generic;
using System.Text;

namespace VideoHosting.Services.Entities
{
    public class VideoUser
    {
        public int VideoId { get; set; }
        public virtual Video Video { get; set; }

        public string UserProfileId { get; set; }
        public virtual UserProfile UserProfile { get; set; }
    }


    public class UserUser
    {
        public string SubscriberId { get; set; }
        public virtual UserProfile Subscriber { get; set; }

        public string SubscripterId { get; set; }
        public virtual UserProfile Subscripter { get; set; }
    }

}
