using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VideoHosting.Services.Entities;
using VideoHosting.Services.Repositories;

namespace VideoHostind.DataBase.Repositories
{
    public class ComentaryRepository : IComentaryRepository
    {
        DataBaseContext context;
        public ComentaryRepository(DataBaseContext context)
        {
            this.context = context;
        }

        public async Task AddComentary(Comentary comentary)
        {
            context.Comentaries.Add(comentary);
        }
        public async Task<IEnumerable<Comentary>> GetComentariesByVideoId(int id)
        {
            Video com = await context.Videos.FirstOrDefaultAsync(x => x.Id == id);
            return com.Comentaries.ToList();
        }
        public async Task<Comentary> GetComentaryById(int id)
        {
            return await context.Comentaries.FirstOrDefaultAsync(x=>x.Id == id);
        }
        public async Task RemoveComentary(Comentary comentary)
        {
            context.Comentaries.Remove(comentary);
        }
    }
}
