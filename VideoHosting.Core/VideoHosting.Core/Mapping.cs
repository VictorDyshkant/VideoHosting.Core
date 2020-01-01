using AutoMapper;
using System;
using System.Collections.Generic;
using VideoHosting.Core.Models;
using VideoHosting.Services.DTO;
using VideoHosting.Services.Entities;

namespace VideoHosting.Core
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<UserProfile, UserDTO>()
               .ForMember(c => c.Id, x => x.MapFrom(c => c.Id))
               .ForMember(c => c.Name, x => x.MapFrom(c => c.Name))
               .ForMember(c => c.Surname, x => x.MapFrom(c => c.Surname))
               .ForMember(c => c.Sex, x => x.MapFrom(c => c.Sex))
               .ForMember(c => c.DateOfCreation, x => x.MapFrom(c => c.DateOfCreation))
               .ForMember(c => c.Birthday, x => x.MapFrom(c => c.Birthday))
               .ForMember(c => c.Subscribers, x => x.MapFrom(c => c.Subscribers.Count))
               .ForMember(c => c.Subscriptions, x => x.MapFrom(c => c.Subscriptions.Count))
               .ForMember(c => c.Country, x => x.MapFrom(c => c.Country.CountryName))
               .ForMember(c => c.PhotoPath, x => x.MapFrom(c => c.PhotoPath));

            CreateMap<UserDTO, UserProfile>()
                .ForMember(c => c.Name, x => x.MapFrom(c => c.Name))
                .ForMember(c => c.Surname, x => x.MapFrom(c => c.Surname))
                .ForMember(c => c.Sex, x => x.MapFrom(c => c.Sex))
                .ForMember(c => c.Birthday, x => x.MapFrom(c => c.Birthday))
                .ForMember(c => c.DateOfCreation, x => x.MapFrom(c => DateTime.Now))
                .ForMember(c => c.Country, x => x.MapFrom(c => new Country()))
                .ForMember(c => c.Subscribers, x => x.MapFrom(c => new List<UserUser>()))
                .ForMember(c => c.Subscriptions, x => x.MapFrom(c => new List<UserUser>()));

            CreateMap<UserLogin, UserLoginDTO>()
                .ForMember(c => c.Id, x => x.MapFrom(c => c.Id))
                .ForMember(c => c.Email, x => x.MapFrom(c => c.Email))
                .ForMember(c => c.PhoneNumber, x => x.MapFrom(c => c.PhoneNumber));


            CreateMap<Comentary, ComentaryDTO>()
                .ForMember(c => c.Id, x => x.MapFrom(p => p.Id))
                .ForMember(c => c.Content, x => x.MapFrom(p => p.Content))
                .ForMember(c => c.DayofCreation, x => x.MapFrom(p => p.DayofCreation))
                .ForMember(c => c.UserId, x => x.MapFrom(p => p.UserProfile.Id))
                .ForMember(c => c.VideoId, x => x.MapFrom(p => p.Video.Id));

            CreateMap<Video, VideoDTO>()
                .ForMember(v => v.Id, x => x.MapFrom(p => p.Id))
                .ForMember(v => v.Name, x => x.MapFrom(p => p.Name))
                .ForMember(v => v.UserId, x => x.MapFrom(p => p.UserProfile.Id))
                .ForMember(v => v.PhotoPath, x => x.MapFrom(p => p.PhotoPath))
                .ForMember(v => v.VideoPath, x => x.MapFrom(p => p.VideoPath))
                .ForMember(v => v.Views, x => x.MapFrom(p => p.Views))
                .ForMember(v => v.Likes, x => x.MapFrom(p => p.Likes.Count))
                .ForMember(v => v.Dislikes, x => x.MapFrom(p => p.Dislikes.Count))
                .ForMember(v => v.DayofCreation, x => x.MapFrom(p => p.DayofCreation));

           
            CreateMap<UserLoginDTO, LoginUserModel>().ReverseMap();
            CreateMap<LoginUserModel, UserLoginDTO>();

            CreateMap<UserRegistrationModel, UserLoginDTO>()
                .ForMember(x=>x.Email, opt => opt.MapFrom(c => c.Email))
                .ForMember(x=>x.Password, opt => opt.MapFrom(c => c.Password))
                .ForMember(x=>x.PhoneNumber, opt => opt.MapFrom(c => c.PhoneNumber));

            CreateMap<UserRegistrationModel, UserDTO>()
                .ForMember(x => x.Name, opt => opt.MapFrom(c => c.Name))
                .ForMember(x => x.Surname, opt => opt.MapFrom(c => c.Surname))
                .ForMember(x => x.Sex, opt => opt.MapFrom(c => c.Sex))
                .ForMember(x => x.Birthday, opt => opt.MapFrom(c => c.Birthday))
                .ForMember(x => x.Country, opt => opt.MapFrom(c => c.Country));
        }
        public static IMapper GetMapper()
        {
            var conf = new MapperConfiguration(opt => { opt.AddProfile(new Mapping()); });
            return conf.CreateMapper();
        }
    }
}
