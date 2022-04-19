# AWS Serverless API

This simple API consists of:
* serverless.template - an AWS CloudFormation Serverless Application Model template file which declares the Serverless functions
* Function.cs - class file containing the C# methods mapped to the functions declared in the template file

## Endpoints:
* BaseUrl - https://4q7sjxmzc5.execute-api.us-east-1.amazonaws.com/Prod
* Hello - [BaseUrl](https://4q7sjxmzc5.execute-api.us-east-1.amazonaws.com/Prod)/hello
* GetWeather - [BaseUrl](https://4q7sjxmzc5.execute-api.us-east-1.amazonaws.com/Prod)/weather. You must pass the following query parameters:
    - city: name of the city from which you would like to know the weather
    - countryCode: city's ISO 3166 country code. Please refer to this [link](https://en.wikipedia.org/wiki/List_of_ISO_3166_country_codes)

[Example](https://4q7sjxmzc5.execute-api.us-east-1.amazonaws.com/Prod/weather?city=%22envigado%22&countryCode=%22co%22)

