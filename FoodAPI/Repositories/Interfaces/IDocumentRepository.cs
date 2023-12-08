using FoodAPI.Models;

namespace FoodAPI.IRepositories
{
    public interface IDocumentRepository
    {
        Task<Guid?> SaveImage(ImageDetails ImageDetails);
        Task<string> GetImage(Guid? temp_id);
    }
}
