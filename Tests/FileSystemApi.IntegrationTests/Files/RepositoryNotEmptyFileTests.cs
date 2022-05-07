using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc.Testing;

using NUnit.Framework;

using P3Mobility.CloudFileSystem.FileSystem.Files;
using P3Mobility.CloudFileSystem.FileSystemApi.IntegrationTests.Helpers;

namespace P3Mobility.CloudFileSystem.FileSystemApi.IntegrationTests.Files;

[TestFixture]
public class RepositoryNotEmptyFileTests
{
    private HttpClient httpClient;

    public RepositoryNotEmptyFileTests()
    {
        var webApplicationFactory = new WebApplicationFactory<Program>();
        this.httpClient = webApplicationFactory.CreateDefaultClient();
    }

    [Test, Order(1)]
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

    [Test, Order(2)]
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
    public async Task CreateFile_FileAreadyExists_ReturnsBadRequest()
    {
        // Arrange
        string fileName = "same-file-name.txt";
        await this.CreateTestFile(fileName).ConfigureAwait(false);
        string expectedErrorMessage = "Bad Request";
        int expectedStatusCode = 400;

        // Act
        HttpContent content = new StringContent(
            $"{{\"name\": \"{fileName}\"}}",
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
        HttpContent content = new StringContent(
            $"{{\"name\": \"{name}\"}}",
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
