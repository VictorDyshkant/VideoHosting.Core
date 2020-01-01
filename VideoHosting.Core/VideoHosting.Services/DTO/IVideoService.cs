using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoHosting.Services.DTO
{
    public interface IVideoService
    {
        Task<VideoDTO> GetVideoById(int id,string userId);
        Task<IEnumerable<VideoDTO>> GetVideos(string userId);

        Task<IEnumerable<VideoDTO>> GetVideosOfSubscripters( string userId);
        Task<IEnumerable<VideoDTO>> GetLikedVideos(string userId);
        Task<IEnumerable<VideoDTO>> GetDisLikedVideos(string userId);
        Task<IEnumerable<VideoDTO>> GetVideosByName(string name, string userId);
        Task<IEnumerable<VideoDTO>> GetVideosOfUser(string userId);

        Task AddVideo(VideoDTO video);
        Task RemoveVideo(int id);

        Task AddView(int id);
        Task PutLike(int videoId, string userId);
        Task PutDislike(int videoId, string userId);
    }
}
