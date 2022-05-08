using System;
using System.Collections.Generic;
using System.Linq;
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
public class GetFileTests
{
    private HttpClient httpClient;

    public GetFileTests()
    {
        var webApplicationFactory = new WebApplicationFactory<Program>();
        this.httpClient = webApplicationFactory.CreateDefaultClient();
    }

    [Test, Order(1)]
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

    [Test, Order(2)]
    public async Task GetFiles_FilesExist_ReturnsFiles()
    {
        // Arrange
        FileModel file1 = await this.CreateTestFile("file1.txt").ConfigureAwait(false);
        FileModel file2 = await this.CreateTestFile("file2.txt").ConfigureAwait(false);

        // Act
        HttpResponseMessage httpResponse = await this.httpClient
            .GetAsync("/files").ConfigureAwait(false);
        List<FileModel>? actualFiles = await httpResponse.Content
            .ReadFromJsonAsync<List<FileModel>>().ConfigureAwait(false);

        // Assert
        Assert.NotNull(actualFiles);
        Assert.AreEqual(2, actualFiles?.Count);
        Assert.IsTrue(actualFiles?.Any(x => x.Id == file1.Id));
        Assert.IsTrue(actualFiles?.Any(x => x.Id == file2.Id));
    }

    [Test]
    public async Task GetFileById_FileExists_ReturnsCorrectFile()
    {
        // Arrange
        FileModel expectedFile = await this.CreateTestFile("file3.txt").ConfigureAwait(false);

        // Act
        HttpResponseMessage httpResponse = await this.httpClient
            .GetAsync($"/files/{expectedFile?.Id}").ConfigureAwait(false);
        FileModel? actualFile = await httpResponse.Content
            .ReadFromJsonAsync<FileModel>().ConfigureAwait(false);

        // Assert
        Assert.NotNull(actualFile);
        Assert.AreEqual(expectedFile?.Id, actualFile?.Id);
        Assert.AreEqual(expectedFile?.Name, actualFile?.Name);
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
