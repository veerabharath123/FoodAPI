namespace FoodAPI.Dtos.RequestDto
{
    public class ImageRequest
    {
        public string? Base64 { get; set; }
        public Guid? Temp_Id { get; set; }
        public string? Image_Name { get; set; }
        public string? Image_Type { get; set; }

    }
}
