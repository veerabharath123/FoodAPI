using FoodAPI.IRepositories;
using FoodAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FoodAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentsController : ControllerBase
    {
        private IDocumentRepository _documentRepository;
        public DocumentsController(IDocumentRepository documentRepository)
        {
            _documentRepository = documentRepository;
        }
        [HttpPost("SaveImage")]
        public async Task<IActionResult> SaveImage(ImageDetails imgDetails)
        {
            var result = await _documentRepository.SaveImage(imgDetails);
            return Ok(new ApiResponse<Guid?>
            {
                Success = result != null,
                Result = result,
                Message = $"Saving image is {(result != null ? "" : "un")}successful"
            });
        }
    }
}
