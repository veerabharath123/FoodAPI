namespace FoodAPI.Models
{
    public class Image:BaseClass<decimal>
    {
        public string? IMAGE_NAME { get; set; }
        public byte[]? IMAGE_DATA { get; set; }
        public string? IMAGE_TYPE { get; set; }
        public string? MODULE_TYPE { get; set; }
        public decimal? MODULE_ID { get; set; }
        public Guid? TEMP_ID { get; set; }
    }
}
