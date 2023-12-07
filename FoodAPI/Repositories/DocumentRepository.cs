using FoodAPI.Database;
using FoodAPI.Helpers;
using FoodAPI.Models;

namespace FoodAPI.Repositories
{
    public class DocumentRepository
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
                    IMAGE_DATA = Convert.FromBase64String(ImageDetails.Base64!),
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
    }
}
