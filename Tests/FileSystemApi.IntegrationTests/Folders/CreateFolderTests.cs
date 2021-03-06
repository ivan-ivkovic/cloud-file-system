using System.Linq;
using System.Net;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;
using CloudFileSystem.FileSystem.Folders.Models;
using CloudFileSystem.FileSystemApi.IntegrationTests.Helpers;

namespace CloudFileSystem.FileSystemApi.IntegrationTests.Folders;

[TestFixture]
public class CreateFolderTests
{
    private readonly HttpClient httpClient;
    private readonly Guid RootLevelFolderId;

    public CreateFolderTests()
    {
        var webApplicationFactory = new WebApplicationFactory<Program>();
        this.httpClient = webApplicationFactory.CreateDefaultClient();
        this.RootLevelFolderId = Guid.Empty;
    }

    [Test]
    public async Task CreateFolderAtRootLevel_FolderSuccessfullyCreated_ReturnsCreated()
    {
        // Arrange
        HttpStatusCode expectedStatusCode = HttpStatusCode.Created;
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

        // Assert
        Assert.IsNotNull(createdFolder);
        Assert.AreEqual(expectedStatusCode, httpResponse.StatusCode);
        Assert.AreEqual(createFolderModel.FolderName, createdFolder?.FolderName);
        Assert.AreEqual(0, createdFolder?.ParentFolderIds.Count());
    }

    [Test]
    public async Task CreateFolderInsideAFolder_FolderSuccessfullyCreated_ReturnsCreated()
    {
        // Arrange
        HttpStatusCode expectedStatusCode = HttpStatusCode.Created;
        var requestFolder1 = new CreateFolderModel
        {
            ParentFolderId = this.RootLevelFolderId,
            FolderName = "folder1"
        };
        HttpContent requestContent = this.CreateRequestContent(requestFolder1);

        HttpResponseMessage httpResponse = await this.httpClient
            .PostAsync("/folders", requestContent).ConfigureAwait(false);
        FolderResponseModel? responseFolder1 = await httpResponse.Content
            .ReadFromJsonAsync<FolderResponseModel>().ConfigureAwait(false);

        if (responseFolder1 == null)
        {
            Assert.Fail();
            return;
        }

        var requestFolder2 = new CreateFolderModel
        {
            ParentFolderId = responseFolder1.Id,
            FolderName = "folder2"
        };
        requestContent = this.CreateRequestContent(requestFolder2);

        // Act
        httpResponse = await this.httpClient
            .PostAsync("/folders", requestContent).ConfigureAwait(false);
        FolderResponseModel? responseFolder2 = await httpResponse.Content
            .ReadFromJsonAsync<FolderResponseModel>().ConfigureAwait(false);

        // Assert
        Assert.IsNotNull(responseFolder1);
        Assert.IsNotNull(responseFolder2);
        Assert.AreEqual(expectedStatusCode, httpResponse.StatusCode);
        Assert.AreEqual(requestFolder2.ParentFolderId, responseFolder1.Id);
        Assert.AreEqual(requestFolder2.FolderName, responseFolder2?.FolderName);
        Assert.AreEqual(1, responseFolder2?.ParentFolderIds.Count());
        Assert.AreEqual(responseFolder1.Id, responseFolder2?.ParentFolderIds.First());
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
