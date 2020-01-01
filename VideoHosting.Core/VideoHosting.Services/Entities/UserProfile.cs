using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoHosting.Services.Entities
{
    public class UserProfile
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public bool Sex { get; set; }

        public int TempPassword { get; set; }

        public DateTime Birthday { get; set; }
        public virtual Country Country { get; set; }
        public DateTime DateOfCreation { get; set; }
        public string PhotoPath { get; set; }

        public virtual UserLogin UserLogin { get; set; }
        public virtual List<Video> Videos { get; set; }
        public virtual List<Comentary> Comentaries { get; set; }

        public virtual List<UserUser> Subscribers { get; set; }
        public virtual List<UserUser> Subscriptions { get; set; }

        public virtual List<VideoUser> Likes  { get; set; }
        public virtual List<VideoUser> Dislikes  { get; set; }

        public UserProfile()
        {
            Videos = new List<Video>();
            Comentaries = new List<Comentary>();

            Subscribers = new List<UserUser>();
            Subscriptions = new List<UserUser>();

            Likes = new List<VideoUser>();
            Dislikes = new List<VideoUser>();
        }

        public void AddLike(Video video)
        {
            if(Dislikes.FirstOrDefault(x=>x.Video == video) != null)
            {
                Dislikes.Remove(Dislikes.FirstOrDefault(x => x.Video == video));
            }
            Likes.Add(new VideoUser() { Video = video,UserProfile = this});
        }
        public void DeleteLike(Video video)
        {
            Dislikes.Remove(Dislikes.FirstOrDefault(x => x.Video == video));
        }

        public void AddDislike(Video video)
        { 
            if(Likes.FirstOrDefault(x => x.Video == video) != null)
            {
                Likes.Remove(Likes.FirstOrDefault(x => x.Video == video));
            }
            Dislikes.Add(new VideoUser() { Video = video, UserProfile = this });
        }
        public void DeleteDislike(Video video)
        {
            Dislikes.Remove(Likes.FirstOrDefault(x => x.Video == video));
        }

        public void Subscribe(UserProfile user)
        {
            if(Subscriptions.FirstOrDefault(x=>x.Subscripter == user) == null)
            {
                Subscriptions.Add(new UserUser() { Subscriber = this, Subscripter = user });
            }
        }
        public void Unsubscribe(UserProfile user)
        {
            if (Subscriptions.FirstOrDefault(x => x.Subscripter == user) != null)
            {
                Subscribers.Remove(Subscriptions.FirstOrDefault(x => x.Subscripter == user));
            }
        }
    }
}
