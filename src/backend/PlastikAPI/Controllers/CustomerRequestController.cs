using BLL.AbstractServices;
using BLL.ConcreteServices;
using BLL.Dtos.CustomerRequestsDtos;
using BLL.Dtos.CustomerDto;
using BLL.Dtos.UserReportsDto;
using BLL.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.IdentityModel.Tokens.Jwt;
using Newtonsoft.Json;

namespace PlastikAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerRequestController : ControllerBase
    {
        private readonly ICustomerRequestService _customerRequestService;

        public CustomerRequestController(ICustomerRequestService customerRequestService)
        {
            _customerRequestService = customerRequestService ?? throw new ArgumentNullException(nameof(customerRequestService));

        }

        [HttpPost("CreateCustomerRequest")]
        public async Task<IActionResult> CreateCustomerRequest(CustomerRequestAddDto requestDto)
        {
            try
            {
                if (!HttpContext.Request.Headers.TryGetValue("Authorization", out var authHeader) || string.IsNullOrEmpty(authHeader))
                {
                    Console.WriteLine("[ERROR] Authorization token eksik!");
                    return Unauthorized(new { message = "Yetkilendirme başarısız. Token eksik." });
                }

                var authToken = authHeader.ToString().Replace("Bearer ", string.Empty);
                var handler = new JwtSecurityTokenHandler();
                var jwtSecurityToken = handler.ReadJwtToken(authToken);

                var userIdClaim = jwtSecurityToken.Claims.FirstOrDefault(claim => claim.Type == "UserId");
                if (userIdClaim == null || string.IsNullOrEmpty(userIdClaim.Value))
                {
                    Console.WriteLine("[ERROR] JWT içinde 'UserId' bulunamadı!");
                    return Unauthorized(new { message = "Kullanıcı kimliği doğrulanamadı." });
                }

                var userId = userIdClaim.Value;

                // 📌 **Gelen requestDto NULL mı?**
                if (requestDto == null)
                {
                    Console.WriteLine("[ERROR] API'ye null veri gönderildi.");
                    return BadRequest(new { message = "Gönderilen talep verisi boş olamaz." });
                }

                // 📌 **API'ye gelen veriyi ekrana yazdıralım (Debug Mode)**
                Console.WriteLine("[DEBUG] API'ye gelen veri: " + JsonConvert.SerializeObject(requestDto));

                // 📌 **UserId ekleniyor**
                requestDto.UserId = userId;

                // 📌 **RequestDetails NULL olamaz!**
                //if (string.IsNullOrWhiteSpace(requestDto.RequestDetails))
                //{
                //    Console.WriteLine("[ERROR] RequestDetails boş olarak API'ye gönderildi.");
                //    return BadRequest(new { message = "RequestDetails boş olamaz." });
                //}

                // 📌 **Model Validasyonu**
                if (!ModelState.IsValid)
                {
                    Console.WriteLine("[ERROR] Model geçersiz!");
                    return BadRequest(new
                    {
                        message = "Geçersiz talep verisi.",
                        errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
                    });
                }

                var createdRequest = await _customerRequestService.CustomerRequestAddAsync(requestDto);
                return CreatedAtAction(nameof(GetCustomerRequestById), new { id = createdRequest.Id }, createdRequest);
            }
            catch (KeyNotFoundException ex)
            {
                Console.WriteLine("[ERROR] Kayıt bulunamadı: " + ex.Message);
                return NotFound(new { message = "Kayıt bulunamadı: " + ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine("[ERROR] İşlem hatası: " + ex.Message);
                return BadRequest(new { message = "İşlem hatası: " + ex.Message });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Beklenmeyen bir hata oluştu: {ex.Message} \nStackTrace: {ex.StackTrace}");
                return StatusCode(500, new { message = "Beklenmeyen bir hata oluştu. Lütfen tekrar deneyin.", error = ex.Message });
            }
        }



        [HttpGet("GetCustomerRequestById/{id}")]
        public async Task<IActionResult> GetCustomerRequestById(int id)
        {
            var request = await _customerRequestService.CustomerRequestGetByIdAsync(id);
            if (request == null)
            {
                return NotFound("Müşteri talebi bulunamadı.");
            }
            return Ok(request);
        }

        [HttpGet("GetRequestByUserId/{userId}")]
        public async Task<IActionResult> GetCustomerRequestsByUserId(string userId)
        {
            var requests = await _customerRequestService.CustomerRequestGetByUserIdAsync(userId);
            if (requests == null)
            {
                return NotFound("Bu kullanıcıya ait müşteri talebi bulunamadı.");
            }
            return Ok(requests);
        }

        [HttpPut("UpdateRequest/{id}")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCustomerRequest(int id, [FromBody] CustomerRequestUpdateDto requestDto)
        {
            if (requestDto == null)
            {
                return BadRequest("Geçersiz güncelleme verisi.");
            }

            var updatedRequest = await _customerRequestService.CustomerRequestUpdateAsync(id, requestDto);
            return Ok(updatedRequest);
        }

        [HttpDelete("DeleteRequest/{id}")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCustomerRequest(int id)
        {
            var success = await _customerRequestService.CustomerRequestDeleteAsync(id);
            if (!success)
            {
                return NotFound("Silinecek talep bulunamadı.");
            }
            return NoContent();
        }

        [HttpPut("AssignCustomer/{id}")]
        [Authorize(Roles = "Admin,SupportAgent")]
        public async Task<IActionResult> AssignCustomerRequest(int id)
        {
            var assignedRequest = await _customerRequestService.CustomerRequestAssignToAsync(id);
            return Ok(assignedRequest);
        }

        [HttpGet("GetUnassigned")]
        //[Authorize(Roles = "Admin,SupportAgent")]
        public async Task<IActionResult> GetUnassignedRequests()
        {
            var requests = await _customerRequestService.CustomerRequestGetUnassignedAsync();
            return Ok(requests);
        }

        [HttpGet("GetCustomerStatus")]
        //[Authorize(Roles = "Admin,SupportAgent")]
        public async Task<IActionResult> GetCustomerRequestsByStatus([FromQuery] CustomerRequestDto requestDto)
        {
            var requests = await _customerRequestService.CustomerRequestGetByStatusAsync(requestDto);
            return Ok(requests);
        }
        [HttpGet("GetAllCustomerRequests")]
        public async Task<IActionResult> GetAllCustomerRequests()
        {
            try
            {
                var getAllCustomerRequests = await _customerRequestService.GetAllCustomerRequest();

                if (getAllCustomerRequests == null || !getAllCustomerRequests.Any())
                {
                    return NotFound("Hiçbir müşteri talebi bulunmamaktadır!");
                }
                return Ok(getAllCustomerRequests);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [Authorize(Roles = "Admin,Müşteri Temsilcisi")]
        [HttpGet]
        public async Task<IActionResult> GetAllRequests([FromQuery] string? filter)
        {
            try
            {
                var requests = await _customerRequestService.GetAllRequestsAsync(filter);
                if (requests == null || !requests.Any())
                {
                    return NotFound(new { Message = "Henüz müşteri talebi bulunamadı." });
                }
                return Ok(requests);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Sunucu hatası oluştu.", Error = ex.Message });
            }
        }


        [Authorize(Roles = "Admin,Müşteri Temsilcisi")]
        [HttpGet("GetRequestById/{id}")]
        public async Task<IActionResult> GetRequestById(int id)
        {
            var request = await _customerRequestService.GetRequestByIdAsync(id);
            if (request == null)
            {
                return NotFound(new { Message = "Talep bulunamadı." });
            }
            return Ok(request);
        }

        [Authorize(Roles = "Admin,Müşteri Temsilcisi")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRequest(int id, [FromBody] UpdateCustomerRequestDto requestDto)
        {
            if (!Enum.TryParse(typeof(CustomerRequestStatus), requestDto.Status, out _))
            {
                Console.WriteLine($"[API ERROR] Geçersiz durum: {requestDto.Status}");
                return BadRequest(new { Message = "Geçersiz durum. Sadece 'Beklemede', 'İşlemde' veya 'Tamamlandı' olabilir." });
            }

            try
            {
                Console.WriteLine($"[API] Güncelleme işlemi başladı. ID: {id}, Yeni Status: {requestDto.Status}");

                var existingRequest = await _customerRequestService.GetRequestByIdAsync(id);
                if (existingRequest == null)
                {
                    Console.WriteLine("[API ERROR] Talep bulunamadı.");
                    return NotFound(new { Message = "Talep bulunamadı." });
                }

                var updatedRequest = await _customerRequestService.UpdateRequestAsync(id, requestDto);
                if (!updatedRequest)
                {
                    Console.WriteLine("[API ERROR] Talep güncellenemedi.");
                    return BadRequest(new { Message = "Talep güncellenemedi." });
                }

                Console.WriteLine("[API] Talep başarıyla güncellendi.");
                return Ok(new { Message = "Talep başarıyla güncellendi.", UpdatedRequest = requestDto });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[API ERROR] Sunucu hatası: {ex.Message}");
                return StatusCode(500, new { Message = "Sunucu hatası oluştu.", Error = ex.Message });
            }
        }




        [Authorize(Roles = "Admin,Müşteri Temsilcisi")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRequest(int id)
        {
            var result = await _customerRequestService.DeleteRequestAsync(id);
            if (!result)
            {
                return NotFound(new { Message = "Talep bulunamadı." });
            }
            return Ok(new { Message = "Talep başarıyla silindi." });
        }


        [Authorize(Roles = "User")]
        [HttpPost]
        public async Task<IActionResult> CreateRequest([FromBody] CreateCustomerRequestDto requestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Kullanıcının kimliğini almak
                var userId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
                if (userId == null)
                {
                    return Unauthorized(new { Message = "Kimlik doğrulama başarısız." });
                }

                // Kullanıcıya bağlı talep oluşturma
                var newRequest = await _customerRequestService.CreateRequestAsync(requestDto, userId);

                // Başarıyla oluşturulan talebi döndür
                return CreatedAtAction(nameof(GetRequestById), new { id = newRequest.Id }, newRequest);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Sunucu hatası oluştu.", Error = ex.Message });
            }
        }
        [HttpPost("CreatePurposalRequest")]
        public async Task<IActionResult> CreatePurposalRequest([FromBody] CustomerRequestAddDto customerRequestAddDto)
        {
            if (customerRequestAddDto == null)
            {
                return BadRequest("Geçersiz istek! Boş veri gönderilemez.");
            }

            try
            {
                var authToken = HttpContext.Request.Headers.Authorization.ToString();
                authToken = authToken.Replace("Bearer ", string.Empty);

                var handler = new JwtSecurityTokenHandler();
                var jwtSecurityToken = handler.ReadJwtToken(authToken);

                var userId = jwtSecurityToken.Claims.First(claim => claim.Type == "UserId").Value;
                var userEmail = jwtSecurityToken.Claims.First(claim => claim.Type == "Email").Value;
                var userName = jwtSecurityToken.Claims.First(claim => claim.Type == "sub").Value;

                customerRequestAddDto.UserId = userId;
                customerRequestAddDto.CustomerEmail = userEmail;
                customerRequestAddDto.CustomerUsername = userName;

                var result = await _customerRequestService.CreatePurposalRequestAsync(customerRequestAddDto);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }

}
