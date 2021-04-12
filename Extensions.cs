using Amazon;
using Amazon.DynamoDBv2;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PocoDynamoDemo.Models;
using ServiceStack;
using ServiceStack.Aws.DynamoDb;
using ServiceStack.DataAnnotations;

namespace PocoDynamoDemo
{
    public static class Extensions
    {
        public static IServiceCollection AddDynamoDb(this IServiceCollection services)
        {
            DynamoDbOptions options;
            using (var serviceProvider = services.BuildServiceProvider())
            {
                var configuration = serviceProvider.GetService<IConfiguration>();
                services.Configure<DynamoDbOptions>(configuration.GetSection("DynamoDb"));
                options = configuration.GetOptions<DynamoDbOptions>("DynamoDb");
            }

            services.AddSingleton<IAmazonDynamoDB>(x => CreateAmazonDynamoDb(options.ServiceUrl));
            services.AddSingleton<IPocoDynamo, PocoDynamo>();

            return services;
        }
        
        public static void ConfigureDynamoDb(this IApplicationBuilder builder)
        {
            var serviceProvider = builder.ApplicationServices;
            var db = serviceProvider.GetService<IPocoDynamo>();
            var settingsAccessor = serviceProvider.GetService<Microsoft.Extensions.Options.IOptions<DynamoDbOptions>>();

            InitSchema(db, settingsAccessor.Value);
        }
        
        public static TModel GetOptions<TModel>(this IConfiguration configuration, string section) where TModel : new()
        {
            var model = new TModel();
            configuration.GetSection(section).Bind(model);

            return model;
        }
        
        public static IAmazonDynamoDB CreateAmazonDynamoDb(string serviceUrl)
        {
            var clientConfig = new AmazonDynamoDBConfig {RegionEndpoint = RegionEndpoint.EUWest1};

            if (!string.IsNullOrEmpty(serviceUrl))
            {
                clientConfig.ServiceURL = serviceUrl;
            }

            var dynamoClient = new AmazonDynamoDBClient(clientConfig);

            return dynamoClient;
        }
        
        public static void InitSchema(IPocoDynamo db, DynamoDbOptions options)
        {
            var employeeType = typeof(Employee);
            employeeType.AddAttributes(new AliasAttribute("employee"));

            var customerType = typeof(Customer);
            customerType.AddAttributes(new AliasAttribute("customer-orders"));
            
            var orderType = typeof(Order);
            orderType.AddAttributes(new AliasAttribute("customer-orders"));

            db.RegisterTable(employeeType);
            db.RegisterTable(customerType);
            db.RegisterTable(orderType);

            db.InitSchema();
        }
    }
}