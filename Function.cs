using System;
using System.Threading.Tasks;
using Amazon.Lambda;
using Amazon.Lambda.Model;
using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using Amazon.XRay.Recorder.Handlers.AwsSdk;
using System.IO;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]
[assembly: CLSCompliant(true)]
namespace aws_lambda_style_demo
{
    public class MyFunction
    {
        private AmazonLambdaClient lambdaClient;

        public MyFunction()
        {
            initialize();
        }

        private async void initialize()
        {
            AWSSDKHandler.RegisterXRayForAllServices();
            lambdaClient = new AmazonLambdaClient();
            await callLambda();
        }

        public async Task<AccountUsage> FunctionHandler(SQSEvent invocationEvent, ILambdaContext context)
        {
            GetAccountSettingsResponse accountSettings;
            try
            {
                accountSettings = await callLambda();
            }
            catch (AmazonLambdaException)
            {
                throw;
            }

            AccountUsage accountUsage = accountSettings.AccountUsage;
            MemoryStream logData = new();
            StreamReader logDataReader = new StreamReader(logData);

            Amazon.Lambda.Serialization.Json.JsonSerializer serializer = new Amazon.Lambda.Serialization.Json.JsonSerializer();

            serializer.Serialize<System.Collections.IDictionary>(System.Environment.GetEnvironmentVariables(), logData);
            LambdaLogger.Log("ENVIRONMENT VARIABLES: " + logDataReader.ReadLine());
            logData.Position = 0;
            serializer.Serialize<ILambdaContext>(context, logData);
            LambdaLogger.Log("CONTEXT: " + logDataReader.ReadLine());
            logData.Position = 0;
            serializer.Serialize<SQSEvent>(invocationEvent, logData);
            LambdaLogger.Log("EVENT: " + logDataReader.ReadLine());

            return accountUsage;
        }

        public async Task<GetAccountSettingsResponse> callLambda()
        {
            //var a = 10;
            GetAccountSettingsRequest request = new GetAccountSettingsRequest();
            GetAccountSettingsResponse response = await lambdaClient.GetAccountSettingsAsync(request);
            return response;
        }


    }

}
