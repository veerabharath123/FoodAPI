using FoodAPI.Dtos.RequestDto;
using FoodAPI.Models;

namespace FoodAPI.Services.Interfaces
{
    public interface IDocumentServices
    {
        Task<ApiResponse<Guid?>> SaveImage(ImageRequest imgDetails);
    }
}
