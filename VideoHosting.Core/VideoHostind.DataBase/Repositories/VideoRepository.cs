using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VideoHostind.DataBase;
using VideoHosting.Services.Entities;
using VideoHosting.Services.Repositories;

namespace VideoHostind.DataBase.Repositories
{
    public class VideoRepository : IVideoRepository
    {
        DataBaseContext context;
        public VideoRepository(DataBaseContext context)
        {
            this.context = context;
        }

        public async Task<Video> GetVideoById(int id)
        {
           return await context.Videos.FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<IEnumerable<Video>> GetVideos()
        {
            return await context.Videos.ToListAsync();
        }
        public async Task<IEnumerable<Video>> GetVideosbyName(string name)
        {
            return await context.Videos.Where(x=> x.Name.ToLower().Contains(name.ToLower())).ToListAsync();
        }

        public async Task AddVideo(Video video)
        {
            context.Videos.Add(video);
        }
        public async Task RemoveVideo(Video video)
        {
            context.Videos.Remove(video);
        }

   
    }
}
