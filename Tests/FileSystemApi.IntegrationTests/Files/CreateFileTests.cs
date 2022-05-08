using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc.Testing;

using NUnit.Framework;

using P3Mobility.CloudFileSystem.FileSystem.Files;
using P3Mobility.CloudFileSystem.FileSystemApi.IntegrationTests.Helpers;

namespace P3Mobility.CloudFileSystem.FileSystemApi.IntegrationTests.Files;

[TestFixture]
public class CreateFileTests
{
    private HttpClient httpClient;

    public CreateFileTests()
    {
        var webApplicationFactory = new WebApplicationFactory<Program>();
        this.httpClient = webApplicationFactory.CreateDefaultClient();
    }

    [Test]
    public async Task CreateFile_FileAreadyExists_ReturnsBadRequest()
    {
        // Arrange
        string fileName = "same-file-name.txt";
        var file = new CreateOrUpdateFileModel
        {
            Name = fileName,
            FolderId = Guid.Empty
        };
        await this.CreateTestFile(fileName).ConfigureAwait(false);
        string expectedErrorMessage = "Bad Request";
        int expectedStatusCode = 400;

        // Act
        HttpContent content = new StringContent(
            JsonSerializer.Serialize<CreateOrUpdateFileModel>(file, JsonOptions.CamelCasePolicy),
            Encoding.UTF8,
            "application/json"
        );
        HttpResponseMessage httpResponse = await this.httpClient
            .PostAsync("/files", content).ConfigureAwait(false);
        HttpErrorResponseModel? responseModel = await httpResponse.Content
            .ReadFromJsonAsync<HttpErrorResponseModel>().ConfigureAwait(false);

        // Assert
        Assert.AreEqual(expectedStatusCode, responseModel?.Status);
        Assert.AreEqual(expectedErrorMessage, responseModel?.Title);
    }

    private async Task<FileModel> CreateTestFile(string name)
    {
        var file = new CreateOrUpdateFileModel
        {
            Name = name,
            FolderId = Guid.Empty
        };
        HttpContent content = new StringContent(
            JsonSerializer.Serialize<CreateOrUpdateFileModel>(
                file,
                JsonOptions.CamelCasePolicy
            ),
            Encoding.UTF8,
            "application/json"
        );

        HttpResponseMessage httpResponse = await this.httpClient
            .PostAsync("/files", content).ConfigureAwait(false);
        FileModel? createdFile = await httpResponse.Content
            .ReadFromJsonAsync<FileModel>().ConfigureAwait(false);

        if (createdFile == null)
        {
            throw new Exception("Test failed. Could not create test data.");
        }

        return createdFile;
    }
}