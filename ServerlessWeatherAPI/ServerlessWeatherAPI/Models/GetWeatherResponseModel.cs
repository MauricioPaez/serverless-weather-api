namespace ServerlessWeatherAPI.Models
{
    public class GetWeatherResponseModel
    {
        public string? CityName { get; set; }
        public string? Condition { get; set; }
        public string? Description { get; set; }
        public double? Temperature { get; set; }
        public double? Humidity { get; set; }
    }
}
