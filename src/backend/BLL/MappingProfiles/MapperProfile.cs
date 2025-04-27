using AutoMapper;
using BLL.Dtos;
using BLL.Dtos.CategoryDto;
using BLL.Dtos.ContentDto;
using BLL.Dtos.ContentMediaDto;
using BLL.Dtos.CustomerRequestsDtos;
using BLL.Dtos.RoleDtos;
using BLL.Dtos.UserReportsDto;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.MappingProfiles
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Category, CategoryDto>().ReverseMap();

            CreateMap<CreateUserDto, User>().ReverseMap();
            CreateMap<UpdateUserDto, User>().ReverseMap();
            CreateMap<LoginDto, User>().ReverseMap();

            CreateMap<Role, GetAllRolesDto>().ReverseMap();
            CreateMap<Role, GetRoleByIdDto>().ReverseMap();

            CreateMap<User,UserDto>().ReverseMap();

            CreateMap<CreateRoleDto, Role>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow)).ReverseMap();
            CreateMap<UpdateRoleDto, Role>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()).ReverseMap();

            CreateMap<UpdateLastLoginDto, User>().ReverseMap();

           // CreateMap<CustomerRequestDto, CustomerRequest>().ReverseMap();

            CreateMap<Content,ContentAddDto>().ReverseMap();
            CreateMap<Content, ContentDeleteDto>().ReverseMap();
            CreateMap<Content, ContentUpdateDto>().ReverseMap();

            CreateMap<ContentMedia, ContentMediaAddDto>().ReverseMap();
            CreateMap<ContentMedia, ContentMediaDeleteDto>().ReverseMap();
            CreateMap<ContentMedia, ContentMediaUpdateDto>().ReverseMap();
           // CreateMap<ContentMedia, ContentMediaGetByIdDto>().ReverseMap();
            CreateMap<ContentMedia, ContentMediaGetAllDto>().ReverseMap();

            CreateMap<Content, ContentGetByIdDto>()
                .ForMember(dest => dest.Medias, opt => opt.MapFrom(src => src.Medias))
                .ReverseMap();

            CreateMap<Content, ContentDto>()     
     .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
     .ForMember(dest => dest.Summary, opt => opt.MapFrom(src => src.Summary))
     .ForMember(dest => dest.Body, opt => opt.MapFrom(src => src.Body))
     .ForMember(dest => dest.IsFeatured, opt => opt.MapFrom(src => src.IsFeatured))
     .ForMember(dest => dest.ScheduledPublishDate, opt => opt.MapFrom(src => src.ScheduledPublishDate))
     .ForMember(dest => dest.PublishedDate, opt => opt.MapFrom(src => src.PublishedDate))
     .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
     .ForMember(dest => dest.ContentSource, opt => opt.MapFrom(src => src.ContentSource))
     .ForMember(dest => dest.Medias, opt => opt.MapFrom(src => src.Medias));

            CreateMap<ContentDto, Content>();
            CreateMap<ContentMedia, ContentMediaDto>().ReverseMap();





            CreateMap<Content, ContentGetByIdDto>()
                .ForMember(dest => dest.Medias, opt => opt.MapFrom(src => src.Medias))
                .ReverseMap();

            CreateMap<Content, ContentGetAllDto>()
                .ForMember(dest => dest.Medias, opt => opt.MapFrom(src => src.Medias))
                .ReverseMap();

            CreateMap<User, ActiveUsersDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"Ad: {src.FirstName} {src.LastName}"));
            //CreateMap<User, DetaileduserReportDto>().ReverseMap();



            // CustomerRequestAddDto → CustomerRequest
            CreateMap<CustomerRequestAddDto, CustomerRequest>()
                .ForMember(dest => dest.Status, opt => opt.Ignore()) // Status DTO'dan gelmemeli
                .ForMember(dest => dest.Customer, opt => opt.Ignore()) // Müşteri bilgisi User tablosundan alınmalı
                .ForMember(dest => dest.AssignedUser, opt => opt.Ignore()).ReverseMap(); // Atanmış müşteri temsilcisi dışarıdan gelmemeli

            // CustomerRequestUpdateDto → CustomerRequest
            CreateMap<CustomerRequestUpdateDto, CustomerRequest>()
                .ForMember(dest => dest.Customer, opt => opt.Ignore()) // Müşteri bilgisi güncellenmemeli
                .ForMember(dest => dest.AssignedUser, opt => opt.Ignore()); // AssignedUser sadece AssignedTo ID üzerinden atanmalı

            // CustomerRequest → CustomerRequestResponseDto
            CreateMap<CustomerRequest, CustomerRequestResponseDto>()
                .ForMember(dest => dest.AssignedUserName, opt => opt.MapFrom(src => src.AssignedUser != null ? src.AssignedUser.UserName : null))
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer.FirstName))
                .ForMember(dest => dest.CustomerSurname, opt => opt.MapFrom(src => src.Customer.LastName))
                .ForMember(dest => dest.CustomerUsername, opt => opt.MapFrom(src => src.Customer.UserName))
                .ForMember(dest => dest.CustomerEmail, opt => opt.MapFrom(src => src.Customer.Email));

            // CustomerRequest → CustomerRequestDto (Listeleme için kullanılabilir)
            CreateMap<CustomerRequest, CustomerRequestDto>()
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer.FirstName))
                .ForMember(dest => dest.CustomerSurname, opt => opt.MapFrom(src => src.Customer.LastName))
                .ForMember(dest => dest.CustomerUsername, opt => opt.MapFrom(src => src.Customer.UserName))
                .ForMember(dest => dest.CustomerEmail, opt => opt.MapFrom(src => src.Customer.Email))
                .ForMember(dest => dest.AssignedUserName, opt => opt.MapFrom(src => src.AssignedUser != null ? src.AssignedUser.UserName : null)).ReverseMap();



          



            CreateMap<IEnumerable<User>, UserReportDto>()
                .ForMember(dest => dest.TotalUsers, opt => opt.MapFrom(src => src.Count()))
                .ForMember(dest => dest.ActiveUsers, opt => opt.MapFrom(src => src.Count(u => u.IsActive)))
                .ForMember(dest => dest.InactiveUsers, opt => opt.MapFrom(src => src.Count(u => !u.IsActive)))
                .ForMember(dest => dest.UsersCreatedThisMonth, opt => opt.MapFrom(src => src.Count(u => u.CreatedAt.Month == DateTime.UtcNow.Month && u.CreatedAt.Year == DateTime.UtcNow.Year)))
            .ForMember(dest => dest.AdminUsers, opt => opt.MapFrom(src => src.Count(u => u.IsAdmin == true)));

            CreateMap<User, DetaileduserReportDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"Adı : {src.FirstName} Soyadı : {src.LastName}"))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
                .ForMember(dest => dest.LastLoginDate, opt => opt.MapFrom(src => src.LastLoginDate));


        }
    }
}
