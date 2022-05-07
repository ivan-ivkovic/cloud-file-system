using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc.Testing;

using NUnit.Framework;

using P3Mobility.CloudFileSystem.FileSystemApi.IntegrationTests.Helpers;

namespace P3Mobility.CloudFileSystem.FileSystemApi.IntegrationTests;

[TestFixture]
public class FileSystemApiTests
{
    private HttpClient httpClient;

    public FileSystemApiTests()
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
    public async Task GetFileById_FileExists_ReturnsCorrectFile()
    {
        // Arrange
        string fileId = "013b03b8-5787-40c4-889e-adc9e8c605d1";
        string expectedFile = $"{{\"id\":\"{fileId}\",\"name\":\"file1.txt\"}}";

        // Act
        HttpResponseMessage httpResponse = await this.httpClient
            .GetAsync($"/files/{fileId}").ConfigureAwait(false);
        string jsonBody = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);

        // Assert
        Assert.AreEqual(expectedFile, jsonBody);
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
        var responseModel = JsonSerializer.Deserialize<Http404ResponseModel>(
            jsonBody,
            JsonOptions.CamelCasePolicy
        );

        // Assert
        Assert.AreEqual(expectedStatusCode, responseModel?.Status);
        Assert.AreEqual(expectedErrorMessage, responseModel?.Title);
    }
}
