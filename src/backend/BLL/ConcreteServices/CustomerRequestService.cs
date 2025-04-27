using AutoMapper;
using BLL.AbstractServices;
using BLL.Dtos.CustomerDto;
using BLL.Dtos.CustomerRequestsDtos;
using BLL.Dtos.UserReportsDto;
using DAL.Data;
using DAL.Entities;
using DAL.Repositories.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;

namespace BLL.ConcreteServices
{
    public class CustomerRequestService : ICustomerRequestService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly IGenericRepository<Category> _genericCategoryRepository;
        private readonly IGenericRepository<CustomerRequest> _genericCustomerRequestRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public CustomerRequestService(IGenericRepository<CustomerRequest> genericRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor, IUserRepository userRepository, IGenericRepository<Category> genericCategoryRepository)
        {
            _genericCustomerRequestRepository = genericRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
            _genericCategoryRepository = genericCategoryRepository;
        }
        public async Task<CustomerRequestDto> CustomerRequestAddAsync(CustomerRequestAddDto customerRequestAddDto)
        {
            // Aynı talebin daha önce oluşturulup oluşturulmadığını kontrol et
            //bool getAllRequests = await _genericCustomerRequestRepository
            //    .GetAll()
            //    .AnyAsync(x => x.UserId == customerRequestAddDto.UserId
            //                   && x.RequestDetails == customerRequestAddDto.RequestDetails
            //                   && x.AssignedTo != null);

            //if (getAllRequests)
            //{
            //    throw new InvalidOperationException("Bu talep zaten oluşturulmuştur!");
            //}

            // Kullanıcıyı veritabanında bul
            var customers = await _userRepository.GetByIdAsync(customerRequestAddDto.UserId);
            if (customers == null)
            {
                throw new KeyNotFoundException("Kullanıcı bulunamadı!");
            }

            // DTO'dan Entity'ye dönüşüm yap
            var addRequest = _mapper.Map<CustomerRequest>(customerRequestAddDto);
            addRequest.UserId = customerRequestAddDto.UserId;

            // AssignedUser'ı bul ve ata
            addRequest.AssignedUser = customers;
            if (addRequest.AssignedUser == null)
            {
                throw new KeyNotFoundException("Belirtilen kullanıcı bulunamadı.");
            }

            // Kullanıcı bilgilerini ata
            addRequest.CustomerName = customers.FirstName;
            addRequest.CustomerSurname = customers.LastName;
            addRequest.CustomerUsername = customers.UserName;
            addRequest.CustomerEmail = customers.Email;

            // Eğer teklif talebi varsa, ilgili bilgileri ata
            if (customerRequestAddDto.IsProposalRequest == true)
            {
                addRequest.IsProposalRequest = true;
                addRequest.EstimatedPrice = customerRequestAddDto.EstimatedPrice;
                addRequest.ProposalDetails = customerRequestAddDto.ProposalDetails;
            }

            // Eğer AssignedTo boşsa, en az talep alan bir admini otomatik olarak ata
            if (string.IsNullOrEmpty(customerRequestAddDto.AssignedTo))
            {
                var collectRequests = await _userRepository.GetAll()
                    .Where(u => u.RoleName == "Admin")
                    .OrderBy(u => u.CustomerRequests.Count)
                    .FirstOrDefaultAsync();

                addRequest.AssignedTo = collectRequests?.UserName;
            }

            // Talebi veritabanına ekle
            await _genericCustomerRequestRepository.AddAsync(addRequest);
            await _genericCustomerRequestRepository.SaveChangesAsync(); // Değişiklikleri kaydet!

            // DTO olarak geri döndür
            return _mapper.Map<CustomerRequestDto>(addRequest);
        }


        public async Task<CustomerRequestResponseDto> CustomerRequestAssignToAsync(int id)
        {
            var getCustById = await _genericCustomerRequestRepository.GetAll().FirstOrDefaultAsync(c => c.Id == id && c.AssignedTo == null);

            if (getCustById == null)
            {
                throw new KeyNotFoundException("Böyle bir kullanıcı bulunamadı!");
            }



            //var getById = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            //var userRole = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;

            //if (string.IsNullOrEmpty(getById) || (userRole != "Admin" && userRole != "SupportAgent"))
            //{
            //    throw new UnauthorizedAccessException("Bu işlemi yapmak için yetkiniz yok!");
            //}

            string token = null;

            StringValues bearerToken;

            if (_httpContextAccessor.HttpContext.Request.Headers.TryGetValue("Authorize", out bearerToken))
            {
                token = bearerToken.FirstOrDefault()?.Replace("Bearer ", string.Empty);
            }

            if (!string.IsNullOrEmpty(token))
            {
                string getById = "";
                // TODO: jwt access token parse edip, içinden user id alınacak
                getCustById.AssignedTo = getById;
            }



            _genericCustomerRequestRepository.Update(getCustById);

            return _mapper.Map<CustomerRequestResponseDto>(getCustById);

        }

        public async Task<bool> CustomerRequestDeleteAsync(int id)
        {
            await _genericCustomerRequestRepository.DeleteAsync(id);

            return true;

        }

        public async Task<CustomerRequestResponseDto> CustomerRequestGetByIdAsync(int id)
        {
            var getById = await _genericCustomerRequestRepository.GetByIdAsync(id);

            if (getById == null)
            {
                throw new KeyNotFoundException("Böyle bir destek talebi bulnmamaktadır!");
            }

            var response = _mapper.Map<CustomerRequestResponseDto>(getById);

            if (getById.AssignedUser != null)
            {
                response.AssignedUserName = getById.AssignedUser.UserName;
            }

            return response;

        }

        public async Task<CustomerRequestDto> CustomerRequestGetByUserIdAsync(string id)
        {
            var getByIdCustReq = await _genericCustomerRequestRepository.GetAll().FirstOrDefaultAsync(x => x.UserId == id);

            if (getByIdCustReq == null)
            {
                return null;

            }
            return _mapper.Map<CustomerRequestDto>(getByIdCustReq);

        }

        public async Task<List<CustomerRequestDto>> CustomerRequestGetUnassignedAsync()
        {
            var getAssigned = await _genericCustomerRequestRepository.GetAll().Where(x => x.AssignedTo == null).Select(x => new CustomerRequestDto
            {
                Id = x.Id,
                AssignedTo = x.AssignedTo,
                AssignedUserName = x.AssignedUser == null ? x.AssignedUser.UserName : "Kullanıcı Talebi Yoktur!",
                CustomerEmail = x.CustomerEmail,
                CustomerName = x.CustomerName,
                CustomerSurname = x.CustomerSurname,
                CustomerUsername = x.CustomerUsername,
                RequestDetails = x.RequestDetails,
                IsProposalRequest = x.IsProposalRequest,
                EstimatedPrice = x.EstimatedPrice,
                ProposalDetails = x.ProposalDetails,
            }).ToListAsync();

            return getAssigned;
        }

        public async Task<List<CustomerRequestDto>> CustomerRequestGetByStatusAsync(CustomerRequestDto customerRequestDto)
        {
            var getAllCustomerStatus = await _genericCustomerRequestRepository.GetAll().Where(x => x.Status == DAL.Enums.RequestStatus.Oluşturuldu || x.Status == DAL.Enums.RequestStatus.DevamEdiyor).Select(x => new CustomerRequestDto
            {
                CustomerName = x.CustomerName,
                CustomerSurname = x.CustomerSurname,
                CustomerUsername = x.CustomerUsername,
                CustomerEmail = x.CustomerEmail,
                AssignedTo = x.AssignedTo,
                AssignedUserName = x.AssignedUser != null ? x.AssignedUser.UserName : null,
                RequestDetails = x.RequestDetails,
                Status = x.Status,
                IsProposalRequest = x.IsProposalRequest,
                EstimatedPrice = x.EstimatedPrice,
                ProposalDetails = x.ProposalDetails,

            }).ToListAsync();

            return getAllCustomerStatus;
        }

        public async Task<CustomerRequestUpdateDto> CustomerRequestUpdateAsync(int id, CustomerRequestUpdateDto customerRequestUpdateDto)
        {

            var updatedCustomerRequest = await _genericCustomerRequestRepository.GetByIdAsync(id);

            if (updatedCustomerRequest == null)
            {
                throw new KeyNotFoundException("Böyle bir talep bulunamadı!");
            }

            _mapper.Map(customerRequestUpdateDto, updatedCustomerRequest);

            if (!customerRequestUpdateDto.Status.HasValue)
            {
                updatedCustomerRequest.Status = DAL.Enums.RequestStatus.DevamEdiyor;
            }

            if (!string.IsNullOrWhiteSpace(customerRequestUpdateDto.RequestDetails))
            {
                updatedCustomerRequest.RequestDetails = customerRequestUpdateDto.RequestDetails;
            }

            if (!string.IsNullOrWhiteSpace(customerRequestUpdateDto.AssignedTo))
            {
                updatedCustomerRequest.AssignedTo = customerRequestUpdateDto.AssignedTo;
            }

            if (customerRequestUpdateDto.IsProposalRequest.HasValue)
            {
                updatedCustomerRequest.IsProposalRequest = customerRequestUpdateDto.IsProposalRequest.Value;
            }

            if (customerRequestUpdateDto.EstimatedPrice.HasValue)
            {
                updatedCustomerRequest.EstimatedPrice = customerRequestUpdateDto.EstimatedPrice.Value;
            }

            if (!string.IsNullOrWhiteSpace(customerRequestUpdateDto.ProposalDetails))
            {
                updatedCustomerRequest.ProposalDetails = customerRequestUpdateDto.ProposalDetails;
            }


            updatedCustomerRequest.Status = customerRequestUpdateDto.Status.Value;

            await _genericCustomerRequestRepository.UpdateAsync(id, updatedCustomerRequest);

            return _mapper.Map<CustomerRequestUpdateDto>(customerRequestUpdateDto);

        }

        public async Task<List<CustomerRequestResponseDto>> GetAllCustomerRequest()
        {

            return await _genericCustomerRequestRepository.GetAll()
                .Select(x => new CustomerRequestResponseDto
                {
                    CustomerName = x.CustomerName,
                    CustomerSurname = x.CustomerSurname,
                    CustomerUsername = x.CustomerUsername,
                    CustomerEmail = x.CustomerEmail,
                    AssignedTo = x.AssignedTo,
                    AssignedUserName = x.AssignedUser != null ? x.AssignedUser.UserName : "Atanmamış",
                    RequestDetails = x.RequestDetails,
                    Status = x.Status,
                    ProposalDetails = x.ProposalDetails,
                    EstimatedPrice = x.EstimatedPrice,
                    IsProposalRequest = x.IsProposalRequest
                })
                .ToListAsync();
        }

        public async Task<List<CustomerRequestDto>> GetAllRequestsAsync(string filter)
        {
            var query = _genericCustomerRequestRepository.GetAll()
                .Select(request => new CustomerRequestDto
                {
                    Id = request.Id,
                    Title = request.Title,
                    Description = request.Description,
                    CategoryId = request.CategoryId != null ? request.CategoryId.Value : null,
                    CategoryName = request.Category != null ? request.Category.Name : null,
                    Status = request.Status,
                    CreatedDate = request.CreatedDate,
                    CustomerName = request.Customer != null ? request.Customer.UserName : "Bilinmiyor",
                    CustomerEmail = request.Customer != null ? request.Customer.Email : "Bilinmiyor",
                    ProposalDetails = request.ProposalDetails,
                    EstimatedPrice = request.EstimatedPrice
                });

            if (!string.IsNullOrEmpty(filter))
            {
                query = query.Where(x => x.Title.Contains(filter)
                    || x.Description.Contains(filter)
                    || x.CategoryName.Contains(filter));
            }                                     

            return await query.ToListAsync();
        }


        public async Task<CustomerRequestDto> GetRequestByIdAsync(int id)
        {
            var request = await _genericCustomerRequestRepository.GetAll()
                .Include(r => r.Category)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (request == null) return null;

            return new CustomerRequestDto
            {
                Id = request.Id,
                Title = request.Title,
                Description = request.Description,
                CategoryId = (int)request.CategoryId,
                CategoryName = request.Category.Name,
                Status = request.Status,
                CreatedDate = request.CreatedDate,
                CustomerName = request.Customer?.UserName,
                CustomerEmail = request.Customer?.Email
            };
        }

        public async Task<bool> UpdateRequestAsync(int id, UpdateCustomerRequestDto requestDto)
        {
            var request = await _genericCustomerRequestRepository.GetByIdAsync(id);
            if (request == null) return false;

            var category = await _genericCategoryRepository.GetAll().FirstOrDefaultAsync();

            request.Title = requestDto.Title;
            request.Description = requestDto.Description;
            //request.Status = requestDto.Status;
            // TODO: hangi status olmalı?
            request.Status = DAL.Enums.RequestStatus.DevamEdiyor;
            request.CategoryId = category.Id;

            _genericCustomerRequestRepository.Update(request);
            await _genericCustomerRequestRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteRequestAsync(int id)
        {
            var request = await _genericCustomerRequestRepository.GetByIdAsync(id);
            if (request == null) return false;

            _genericCustomerRequestRepository.Remove(request);
            await _genericCustomerRequestRepository.SaveChangesAsync();
            return true;
        }

        public async Task<List<CustomerRequestDto>> GetRequestsByCategoryAsync(int categoryId)
        {
            return await _genericCustomerRequestRepository.GetAll()
                .Where(request => request.CategoryId == categoryId)
                .Select(request => new CustomerRequestDto
                {
                    Id = request.Id,
                    Title = request.Title,
                    Description = request.Description,
                    CategoryId = (int)request.CategoryId,
                    CategoryName = request.Category.Name,
                    Status = request.Status,
                    CreatedDate = request.CreatedDate
                }).ToListAsync();
        }

        public async Task<CustomerRequestDto> CreateRequestAsync(CreateCustomerRequestDto requestDto, string userId)
        {
            var newRequest = new CustomerRequest
            {
                Title = requestDto.Title,
                Description = requestDto.Description,
                CategoryId = requestDto.CategoryId,
                UserId = userId,
                Status = DAL.Enums.RequestStatus.Oluşturuldu,
                CreatedDate = DateTime.UtcNow
            };

            await _genericCustomerRequestRepository.AddAsync(newRequest);
            await _genericCustomerRequestRepository.SaveChangesAsync();

            // Kullanıcı bilgilerini yeniden yükleyelim
            var savedRequest = await _genericCustomerRequestRepository.GetAll()
                .Include(r => r.Customer)  // User bilgilerini çek
                .FirstOrDefaultAsync(r => r.Id == newRequest.Id);

            // Kaydedilen veriyi geri döndür
            return new CustomerRequestDto
            {
                Id = savedRequest.Id,
                Title = savedRequest.Title,
                Description = savedRequest.Description,
                CategoryId = (int)savedRequest.CategoryId,
                Status = savedRequest.Status,
                CreatedDate = savedRequest.CreatedDate,
                CustomerName = savedRequest.Customer?.UserName,
                CustomerEmail = savedRequest.Customer?.Email
            };
        }
        
        public async Task<CustomerRequestDto> CreatePurposalRequestAsync(CustomerRequestAddDto customerRequestAdd)
        {

            //var isExist = await _genericCustomerRequestRepository
            //    .GetAll()
            //    .AnyAsync(x => x.UserId == customerRequestAdd.UserId && x.ProposalDetails == customerRequestAdd.ProposalDetails);

            //if (isExist)
            //{
            //    throw new Exception("Böyle bir teklif zaten mevcuttur!");
            //}
            var customerList = await _userRepository.GetAll().ToListAsync(); 
            var customer = customerList.FirstOrDefault(x => x.Id == customerRequestAdd.UserId);

            if (customer == null)
            {
                throw new KeyNotFoundException($"Böyle bir kullanıcı bulunamadı! UserId: {customerRequestAdd.UserId}");
            }

            var category = await _genericCategoryRepository.GetAll().FirstOrDefaultAsync();




            var purposalRequest = _mapper.Map<CustomerRequest>(customerRequestAdd);

            purposalRequest.UserId = customerRequestAdd.UserId;
            purposalRequest.CategoryId = category.Id;

            purposalRequest.IsProposalRequest = customerRequestAdd.IsProposalRequest;
            purposalRequest.ProposalDetails = customerRequestAdd.ProposalDetails;
            purposalRequest.EstimatedPrice = customerRequestAdd.EstimatedPrice;
            purposalRequest.CustomerName = customer.FirstName;
            purposalRequest.CustomerSurname = customer.LastName;


            if (string.IsNullOrWhiteSpace(purposalRequest.CustomerUsername) || string.IsNullOrWhiteSpace(purposalRequest.CustomerEmail))
            {
                purposalRequest.CustomerUsername = customerRequestAdd.CustomerUsername;
                purposalRequest.CustomerEmail = customerRequestAdd.CustomerEmail;
            }


            if (string.IsNullOrEmpty(customerRequestAdd.AssignedTo))
            {
                var collectRequests = await _userRepository.GetAll()
                    .Where(u => u.RoleName == "Admin")
                    .OrderBy(u => u.CustomerRequests.Count)
                    .FirstOrDefaultAsync();

                if (collectRequests != null)
                {
                    purposalRequest.AssignedTo = collectRequests.Id;                    
                }

            }

            await _genericCustomerRequestRepository.AddAsync(purposalRequest);

            return _mapper.Map<CustomerRequestDto>(purposalRequest);
        }

    }
}
