using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoHosting.Services.DTO
{
    public interface IComentaryService
    {
        Task AddComentary(ComentaryDTO comentary);
        Task RemoveComentary(int id);
        Task<ComentaryDTO> GetComentaryById(int id);
        Task<IEnumerable<ComentaryDTO>> GetComentariesByVideoId(int videoId);
    }
}
