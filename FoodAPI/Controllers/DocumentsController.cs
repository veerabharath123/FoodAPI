using FoodAPI.Dtos.RequestDto;
using FoodAPI.IRepositories;
using FoodAPI.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FoodAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentsController : ControllerBase
    {
        private IDocumentServices _documentServices;
        public DocumentsController(IDocumentServices documentServices)
        {
            _documentServices = documentServices;
        }
        [HttpPost("SaveImage")]
        public async Task<IActionResult> SaveImage(ImageRequest request)
        {
            var result = await _documentServices.SaveImage(request);
            return Ok(result);
        }
    }
}
