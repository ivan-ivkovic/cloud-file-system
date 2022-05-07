using System.Net.Http;
using NUnit.Framework;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Threading.Tasks;

namespace FileSystemApi.IntegrationTests;

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
    public async Task GetFiles_ReturnsAllFiles()
    {
        // Arrange
        var expectedFiles = "[{\"id\":\"013b03b8-5787-40c4-889e-adc9e8c605d1\"," +
            "\"name\":\"file1.txt\"},{\"id\":\"5160cb78-2aef-435e-9cb4-0008bcb9c080\","+
            "\"name\":\"file2.txt\"}]";

        // Act
        var response = await this.httpClient.GetAsync("/files");
        var actualFiles = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.AreEqual(expectedFiles, actualFiles);
    }

    [Test]
    public async Task GetFileById_FileExists_ReturnsCorrectFile()
    {
        // Arrange
        var fileId = "013b03b8-5787-40c4-889e-adc9e8c605d1";
        var expectedFile = $"{{\"id\":\"{fileId}\",\"name\":\"file1.txt\"}}";

        // Act
        var response = await this.httpClient.GetAsync($"/files/{fileId}");
        var actualFile = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.AreEqual(expectedFile, actualFile);
    }
}
