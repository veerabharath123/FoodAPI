using FoodAPI.Database;
using FoodAPI.Helpers;
using FoodAPI.IRepositories;
using FoodAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodAPI.Repositories
{
    public class DocumentRepository: IDocumentRepository
    {
        private readonly FoodDbContext _context;
        public DocumentRepository(FoodDbContext context)
        {
            _context = context;
        }

        public async Task<Guid?> SaveImage(ImageDetails ImageDetails)
        {
            var dt = DateTime.Now;
            if (!string.IsNullOrEmpty(ImageDetails.Base64))
            {
                var img = new Image
                {
                    CREATED_DATE = dt.Date,
                    CREATED_TIME = TimeSpan.Parse(dt.ToString("HH:mm:ss")),
                    IMAGE_DATA = Convert.FromBase64String(ImageDetails.Base64.Replace("data:image/jpeg;base64,","")),
                    IMAGE_NAME = ImageDetails.Image_Name,
                    IMAGE_TYPE = ImageDetails.Image_Type,
                    TEMP_ID = Guid.NewGuid(),
                };
                await _context.Images.AddAsync(img);
                if (await _context.SaveChangesAsync() > 0)
                    return img.TEMP_ID;
            }
            return null;
        }
        public async Task<string> GetImage(Guid? temp_id)
        {
            var result = await _context.Images.AsNoTracking().FirstOrDefaultAsync(x => x.DELETE_FLAG != "Y" && x.TEMP_ID == temp_id);
            if (result != null && result.IMAGE_DATA != null)
            {
                return Convert.ToBase64String(result.IMAGE_DATA);                
            }
            return string.Empty;
        }
    }
}
