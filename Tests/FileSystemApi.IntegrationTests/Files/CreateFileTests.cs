using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc.Testing;

using NUnit.Framework;

using P3Mobility.CloudFileSystem.FileSystem.Files;
using P3Mobility.CloudFileSystem.FileSystem.Folders.Models;
using P3Mobility.CloudFileSystem.FileSystemApi.IntegrationTests.Helpers;

namespace P3Mobility.CloudFileSystem.FileSystemApi.IntegrationTests.Files;

[TestFixture]
public class CreateFileTests
{
    private readonly HttpClient httpClient;
    private readonly Guid RootLevelFolderId;

    public CreateFileTests()
    {
        var webApplicationFactory = new WebApplicationFactory<Program>();
        this.httpClient = webApplicationFactory.CreateDefaultClient();
        this.RootLevelFolderId = Guid.Empty;
    }

    [Test]
    public async Task CreateFile_FileAreadyExists_ReturnsBadRequest()
    {
        // Arrange
        string fileName = "same-file-name.txt";
        var file = new CreateOrUpdateFileModel
        {
            Name = fileName,
            FolderId = this.RootLevelFolderId
        };
        await this.CreateTestFile(file).ConfigureAwait(false);
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

    [Test]
    public async Task CreateFileInsideAFolder_CreationSuccessful_ReturnsOk()
    {
        // Arrange
        var folder = await this.CreateRootLevelTestFolder().ConfigureAwait(false);
        if (folder == null)
        {
            Assert.Fail();
            return;
        }

        // Act
        var createOrUpdateFileModel = new CreateOrUpdateFileModel
        {
            Name = "new-file",
            FolderId = folder.Id
        };
        HttpContent content = new StringContent(
            JsonSerializer.Serialize<CreateOrUpdateFileModel>(
                createOrUpdateFileModel,
                JsonOptions.CamelCasePolicy
            ),
            Encoding.UTF8,
            "application/json"
        );

        HttpResponseMessage httpResponse = await this.httpClient
            .PostAsync("/files", content).ConfigureAwait(false);
        FileResponseModel? actualFile = await httpResponse.Content
            .ReadFromJsonAsync<FileResponseModel>().ConfigureAwait(false);

        // Assert
        Assert.NotNull(actualFile);
        Assert.AreEqual(createOrUpdateFileModel.Name, actualFile?.Name);
        Assert.AreEqual(1, actualFile?.AncestorFolderIds.Count());
        Assert.AreEqual(folder.Id, actualFile?.AncestorFolderIds.First());
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

    private async Task<FolderResponseModel?> CreateRootLevelTestFolder()
    {
        // Arrange
        var createFolderModel = new CreateFolderModel
        {
            ParentFolderId = this.RootLevelFolderId,
            FolderName = "new-folder"
        };
        HttpContent requestContent = this.CreateRequestContent(createFolderModel);

        // Act
        HttpResponseMessage httpResponse = await this.httpClient
            .PostAsync("/folders", requestContent).ConfigureAwait(false);
        FolderResponseModel? createdFolder = await httpResponse.Content
            .ReadFromJsonAsync<FolderResponseModel>().ConfigureAwait(false);

        return createdFolder;
    }

    private HttpContent CreateRequestContent(CreateFolderModel createFolderModel)
    {
        string requestPayload = JsonSerializer.Serialize<CreateFolderModel>(
            createFolderModel,
            JsonOptions.CamelCasePolicy
        );
        return new StringContent(
            requestPayload,
            Encoding.UTF8,
            "application/json"
        );
    }
}