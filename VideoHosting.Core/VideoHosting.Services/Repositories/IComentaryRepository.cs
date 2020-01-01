using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoHosting.Services.Entities;

namespace VideoHosting.Services.Repositories
{
    public interface IComentaryRepository
    {
        Task AddComentary(Comentary comentary);
        Task RemoveComentary(Comentary comentary);
        Task<IEnumerable<Comentary>> GetComentariesByVideoId(int id);
        Task<Comentary> GetComentaryById(int id);
    }
}
