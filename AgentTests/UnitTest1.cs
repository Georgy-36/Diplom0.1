using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Diplom0._1
{
    public class FileTransferTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public FileTransferTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task TestFileTransfer()
        {
            var client = _factory.CreateClient();

            var fileName = "testfile.txt";
            var fileContent = Convert.ToBase64String(Encoding.UTF8.GetBytes("This is a test file content."));

            var content = new StringContent(JsonConvert.SerializeObject(new
            {
                FileName = fileName,
                FileContent = fileContent
            }), Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/file/receive_file", content);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();
            Assert.Equal($"File {fileName} received successfully", result);
        }
    }
}
