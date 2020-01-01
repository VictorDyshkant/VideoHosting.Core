using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoHosting.Services.Entities;

namespace VideoHosting.Services.Repositories
{
    public interface IVideoRepository
    {
        Task AddVideo(Video video);
        Task RemoveVideo(Video video);
        Task<IEnumerable<Video>> GetVideosbyName(string name);
        Task<IEnumerable<Video>> GetVideos();
        Task<Video> GetVideoById(int id);
        
    }
}
