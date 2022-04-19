using Xunit;
using Amazon.Lambda.Core;
using Amazon.Lambda.TestUtilities;
using Amazon.Lambda.APIGatewayEvents;
using System.Text.Json;
using ServerlessWeatherAPI.Models;

namespace ServerlessWeatherAPI.Tests;

public class FunctionTest
{
    public FunctionTest()
    {
    }

    [Fact]
    public void TestHelloFromLambdaMethod()
    {
        TestLambdaContext context;
        APIGatewayProxyRequest request;
        APIGatewayProxyResponse response;

        request = new APIGatewayProxyRequest();
        context = new TestLambdaContext();
        response = Functions.HelloFromLambda(request, context);
        Assert.Equal(200, response.StatusCode);
        Assert.Equal("Hello from an AWS serverless API", response.Body);
    }

    [Fact]
    public void TestGetWeather()
    {
        TestLambdaContext context;
        APIGatewayProxyRequest request;
        APIGatewayProxyResponse response;

        request = new APIGatewayProxyRequest();
        var parameters = new Dictionary<string, string>
        {
            { "city", "envigado" },
            { "countryCode", "co" }
        };
        request.QueryStringParameters = parameters;

        context = new TestLambdaContext();
        response = Functions.GetWeather(request, context);
        var responseModel = JsonSerializer.Deserialize<GetWeatherResponseModel>(response.Body);
        Assert.Equal(200, response.StatusCode);
        Assert.Equal("envigado", responseModel.CityName.ToLower());
    }

    [Fact]
    public void TestGetWeather_NoParameters()
    {
        TestLambdaContext context;
        APIGatewayProxyRequest request;
        APIGatewayProxyResponse response;

        request = new APIGatewayProxyRequest();

        context = new TestLambdaContext();
        response = Functions.GetWeather(request, context);
        Assert.Equal(400, response.StatusCode);
        Assert.Equal("Request should contain the query parameter [city]", response.Body);
    }

    [Fact]
    public void TestGetWeather_NoCity()
    {
        TestLambdaContext context;
        APIGatewayProxyRequest request;
        APIGatewayProxyResponse response;

        request = new APIGatewayProxyRequest();
        var parameters = new Dictionary<string, string>
        {
            { "countryCode", "co" }
        };
        request.QueryStringParameters = parameters;

        context = new TestLambdaContext();
        response = Functions.GetWeather(request, context);
        Assert.Equal(400, response.StatusCode);
        Assert.Equal("Request should contain the query parameter [city]", response.Body);
    }

    [Fact]
    public void TestGetWeather_NoCountryCode()
    {
        TestLambdaContext context;
        APIGatewayProxyRequest request;
        APIGatewayProxyResponse response;

        request = new APIGatewayProxyRequest();
        var parameters = new Dictionary<string, string>
        {
            { "city", "envigado" }
        };
        request.QueryStringParameters = parameters;

        context = new TestLambdaContext();
        response = Functions.GetWeather(request, context);
        Assert.Equal(400, response.StatusCode);
        Assert.Equal("Request should contain the query parameter [countryCode]", response.Body);
    }

    [Fact]
    public void TestGetWeather_FakeCity()
    {
        TestLambdaContext context;
        APIGatewayProxyRequest request;
        APIGatewayProxyResponse response;

        request = new APIGatewayProxyRequest();
        var parameters = new Dictionary<string, string>
        {
            { "city", "floating castle" },
            { "countryCode", "co" }
        };
        request.QueryStringParameters = parameters;

        context = new TestLambdaContext();
        response = Functions.GetWeather(request, context);
        Assert.Equal(204, response.StatusCode);
        Assert.Equal("No city was found with the parameters provided", response.Body);
    }

    [Fact]
    public void TestGetWeather_FakeCountryCode()
    {
        TestLambdaContext context;
        APIGatewayProxyRequest request;
        APIGatewayProxyResponse response;

        request = new APIGatewayProxyRequest();
        var parameters = new Dictionary<string, string>
        {
            { "city", "envigado" },
            { "countryCode", "ju" }
        };
        request.QueryStringParameters = parameters;

        context = new TestLambdaContext();
        response = Functions.GetWeather(request, context);
        Assert.Equal(204, response.StatusCode);
        Assert.Equal("No city was found with the parameters provided", response.Body);
    }
}