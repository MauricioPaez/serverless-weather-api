using Amazon;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using System.Text.Json;

namespace ServerlessWeatherAPI
{
    public class SecretsManagerHelper
    {
        public static string GetSecret()
        {
            string region = "us-east-1";
            string secretName = "prod/weather-api/api-key";

            var response = new AmazonSecretsManagerClient(
                RegionEndpoint.GetBySystemName(region)).GetSecretValueAsync(
                new GetSecretValueRequest
                {
                    SecretId = secretName
                }).Result;

            if (response.SecretString is not null)
            { 
                var secret = JsonSerializer.Deserialize<OpenWeatherMapSecret>(response.SecretString);
                return secret.OpenWeatherMapAPIKey;
            }
            else
            {
                StreamReader reader = new (response.SecretBinary);
                return System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(reader.ReadToEnd()));
            }
        }
    }
}

public class OpenWeatherMapSecret
{
    public string? OpenWeatherMapAPIKey { get; set; } 
}
