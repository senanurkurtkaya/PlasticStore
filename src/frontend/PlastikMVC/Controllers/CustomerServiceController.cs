using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PlastikMVC.AllDtos.CustomerRequestDtos;
using PlastikMVC.Client.Models.Request.CustomerRequest;
using PlastikMVC.Filters;
using PlastikMVC.HttpClientService.HttpClientForCustomerRequests;
using System.Net.Http.Headers;
using System.Text;

namespace PlastikMVC.Controllers
{
    public class CustomerServiceController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClientForCustomerRequests _customerRequestService;
        private readonly string _baseUrl;

        public CustomerServiceController(IHttpClientFactory httpClientFactory, IConfiguration configuration, HttpClientForCustomerRequests httpClientForCustomerRequests)
        {
            _httpClientFactory = httpClientFactory;
            _customerRequestService = httpClientForCustomerRequests;
            _baseUrl = configuration["ApiSettings:BaseUrl"].ToString();
        }


        [HttpGet]
        [PlastikAuth(Roles = "User")]
        public async Task<IActionResult> CreatePurposalRequest()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {

                return BadRequest("Teklif alınırken bir hata oluştu.");
            }
        }

        [HttpPost]
        [PlastikAuth(Roles = "User")]
        public async Task<IActionResult> CreatePurposalRequest(PurposalRequestViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var customerRequestDto = new CustomerRequestAddDto
            {
                IsProposalRequest = model.IsProposalRequest,
                EstimatedPrice = model.EstimatedPrice,
                ProposalDetails = model.ProposalDetails,
            };

            await _customerRequestService.CreatePurposalRequestAsync(customerRequestDto);

            return Redirect("/");

        }

    }

}
