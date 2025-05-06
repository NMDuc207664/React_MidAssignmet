using Library_API_2._0.Application.Dtos.BorrowingRequests.Request;
using Library_API_2._0.Application.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library_API_2._0.Api.Controller
{
    [Route("api/request")]
    [ApiController]
    public class BorrowingRequestController : ControllerBase
    {
        private readonly IBorrowingRequestService _service;
        public BorrowingRequestController(IBorrowingRequestService service)
        {
            _service = service;
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllRequestsAsync()
        {
            var requests = await _service.GetAllBorrowingRequestsAsync();
            return Ok(requests);
        }
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetBRequestByIdAsync(Guid id)
        {
            var request = await _service.GetBorrowingRequestByIdAsync(id);
            return Ok(request);
        }
        [HttpGet("by-user/{id}")]
        [Authorize]
        public async Task<IActionResult> GetBorrowingRequestByUserIdAsync(string id)
        {
            var requests = await _service.GetBorrowingRequestByUserIdAsync(id);
            return Ok(requests);
        }
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteBRequestAsync(Guid id)
        {
            await _service.DeleteBorrowingRequestAsync(id);
            return NoContent();
        }
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateBRequestStatusAsync([FromBody] BorrowingRequestUpdateRequest request, Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _service.UpdateBorrowingRequestStatusAsync(id, request);
            return Ok("Update success!");
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddBRequestAsync(CreateBorrowingRequestDto request)
        {
            var requests = await _service.CreateBorrowingRequestAsync(request);
            return Ok(requests);
        }
        [HttpGet("withbooks")]
        public async Task<IActionResult> GetAllRequestsAsyncWithBooks()
        {
            var books = await _service.GetAllBorrowingRequestsAsync();
            return Ok(books);
        }
    }
}