using AutoMapper;
using BLL.AbstractServices;
using BLL.Dtos.ContentMediaDto;
using DAL.Entities;
using DAL.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ConcreteServices
{
    public class ContentMediaService : IContentMediaService
    {
        private readonly IGenericRepository<ContentMedia> _genericRepository;
        private readonly IMapper _mapper;
        private readonly IContentMediaRepository _contentMediaRepository;

        public ContentMediaService(IGenericRepository<ContentMedia> genericRepository, IMapper mapper, IContentMediaRepository contentMediaRepository)
        {
            _genericRepository = genericRepository;
            _mapper = mapper;
            _contentMediaRepository = contentMediaRepository;
        }
        public async Task AddContentMediaAsync(ContentMediaAddDto contentMediaAddDto)
        {
            await _genericRepository.AddAsync(_mapper.Map<ContentMedia>(contentMediaAddDto));
        }

        public async Task DeleteContentMediaAsync(int id)
        {
            var deletedContenMedia = await _genericRepository.GetByIdAsync(id);

            if (deletedContenMedia == null)

            {
                throw new KeyNotFoundException("Böyle bir içerik bulunamadı!");
            }

            await _genericRepository.DeleteAsync(id);
        }
        public async Task UpdateContentMediaAsync(int id, ContentMediaUpdateDto contentMediaUpdateDto)
        {
            var updatedMedia = await _genericRepository.GetByIdAsync(id);
            
            _mapper.Map(contentMediaUpdateDto, updatedMedia);

            if (updatedMedia == null)
            {
                throw new KeyNotFoundException("Böyle bir içerik bulunamadı!");
            }
            await _genericRepository.UpdateAsync(id, _mapper.Map<ContentMedia>(updatedMedia));
        }  

        public async Task<List<ContentMediaDto>> GetAllMediasByContentIdAsync(int id)
        {
            var getAllMediasByContentId = await _contentMediaRepository.GetAllMediasByContentIdAsync(id);

            return _mapper.Map<List<ContentMediaDto>>(getAllMediasByContentId);
        }

        public async Task<ContentMediaDto> GetContentMediaByIdAsync(int id)
        {
            var getContentMediaById = await _contentMediaRepository.GetContentMediaByIdAsync(id);

            return _mapper.Map<ContentMediaDto>(getContentMediaById);
        }

        public async Task<List<ContentMediaDto>> GetFeaturedContentMediaAysnc()
        {
            var featuredContentMedias = await _contentMediaRepository.GetFeaturedContentMediaAysnc();

            return _mapper.Map<List<ContentMediaDto>>(featuredContentMedias);
        }

        public async Task<List<ContentMediaDto>> GetLatestContentMediaAsync(int count)
        {
            var getLatestContentMedias = await _contentMediaRepository.GetLatestContentMediaAsync(count);

           return _mapper.Map<List<ContentMediaDto>>(getLatestContentMedias);
        }
    }
}