using AutoMapper;
using BLL.AbstractServices;
using BLL.Dtos.CategoryDto;
using BLL.Dtos.ContentDto;
using BLL.Dtos.ContentMediaDto;
using BLL.Models;
using DAL.Data;
using DAL.Entities;
using DAL.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Http;
using DAL.Enums;


namespace BLL.ConcreteServices

{   //ONR++ 29012025/22:20
    public class ContentService : IContentService
    {
        private readonly IGenericRepository<Content> _genericRepository;
        private readonly IMapper _mapper;
        private readonly AppDbContext _db;

        public ContentService(IGenericRepository<Content> genericRepository, IMapper mapper, AppDbContext db)
        {
            _genericRepository = genericRepository;
            _mapper = mapper;
            _db = db;
        }
        public async Task<ContentDto> AddContentAsync(ContentAddDto contentAddDto)
        {

            if (contentAddDto == null)
                throw new ArgumentNullException(nameof(contentAddDto));

            var content = _mapper.Map<Content>(contentAddDto);

            if (string.IsNullOrWhiteSpace(contentAddDto.Slug))
            {
                content.Slug = GenerateSlug(content.Title);
            }
            else
            {
                content.Slug = contentAddDto.Slug;
            }

            

            if (contentAddDto.MediaFiles != null && contentAddDto.MediaFiles.Any())
            {
                var mediaList = new List<ContentMedia>();

                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                foreach (var mediaFile in contentAddDto.MediaFiles)
                {
                    if (mediaFile.Length > 0)
                    {
                        var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(mediaFile.FileName)}";
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await mediaFile.CopyToAsync(stream);
                        }

                        mediaList.Add(new ContentMedia
                        {
                            Url = $"/uploads/{uniqueFileName}",
                            Type = MediaType.Image,
                            Content = content,
                            AltText = "Null",
                            Caption = "Media"

                        });
                    }
                }

                content.Medias = mediaList;
            }

            await _genericRepository.AddAsync(content);

            return _mapper.Map<ContentDto>(content);

        }

        private string GenerateSlug(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                return "default-slug";

            var replacements = new Dictionary<string, string>
    {
        {"ı", "i"}, {"ğ", "g"}, {"ü", "u"}, {"ş", "s"}, {"ö", "o"}, {"ç", "c"}
    };

            var sb = new StringBuilder(title.ToLower());

            foreach (var kvp in replacements)
                sb.Replace(kvp.Key, kvp.Value);

            return System.Text.RegularExpressions.Regex.Replace(sb.ToString(), @"[^a-z0-9\s-]", "")
                .Replace(" ", "-")
                .Trim('-');
        }

        public async Task<bool> DeleteContentAsync(int id)
        {
            var content = await _genericRepository.GetByIdAsync(id);

            if (content == null)
            {
                throw new KeyNotFoundException("Silinecek içerik bulunamadı!");
            }

            if (content.Medias != null && content.Medias.Any())
            {
                foreach (var media in content.Medias.ToList())
                {
                    _db.Entry(media).State = EntityState.Deleted; 
                }
            }
            //if (content.Medias != null && content.Medias.Any())
            //{
            //    foreach (var media in content.Medias)
            //    {
            //        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", media.Url.TrimStart('/'));
            //        if (File.Exists(filePath))
            //        {
            //            File.Delete(filePath);
            //        }
            //    }

            //    content.Medias.Clear();
            //}

            await _genericRepository.DeleteAsync(id);

            return true;

        }
        public async Task<List<ContentGetAllDto>> GetAllContentAsync()
        {
            return _mapper.Map<List<ContentGetAllDto>>(await _genericRepository.GetAllAsync());

        }

        public async Task<List<ContentGetByIdDto>> GetContentByCategoryAsync(int categoryId)
        {

            var getContentByCategory = await _db.Contents.Where(x => x.Id == categoryId).OrderByDescending(pd => pd.PublishedDate).ToListAsync();

            if (getContentByCategory == null)
            {
                throw new KeyNotFoundException("Böyle bir içerik bulunamadı!");
            }

            return _mapper.Map<List<ContentGetByIdDto>>(getContentByCategory);

        }

        public async Task<ContentGetByIdDto> GetContentByIdAsync(int id)
        {
            var content = await _genericRepository.GetByIdAsync(id);

            return _mapper.Map<ContentGetByIdDto>(content);

        }

        public async Task<List<ContentGetAllDto>> GetFeaturedContentAsync()
        {
            var getFeaturedContent = await _db.Contents.Where(x => x.IsFeatured == true).OrderByDescending(pd => pd.PublishedDate).Select(x => new ContentGetAllDto
            {
                Id = x.Id,
                Title = x.Title,
                Summary = x.Summary,
                PublishedDate = x.PublishedDate,
                //Category = new CategoryDto
                //{
                //    Id = x.Category.Id,
                //    Name = x.Category.Name,
                //}             
            }).ToListAsync();

            return _mapper.Map<List<ContentGetAllDto>>(getFeaturedContent);
        }

        public async Task<List<ContentGetAllDto>> GetLatestContentAsync(int count)
        {
            var getLatestContent = await _db.Contents.OrderByDescending(pd => pd.PublishedDate).Take(count).Select(x => new ContentGetAllDto
            {
                Id = x.Id,
                Title = x.Title,
                Summary = x.Summary,
                PublishedDate = x.PublishedDate,
                IsFeatured = x.IsFeatured,
                //Category = new CategoryDto
                //{
                //    Id = x.Category.Id,
                //    Name = x.Category.Name,
                //},
                MediaUrl = x.Medias.FirstOrDefault().Url

            }).ToListAsync();

            return getLatestContent;
        }

        public async Task<PagedResult<ContentGetAllDto>> GetPaginatedContentsAsync(int page, int pageSize)
        {

            if (page <= 0 || pageSize <= 0)
            {
                throw new ArgumentException("Sayfa numarası ve boyutu pozitif olmalıdır!");
            }

            int totalCount = await _db.Contents.Where(x => x.PublishedDate <= DateTime.Now).CountAsync();

            var getPaginatedContent = await _db.Contents.OrderByDescending(pd => pd.PublishedDate).Skip((page - 1) * pageSize).Take(pageSize).Select(x => new ContentGetAllDto
            {
                Id = x.Id,
                Title = x.Title,
                Summary = x.Summary,
                PublishedDate = x.PublishedDate,
                IsFeatured = x.IsFeatured,
                //Category = new CategoryDto
                //{
                //    Id = x.Category.Id,
                //    Name = x.Category.Name,
                //}
            }).ToListAsync();

            return new PagedResult<ContentGetAllDto>
            {
                Items = getPaginatedContent,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }
        public async Task<List<ContentGetAllDto>> GetScheduledContentsAsync()
        {
            var getScheduledContents = await _db.Contents.OrderBy(sd => sd.ScheduledPublishDate >= DateTime.Now).Select(x => new ContentGetAllDto
            {
                Id = x.Id,
                Title = x.Title,
                Summary = x.Summary,
                ScheduledPublishDate = x.ScheduledPublishDate,
                IsFeatured = x.IsFeatured,
                //Category = new CategoryDto
                //{
                //    Id = x.Category.Id,
                //    Name = x.Category.Name,
                //}
            }).ToListAsync();

            return getScheduledContents;
        }

        async Task<bool> IContentService.UpdateContentAsync(int id, ContentUpdateDto contentUpdateDto)
        {
            {
                try
                {
                    var existingContent = await _genericRepository.GetByIdAsync(id);

                    if (existingContent == null)
                    {
                        throw new KeyNotFoundException("Güncellenecek içerik bulunamadı!");
                    }

                    if (contentUpdateDto.Id != 0 && contentUpdateDto.Id != id)
                    {
                        throw new ArgumentException("Gönderilen ID ile URL'deki ID eşleşmiyor.");
                    }
                   
                    existingContent.Title = contentUpdateDto.Title;
                    existingContent.Slug = contentUpdateDto.Slug;
                    existingContent.Summary = contentUpdateDto.Summary;
                    existingContent.Body = contentUpdateDto.Body;                    
                    existingContent.UpdatedDate = DateTime.Now;
                    existingContent.IsFeatured = contentUpdateDto.IsFeatured;
                    existingContent.ContentSource = contentUpdateDto.ContentSource;                     
                    existingContent.Type = contentUpdateDto.MediaType;                    

                    if (contentUpdateDto.ScheduledPublishDate.HasValue)
                    {
                        existingContent.ScheduledPublishDate = contentUpdateDto.ScheduledPublishDate.Value;
                    }
                  
                    if (existingContent.Medias != null && existingContent.Medias.Any())
                    {

                        var mediaToRemove = existingContent.Medias.ToList();

                        foreach (var media in mediaToRemove)
                        {
                            _db.Entry(media).State = EntityState.Deleted;
                        }
                    }
                   
                    if (contentUpdateDto.MediaFiles != null && contentUpdateDto.MediaFiles.Any())
                    {
                        var mediaList = new List<ContentMedia>();

                        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }

                        foreach (var mediaFile in contentUpdateDto.MediaFiles)
                        {
                            if (mediaFile.Length > 0)
                            {
                                var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(mediaFile.FileName)}";
                                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                                using (var stream = new FileStream(filePath, FileMode.Create))
                                {
                                    await mediaFile.CopyToAsync(stream);
                                }

                                mediaList.Add(new ContentMedia
                                {
                                    Url = "/uploads/" + uniqueFileName,
                                    Type = MediaType.Image,
                                    ContentId = existingContent.Id,
                                    AltText = string.IsNullOrEmpty(mediaFile.FileName) ? "No description" : Path.GetFileNameWithoutExtension(mediaFile.FileName)
                                });
                            }
                        }

                        if (existingContent.Medias == null)
                        {
                            existingContent.Medias = new List<ContentMedia>();
                        }

                        foreach (var media in mediaList)
                        {
                            existingContent.Medias.Add(media);
                        }
                    }
                    else if (contentUpdateDto.Medias != null && contentUpdateDto.Medias.Any())
                    {
                        
                        existingContent.Medias = _mapper.Map<List<ContentMedia>>(contentUpdateDto.Medias);
                    }
                    
                    _db.Entry(existingContent).State = EntityState.Modified;
                    await _genericRepository.UpdateAsync(existingContent.Id, existingContent);

                    return true;
                }
                catch (DbUpdateException dbEx)
                {
                    var innerExceptionMessage = dbEx.InnerException != null ? dbEx.InnerException.Message : dbEx.Message;
                    throw new Exception($"Veritabanı güncelleme hatası: {innerExceptionMessage}");
                }
                catch (Exception ex)
                {
                    throw new Exception($"Beklenmeyen hata oluştu: {ex.Message}");
                }

            }
        }

        public async Task<ContentGetByIdDto> GetContentBySlug(string slug)
        {
            var getSlug = await _db.Contents.FirstOrDefaultAsync(c => c.Slug == slug);

            if(slug == null)
            {
                throw new Exception("Bu slug ile içerik bulunamadı!");
            }

            return _mapper.Map<ContentGetByIdDto>(getSlug);
        }
    }
}