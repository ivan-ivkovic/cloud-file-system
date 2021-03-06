using System.Text.Json;

namespace CloudFileSystem.FileSystemApi.IntegrationTests.Helpers;

public static class JsonOptions
{
    private static JsonSerializerOptions? camelCasePolicy;

    public static JsonSerializerOptions CamelCasePolicy
    {
        get
        {
            if (camelCasePolicy == null)
            {
                camelCasePolicy = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };
            }

            return camelCasePolicy;
        }
    }
}