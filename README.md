# Serverless Weather API
This API is published and ready to be used.

## Endpoints:
BaseUrl - https://4q7sjxmzc5.execute-api.us-east-1.amazonaws.com/Prod
* Hello - [BaseUrl/hello](https://4q7sjxmzc5.execute-api.us-east-1.amazonaws.com/Prod)
* GetWeather - [BaseUrl/weather](https://4q7sjxmzc5.execute-api.us-east-1.amazonaws.com/Prod). You must pass the following query parameters:
    - city: name of the city from which you would like to know the weather
    - countryCode: city's ISO 3166 country code. Please refer to this [link](https://en.wikipedia.org/wiki/List_of_ISO_3166_country_codes)

Example:
[BaseUrl/weather?city="envigado"&countryCode="co"](https://4q7sjxmzc5.execute-api.us-east-1.amazonaws.com/Prod/weather?city=%22envigado%22&countryCode=%22co%22)

### This simple API was created using:
* .NET 6 - C#
* AWS API Gateway
* AWS Lambda

### Relevant Files:
* Function.cs - class file containing the C# methods mapped to the functions declared in the template file
* FunctionTest.cs - class file containing the C# test methods written in XUnit
* serverless.template - an AWS CloudFormation Serverless Application Model template file which declares the Serverless functions and is used to publish the resources

### Run the project:
To run locally the project, follow these steps:
* Install .NET 6 ([Install](https://dotnet.microsoft.com/download/dotnet/scripts/v1/dotnet-install.ps1))
* Download the repo
* Cd into the project directory
```
cd ServerlessWeatherAPI
```
* Install Amazon.Lambda.Tools Global Tools
```
dotnet tool install -g Amazon.Lambda.Tools
```
* Run the dotnet Amazon Lambda test tool
```
 dotnet-lambda-test-tool-6.0
 ```
 * From the UI interface opened in your browser, select the endPoint from the Function dropdown
 * To test the HelloFromLambda endPoint just click on Execute Function
 * To test the GetWeather endPoint, in the Example Requests dropdown, select the saved TestRequest and click on Execute Function
 * To run the unit tests, cd back and run the dotnet test command
 ```
 cd ..
 dotnet test
 ```




