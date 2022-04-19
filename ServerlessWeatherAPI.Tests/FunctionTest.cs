using Xunit;
using Amazon.Lambda.Core;
using Amazon.Lambda.TestUtilities;
using Amazon.Lambda.APIGatewayEvents;
using System.Text.Json;
using ServerlessWeatherAPI.Models;

namespace ServerlessWeatherAPI.Tests;

public class FunctionTest
{
    private static readonly HttpClient client = new();
    private const string ApiBaseUrl = "https://4q7sjxmzc5.execute-api.us-east-1.amazonaws.com/Prod";

    public FunctionTest()
    {
    }

    [Fact]
    public void TestHelloFromLambdaMethod()
    {
        var response = client.GetAsync($"{ApiBaseUrl}/hello").Result;
        var content = response.Content.ReadAsStringAsync().Result;
        Assert.Equal(200, (int)response.StatusCode);
        Assert.Equal("Hello from an AWS serverless API", content);
    }

    [Fact]
    public void TestGetWeather()
    {
        var response = client.GetAsync($"{ApiBaseUrl}/weather?city=envigado&countryCode=co").Result;
        var content = response.Content.ReadAsStringAsync().Result;
        var responseModel = JsonSerializer.Deserialize<GetWeatherResponseModel>(content);

        Assert.Equal(200, (int)response.StatusCode);
        Assert.Equal("envigado", responseModel.CityName.ToLower());
    }

    [Fact]
    public void TestGetWeather_NoParameters()
    {
        var response = client.GetAsync($"{ApiBaseUrl}/weather").Result;
        var content = response.Content.ReadAsStringAsync().Result;

        Assert.Equal(400, (int)response.StatusCode);
        Assert.Equal("Request should contain the query parameter [city]", content);
    }

    [Fact]
    public void TestGetWeather_NoCity()
    {
        var response = client.GetAsync($"{ApiBaseUrl}/weather?countryCode=co").Result;
        var content = response.Content.ReadAsStringAsync().Result;

        Assert.Equal(400, (int)response.StatusCode);
        Assert.Equal("Request should contain the query parameter [city]", content);
    }

    [Fact]
    public void TestGetWeather_NoCountryCode()
    {
        var response = client.GetAsync($"{ApiBaseUrl}/weather?city=envigado").Result;
        var content = response.Content.ReadAsStringAsync().Result;

        Assert.Equal(400, (int)response.StatusCode);
        Assert.Equal("Request should contain the query parameter [countryCode]", content);
    }

    [Fact]
    public void TestGetWeather_FakeCity()
    {
        var response = client.GetAsync($"{ApiBaseUrl}/weather?city=loremipsum&countryCode=co").Result;
        var content = response.Content.ReadAsStringAsync().Result;
        
        Assert.Equal(204, (int)response.StatusCode);
        Assert.Equal("", content);
    }

    [Fact]
    public void TestGetWeather_FakeCountryCode()
    {
        var response = client.GetAsync($"{ApiBaseUrl}/weather?city=envigado&countryCode=ju").Result;
        var content = response.Content.ReadAsStringAsync().Result;

        Assert.Equal(204, (int)response.StatusCode);
        Assert.Equal("", content);
    }
}