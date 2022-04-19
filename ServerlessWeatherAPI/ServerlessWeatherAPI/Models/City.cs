namespace ServerlessWeatherAPI.Models
{
    public class City
    {
        public string? Name { get; set; }
        public decimal? Lat { get; set; }
        public decimal? Lon { get; set; }
        public string? Country { get; set; }
        public string? State { get; set; }
    }
}
