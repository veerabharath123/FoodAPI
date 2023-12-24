using FoodAPI.Dtos.RequestDto;

namespace FoodAPI.IRepositories
{
    public interface IDocumentRepository
    {
        Task<Guid?> SaveImage(ImageRequest ImageDetails);
        Task<string> GetImage(Guid? temp_id);
    }
}
