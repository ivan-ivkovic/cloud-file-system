using System.Net;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;
using P3Mobility.CloudFileSystem.FileSystem.Folders.Models;
using P3Mobility.CloudFileSystem.FileSystemApi.IntegrationTests.Helpers;

namespace P3Mobility.CloudFileSystem.FileSystemApi.IntegrationTests.Folders;

[TestFixture]
public class FolderTests
{
    private HttpClient httpClient;

    public FolderTests()
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

    [Test]
    public async Task CreateFolderAtRootLevel_FolderSuccessfullyCreated_ReturnsCreated()
    {
        // Arrange
        HttpStatusCode expectedStatusCode = HttpStatusCode.Created;
        var createFolderModel = new CreateFolderModel
        {
            ParentFolderId = Guid.Empty,
            FolderName = "new-folder"
        };
        string requestPayload = JsonSerializer.Serialize<CreateFolderModel>(
            createFolderModel,
            JsonOptions.CamelCasePolicy
        );

        // Act
        HttpContent content = new StringContent(
            requestPayload,
            Encoding.UTF8,
            "application/json"
        );
        HttpResponseMessage httpResponse = await this.httpClient
            .PostAsync("/folders", content).ConfigureAwait(false);
        FolderModel? createdFolder = await httpResponse.Content
            .ReadFromJsonAsync<FolderModel>().ConfigureAwait(false);

        // Assert
        Assert.IsNotNull(createdFolder);
        Assert.AreEqual(expectedStatusCode, httpResponse.StatusCode);
        Assert.AreEqual(createFolderModel.ParentFolderId, createdFolder?.ParentFolderId);
        Assert.AreEqual(createFolderModel.FolderName, createdFolder?.FolderName);
    }
}
