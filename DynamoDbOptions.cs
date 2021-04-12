namespace PocoDynamoDemo
{
    public class DynamoDbOptions
    {
        public const string SectionName = "DynamoDb";

        public string AccessKey { get; set; }

        public string SecretKey { get; set; }

        public string ServiceUrl { get; set; }
    }
}