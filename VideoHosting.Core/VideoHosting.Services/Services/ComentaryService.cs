using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using VideoHosting.Core.Exceptions;
using VideoHosting.Services.DTO;
using VideoHosting.Services.Entities;
using VideoHosting.Services.Unitofwork;

namespace VideoHosting.Services.Services
{
    public class ComentaryService : IComentaryService
    {
        protected IUnitofwork _unit;
        protected readonly IMapper _mapper;
        
        public ComentaryService(IUnitofwork unit,IMapper mapper)
        {
            _unit = unit;
            _mapper = mapper;
        }

        public async Task AddComentary(ComentaryDTO comentaryDTO)
        {
           Comentary comentary = new Comentary()
            {
                Content = comentaryDTO.Content,
                UserProfile = await _unit.UserRepository.GetUserProfileById(comentaryDTO.UserId),
                Video = await _unit.VideoRepository.GetVideoById(comentaryDTO.VideoId),
                DayofCreation = DateTime.Now
            };
           await _unit.ComentaryRepository.AddComentary(comentary);
        }

        public async Task<IEnumerable<ComentaryDTO>> GetComentariesByVideoId(int videoId)
        {
            IEnumerable<Comentary> comentaries = await _unit.ComentaryRepository.GetComentariesByVideoId(videoId);
            List<ComentaryDTO> comentariesDTO = new List<ComentaryDTO>();
            foreach(var c in comentaries)
            {
                comentariesDTO.Add(await GetComentaryById(c.Id));
            }
            return comentariesDTO;
        }

        public async Task<ComentaryDTO> GetComentaryById(int id)
        {
            Comentary comentary = await _unit.ComentaryRepository.GetComentaryById(id);
            if (comentary == null)
                throw new InvalidDataException("Comentary do not exist");

            ComentaryDTO comentaryDTO = _mapper.Map<ComentaryDTO>(comentary);
            comentaryDTO.User = _mapper.Map<UserDTO>(comentary.UserProfile);
            return comentaryDTO;
        }

        public async Task RemoveComentary(int id)
        {
            Comentary comentary = await _unit.ComentaryRepository.GetComentaryById(id);
            if (comentary == null)
                throw new InvalidDataException("Comentary do not exist");

            await _unit.ComentaryRepository.RemoveComentary(comentary);
            await _unit.SaveAsync();
        }
    }
}
