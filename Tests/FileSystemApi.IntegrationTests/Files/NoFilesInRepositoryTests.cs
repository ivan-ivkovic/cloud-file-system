using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;
using P3Mobility.CloudFileSystem.FileSystemApi.IntegrationTests.Helpers;

namespace P3Mobility.CloudFileSystem.FileSystemApi.IntegrationTests.Files;

[TestFixture]
public class NoFilesInRepositoryTests
{
    private HttpClient httpClient;

    public NoFilesInRepositoryTests()
    {
        var webApplicationFactory = new WebApplicationFactory<Program>();
        this.httpClient = webApplicationFactory.CreateDefaultClient();
    }

    [Test]
    public async Task GetFiles_FilesDoNotExist_ReturnsEmptyJson()
    {
        // Arrange
        string expectedFiles = "[]";

        // Act
        HttpResponseMessage httpResponse = await this.httpClient
            .GetAsync("/files").ConfigureAwait(false);
        string actualFiles = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);

        // Assert
        Assert.AreEqual(expectedFiles, actualFiles);
    }

    [Test]
    public async Task GetFileById_FileDoesNotExist_Returns404()
    {
        // Arrange
        string fileId = Guid.NewGuid().ToString();
        string expectedErrorMessage = "Not Found";
        int expectedStatusCode = 404;

        // Act
        HttpResponseMessage httpResponse = await this.httpClient
            .GetAsync($"/files/{fileId}").ConfigureAwait(false);
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