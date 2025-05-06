using Library_API_2._0.Application.Dtos.Datetime;
using Library_API_2._0.Application.Dtos.Users.AdminRequest;
using Library_API_2._0.Application.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library_API_2._0.Api.Controller
{
    [Route("api/record")]
    [ApiController]
    public class RecordController : ControllerBase
    {
        private readonly IBorrowingRecordService _service;

        public RecordController(IBorrowingRecordService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllRecordsAsync()
        {
            var records = await _service.GetAllRecordsAsync();
            return Ok(records);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetRecordByIdAsync(Guid id)
        {
            var record = await _service.GetRecordByIdAsync(id);
            return Ok(record);
        }

        [HttpGet("by-request/{id}")]
        [Authorize]
        public async Task<IActionResult> GetRecordByBorrowingRequestIdAsync(Guid id)
        {
            var record = await _service.GetRecordByBorrowingRequestIdAsync(id);
            return Ok(record);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteRecordAsync(Guid id)
        {
            await _service.DeleteRecordAsync(id);
            return NoContent();
        }

        [HttpPut("pickup/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> MarkBookPickedUpAsync(Guid id, [FromBody] AdminRequestId adminId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var record = await _service.MarkBookPickedUpAsync(id, adminId);
            return Ok(record);
        }

        [HttpPut("return/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> MarkBookReturnedAsync(Guid id, [FromBody] AdminRequestId adminId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var record = await _service.MarkBookReturnedAsync(id, adminId);
            return Ok(record);
        }

        [HttpPost("updatestatus")]
        public async Task<IActionResult> UpdateRecordStatusbyDayAsync([FromBody] RequestDateTime currentTime)
        {
            await _service.UpdateRecordStatusbyDayAsync(currentTime);
            return Ok("Record statuses updated successfully!");
        }
    }

}