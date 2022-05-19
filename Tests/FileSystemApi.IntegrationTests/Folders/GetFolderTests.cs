using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc.Testing;

using NUnit.Framework;

using CloudFileSystem.FileSystemApi.IntegrationTests.Helpers;

namespace CloudFileSystem.FileSystemApi.IntegrationTests.Folders;

[TestFixture]
public class GetFolderTests
{
    private HttpClient httpClient;

    public GetFolderTests()
    {
        var webApplicationFactory = new WebApplicationFactory<Program>();
        this.httpClient = webApplicationFactory.CreateDefaultClient();
    }

    [Test]
    public async Task GetFolder_FolderDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        string folderId = Guid.NewGuid().ToString();
        string expectedErrorMessage = "Not Found";
        int expectedStatusCode = 404;

        // Act
        HttpResponseMessage httpResponse = await this.httpClient
            .GetAsync($"/folders/{folderId}").ConfigureAwait(false);
        string jsonBody = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
        var responseModel = JsonSerializer.Deserialize<HttpErrorResponseModel>(
            jsonBody,
            JsonOptions.CamelCasePolicy
        );

        // Assert
        Assert.AreEqual(expectedStatusCode, responseModel?.Status);
        Assert.AreEqual(expectedErrorMessage, responseModel?.Title);
    }
}
