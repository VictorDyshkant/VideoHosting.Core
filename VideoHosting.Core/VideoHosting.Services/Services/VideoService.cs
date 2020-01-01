using AutoMapper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using VideoHosting.Services.DTO;
using VideoHosting.Services.Entities;
using VideoHosting.Services.Unitofwork;

namespace VideoHosting.Services.Services
{
    public class VideoService : IVideoService
    {
        protected IUnitofwork _unit;
        protected readonly IMapper _mapper;

        public VideoService(IUnitofwork unit,IMapper mapper)
        {
            _unit = unit;
            _mapper = mapper;       
        }

        public async Task AddVideo(VideoDTO videoDTO)
        {
            UserProfile user = await _unit.UserRepository.GetUserProfileById(videoDTO.UserId);
            Video video = new Video()
            {
                Name = videoDTO.Name,
                Description = videoDTO.Description,
                Views = 0,
                UserProfile = user,
                PhotoPath = videoDTO.PhotoPath,
                VideoPath = videoDTO.VideoPath,
                DayofCreation = DateTime.Now,
            };

            await _unit.VideoRepository.AddVideo(video);
        }
        public async Task RemoveVideo(int videoId)
        {
            Video video = await _unit.VideoRepository.GetVideoById(videoId);
            if(video == null)
                throw new InvalidDataException("This video do not exist");

            await _unit.VideoRepository.RemoveVideo(video);
            await _unit.SaveAsync();
        }
        public async Task AddView(int videoId)
        {
            Video video = await _unit.VideoRepository.GetVideoById(videoId);
            if (video == null)
                throw new InvalidDataException("This video do not exist");

            video.Views++;
            await _unit.SaveAsync();
        }

        public async Task<VideoDTO> GetVideoById(int videoId, string userId)
        {
            UserProfile user = await _unit.UserRepository.GetUserProfileById(userId);
            Video video = await _unit.VideoRepository.GetVideoById(videoId);

            VideoDTO videoDTO = _mapper.Map<VideoDTO>(video);
            videoDTO.Liked = video.Likes.FirstOrDefault(x=>x.UserProfile == user) == null ? true :false;
            videoDTO.Disliked = video.Dislikes.FirstOrDefault(x => x.UserProfile == user) == null ? true : false;
            videoDTO.User = _mapper.Map<UserDTO>(video.UserProfile);

            return videoDTO;
        }

        public async Task<IEnumerable<VideoDTO>> GetDisLikedVideos(string userId)
        {
            UserProfile user = await _unit.UserRepository.GetUserProfileById(userId);

            List<VideoDTO> list = new List<VideoDTO>();
            foreach (var v in user.Dislikes)
            {
                list.Add(await GetVideoById(v.VideoId, userId));
            }
            return list;

        }
        public async Task<IEnumerable<VideoDTO>> GetLikedVideos(string userId)
        {
            UserProfile user = await _unit.UserRepository.GetUserProfileById(userId);

            List<VideoDTO> list = new List<VideoDTO>();
            foreach (var v in user.Likes)
            {
                list.Add(await GetVideoById(v.VideoId, userId));
            }
            return list;

        }

        public async Task<IEnumerable<VideoDTO>> GetVideos(string userId)
        {
            IEnumerable<Video> videos = await _unit.VideoRepository.GetVideos();
            List<VideoDTO> list = new List<VideoDTO>();
            foreach (var v in videos)
            {
                list.Add(await GetVideoById(v.Id, userId));
            }
            return list;
        }
        public async Task<IEnumerable<VideoDTO>> GetVideosOfSubscripters(string userId)
        {

            UserProfile user = await _unit.UserRepository.GetUserProfileById(userId);
            IEnumerable<UserUser> users = user.Subscriptions;
            List<VideoDTO> list = new List<VideoDTO>();
            foreach (var u in users)
            {
                foreach (var v in u.Subscripter.Videos)
                {
                    list.Add(await GetVideoById(v.Id, userId));
                }
            }
            return list.OrderBy(x => x.DayofCreation);
        }
        public async Task<IEnumerable<VideoDTO>> GetVideosByName(string name, string userId)
        {
            IEnumerable<Video> videos = await _unit.VideoRepository.GetVideosbyName(name);
            videos.OrderByDescending(x => x.DayofCreation).ToList();
            List<VideoDTO> list = new List<VideoDTO>();
            foreach (var v in videos)
            {
                list.Add(await GetVideoById(v.Id, userId));
            }
            return list;
        }
        public async Task<IEnumerable<VideoDTO>> GetVideosOfUser(string userId)
        {
            UserProfile user  = await _unit.UserRepository.GetUserProfileById(userId);

            List<VideoDTO> list = new List<VideoDTO>();
            foreach (var v in user.Videos)
            {
                list.Add(await GetVideoById(v.Id, userId));
            }
            return list;
        }

        public async Task PutLike(int videoId, string userId)
        {
            UserProfile user = await _unit.UserRepository.GetUserProfileById(userId);
            Video video = await _unit.VideoRepository.GetVideoById(videoId);
            if (video == null)
                throw new InvalidDataException("This video do not exist");

            user.AddLike(video);
            await _unit.SaveAsync();
        }
        public async Task PutDislike(int videoId, string userId)
        {
            UserProfile user = await _unit.UserRepository.GetUserProfileById(userId);
            Video video = await _unit.VideoRepository.GetVideoById(videoId);
            if (video == null)
                throw new InvalidDataException("This video do not exist");

            user.AddDislike(video);
            await _unit.SaveAsync();
        }
    }
}
