using BLL.Dtos.CustomerRequestsDtos;
using BLL.Dtos.CustomerDto;
using BLL.Dtos.UserReportsDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.AbstractServices
{
    public interface ICustomerRequestService
    {
        Task<CustomerRequestDto> CustomerRequestAddAsync(CustomerRequestAddDto customerRequestAddDto);
        Task<bool> CustomerRequestDeleteAsync(int id);
        Task<CustomerRequestUpdateDto> CustomerRequestUpdateAsync(int id,CustomerRequestUpdateDto customerRequestUpdateDto);
        Task<CustomerRequestResponseDto> CustomerRequestGetByIdAsync(int id);
        Task<CustomerRequestDto> CustomerRequestGetByUserIdAsync(string id);
        Task<CustomerRequestResponseDto> CustomerRequestAssignToAsync(int id);
        Task<List<CustomerRequestDto>> CustomerRequestGetByStatusAsync(CustomerRequestDto customerRequestDto);
        Task<List<CustomerRequestDto>> CustomerRequestGetUnassignedAsync();
        Task<List<CustomerRequestResponseDto>> GetAllCustomerRequest();
        Task<List<CustomerRequestDto>> GetAllRequestsAsync(string filter);
        Task<CustomerRequestDto> GetRequestByIdAsync(int id);
        Task<bool> UpdateRequestAsync(int id, UpdateCustomerRequestDto requestDto);
        Task<bool> DeleteRequestAsync(int id);
        Task<List<CustomerRequestDto>> GetRequestsByCategoryAsync(int categoryId);
        Task<CustomerRequestDto> CreateRequestAsync(CreateCustomerRequestDto requestDto, string userId);
        Task<CustomerRequestDto> CreatePurposalRequestAsync(CustomerRequestAddDto customerRequestAdd);
    }

}
