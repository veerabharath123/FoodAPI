using FoodAPI.Dtos.RequestDto;
using FoodAPI.IRepositories;
using FoodAPI.Models;
using FoodAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FoodAPI.Services.Classes
{
    public class DocumentServices: IDocumentServices
    {
        private IDocumentRepository _documentRepository;
        public DocumentServices(IDocumentRepository documentRepository)
        {
            _documentRepository = documentRepository;
        }
        public async Task<ApiResponse<Guid?>> SaveImage(ImageRequest request)
        {
            var result = await _documentRepository.SaveImage(request);
            return new ApiResponse<Guid?>
            {
                Success = result != null,
                Result = result,
                Message = $"Saving image is {(result != null ? "" : "un")}successful"
            };
        }
    }
}
