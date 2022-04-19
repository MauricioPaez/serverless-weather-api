using System.Net;
using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using System.Text.Json;
using ServerlessWeatherAPI.Models;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace ServerlessWeatherAPI;

public class Functions

{
    private static readonly HttpClient client = new();
    private const string ApiKey = "d7fdd921901adca09c3ed0b0a9f7e70a";
    private const string ApiBaseUrl = "http://api.openweathermap.org";
    private const string GeoUrl = "/geo/1.0/direct";
    private const string WeatherUrl = "/data/2.5/weather";

    /// <summary>
    /// Default constructor that Lambda will invoke.
    /// </summary>
    public Functions()
    {
    }

    /// <summary>
    /// A Lambda function to respond to HTTP Get methods from API Gateway
    /// </summary>
    /// <param name="request"></param>
    /// <returns>The API Gateway response.</returns>
    public static APIGatewayProxyResponse HelloFromLambda(APIGatewayProxyRequest request, ILambdaContext context)
    {
        var response = new APIGatewayProxyResponse
        {
            StatusCode = (int)HttpStatusCode.OK,
            Body = "Hello from an AWS serverless API",
            Headers = new Dictionary<string, string> { { "Content-Type", "text/plain" } }
        };

        return response;
    }

    /// <summary>
    /// A Lambda function to respond to HTTP Get methods from API Gateway
    /// </summary>
    /// <param name="request"></param>
    /// <returns>The API Gateway response.</returns>
    public static APIGatewayProxyResponse GetWeather(APIGatewayProxyRequest request, ILambdaContext context)
    {
        try
        {
            if (client.BaseAddress is null)
            {
                client.BaseAddress = new Uri($"{ApiBaseUrl}");
            }

            var city = GetQueryParameter(request, "city");
            var countryCode = GetQueryParameter(request, "countryCode");

            var citiesResponse = client.GetStringAsync($"{client.BaseAddress}{GeoUrl}?q={city}, {countryCode}&limit=1&appid={ApiKey}").Result;

            var cityObjects = JsonSerializer.Deserialize<City[]>(citiesResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (cityObjects is not null && cityObjects.Length > 0)
            {
                var cityObject = cityObjects[0];

                var weatherResponse = client.GetStringAsync($"{client.BaseAddress}{WeatherUrl}?lat={cityObject.Lat}&lon={cityObject.Lon}&appid={ApiKey}&units=metric").Result;

                var weatherRoot = JsonSerializer.Deserialize<WeatherRoot>(weatherResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if(weatherRoot is not null && weatherRoot.Weather is not null && weatherRoot.Main is not null)
                {
                    var response = new GetWeatherResponseModel
                    {
                        CityName = weatherRoot.Name,
                        Condition = weatherRoot.Weather[0].Main,
                        Description = weatherRoot.Weather[0].Description,
                        Temperature = weatherRoot.Main.Temp,
                        Humidity = weatherRoot.Main.Humidity
                    };

                    return GetResponse(HttpStatusCode.OK, JsonSerializer.Serialize(response));
                }else
                {
                    return GetResponse(HttpStatusCode.NoContent, "No weather data found for the specified city");
                }                
            }
            else
            {
                return GetResponse(HttpStatusCode.NoContent, "No city was found with the parameters provided");
            }
        }
        catch (RequestParameterException ex)
        {
            return GetResponse(HttpStatusCode.BadRequest, ex.Message);
        }
        catch (Exception ex)
        {
            return GetResponse(HttpStatusCode.BadGateway, ex.Message);
        }
    }

    public static string? GetQueryParameter(APIGatewayProxyRequest request, string parameter)
    {
        if (request.QueryStringParameters is null || !request.QueryStringParameters.ContainsKey(parameter) || !request.QueryStringParameters.TryGetValue(parameter, out string? value))
        {
            throw new RequestParameterException($"Request should contain the query parameter [{parameter}]");
        }

        if (string.IsNullOrEmpty(value))
        {
            throw new RequestParameterException($"Query parameter [{parameter}] cannot be null");
        }

        return value;
    }

    public static APIGatewayProxyResponse GetResponse(HttpStatusCode statusCode, string body)
    {
        var response = new APIGatewayProxyResponse
        {
            StatusCode = (int)statusCode,
            Body = body,
            Headers = new Dictionary<string, string> { { "Content-Type", "text/plain" } }
        };

        return response;
    }
}

public class RequestParameterException : Exception
{
    public string? RequestParameter { get; set; }
    public string? RequestValue { get; set; }

    public string? CustomMessage { get; set; }

    public override string Message => string.IsNullOrEmpty(CustomMessage)
                                    ? "Error reading the parameter ["
                                    + RequestParameter
                                    + (string.IsNullOrEmpty(RequestValue) ? "" : ": " + RequestValue)
                                    + "]"
                                    : CustomMessage;

    public RequestParameterException(string requestParameter, string requestValue = "")
    {
        RequestParameter = requestParameter;
        RequestValue = requestValue;
    }

    public RequestParameterException(string customMessage)
    {
        CustomMessage = customMessage;
    }
}