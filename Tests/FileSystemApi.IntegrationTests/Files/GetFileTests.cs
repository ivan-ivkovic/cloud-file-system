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
    private readonly HttpClient httpClient;
    private readonly Guid RootLevelFolderId;

    public GetFileTests()
    {
        var webApplicationFactory = new WebApplicationFactory<Program>();
        this.httpClient = webApplicationFactory.CreateDefaultClient();
        this.RootLevelFolderId = Guid.Empty;
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
        var createOrUpdateFileModel1 = new CreateOrUpdateFileModel
        {
            Name = "file1.txt",
            FolderId = this.RootLevelFolderId
        };
        var createOrUpdateFileModel2 = new CreateOrUpdateFileModel
        {
            Name = "file2.txt",
            FolderId = this.RootLevelFolderId
        };
        FileResponseModel file1 = await this.CreateTestFile(createOrUpdateFileModel1).ConfigureAwait(false);
        FileResponseModel file2 = await this.CreateTestFile(createOrUpdateFileModel2).ConfigureAwait(false);

        // Act
        HttpResponseMessage httpResponse = await this.httpClient
            .GetAsync("/files").ConfigureAwait(false);
        List<FileResponseModel>? actualFiles = await httpResponse.Content
            .ReadFromJsonAsync<List<FileResponseModel>>().ConfigureAwait(false);

        // Assert
        Assert.NotNull(actualFiles);
        Assert.AreEqual(2, actualFiles?.Count);
        Assert.IsTrue(actualFiles?.Any(x => x.Id == file1.Id));
        Assert.IsTrue(actualFiles?.Any(x => x.Id == file2.Id));
        Assert.IsTrue(actualFiles?.All(x => x.AncestorFolderIds.Count() == 0));
    }

    [Test]
    public async Task GetFileById_FileExists_ReturnsCorrectFile()
    {
        // Arrange
        var createOrUpdateFileModel = new CreateOrUpdateFileModel
        {
            Name = "file3.txt",
            FolderId = this.RootLevelFolderId
        };
        FileResponseModel expectedFile = await this.CreateTestFile(createOrUpdateFileModel).ConfigureAwait(false);

        // Act
        HttpResponseMessage httpResponse = await this.httpClient
            .GetAsync($"/files/{expectedFile?.Id}").ConfigureAwait(false);
        FileResponseModel? actualFile = await httpResponse.Content
            .ReadFromJsonAsync<FileResponseModel>().ConfigureAwait(false);

        // Assert
        Assert.NotNull(actualFile);
        Assert.AreEqual(expectedFile?.Id, actualFile?.Id);
        Assert.AreEqual(expectedFile?.Name, actualFile?.Name);
        Assert.AreEqual(0, actualFile?.AncestorFolderIds.Count());
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

    private async Task<FileResponseModel> CreateTestFile(CreateOrUpdateFileModel file)
    {
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
        FileResponseModel? createdFile = await httpResponse.Content
            .ReadFromJsonAsync<FileResponseModel>().ConfigureAwait(false);

        if (createdFile == null)
        {
            throw new Exception("Test failed. Could not create test data.");
        }

        return createdFile;
    }
}
