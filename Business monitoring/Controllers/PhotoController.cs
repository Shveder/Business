﻿
using Microsoft.AspNetCore.Mvc;

using Business_monitoring.Services.Interfaces;

namespace Business_monitoring.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PhotoController : ControllerBase
    {
        private readonly ILogger<PhotoController> _logger;
        private readonly IPhotoService _photoService;

        public PhotoController(ILogger<PhotoController> logger, IPhotoService photoService)
        {
            _logger = logger;
            _photoService = photoService;
        }

        [HttpPost("HandleFileUpload/{userId}")]
        public async Task<IActionResult> HandleFileUpload([FromRoute] Guid userId, [FromForm] IFormFile file, [FromQuery] int role)
        {
            await _photoService.SavePhotoAsync(userId, file.OpenReadStream(), role);
            return Ok("Файл успешно загружен!");
        }

        [HttpGet("GetPhoto/{userId}")]
        public async Task<IActionResult> GetPhoto([FromRoute] Guid userId, [FromQuery] int role)
        {
            var photoStream = await _photoService.GetPhotoAsync(userId, role);
                return File(photoStream, "image/jpeg");
        }
    }
}