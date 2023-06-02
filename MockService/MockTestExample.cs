using MbDotNet;
using MbDotNet.Models;
using NUnit.Framework;
using System.Net;

namespace MockService
{
    [TestFixture]
    public class MockTestExample
    {
        private const int ImposterPort = 3000;
        private MountebankClient _mountebankClient;
        private HttpClient _httpClient;

        [SetUp]
        public async Task SetUp()
        {
            _mountebankClient = new MountebankClient();
            _httpClient = new HttpClient();

            await _mountebankClient.DeleteAllImpostersAsync();
        }

        [Test]
        public async Task CheckHomePage()
        {
            var imposter = _mountebankClient.CreateHttpImposterAsync(ImposterPort, imposter =>
            {
                imposter.AddStub()
                    .OnPathAndMethodEqual("/home", Method.Get)
                    .ReturnsBody(HttpStatusCode.OK, "Welcome in our store!");
            });
            var response = await _httpClient.GetAsync($"http://localhost:{ImposterPort}/home");
            var responseContent = await response.Content.ReadAsStringAsync();

            Assert.That(responseContent, Is.EqualTo("Welcome in our store!"));
        }

        [Test]
        public async Task CheckAuthorization()
        {
            var imposter = _mountebankClient.CreateHttpImposterAsync(ImposterPort, imposter =>
            {
                imposter.AddStub()
                .OnPathAndMethodEqual("/settings", Method.Get)
                .ReturnsBody(HttpStatusCode.Unauthorized, "You need to login before configure account settings");
            });

            var response = await _httpClient.GetAsync($"http://localhost:{ImposterPort}/settings");
            var message = await response.Content.ReadAsStringAsync();

            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
                Assert.That(message, Is.EqualTo("You need to login before configure account settings"));
            });
        }

        [Test]
        public async Task CheckRequestTimeOut()
        {
            var imposter = _mountebankClient.CreateHttpImposterAsync(ImposterPort, imposter =>
            {
                imposter.AddStub()
                .OnPathAndMethodEqual("/signup", Method.Get)
                .ReturnsBody(HttpStatusCode.RequestTimeout, "Oops! Time is up. Please check your connection");
            });

            var response = await _httpClient.GetAsync($"http://localhost:{ImposterPort}/signup");
            var message = await response.Content.ReadAsStringAsync();

            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.RequestTimeout));
                Assert.That(message, Is.EqualTo("Oops! Time is up. Please check your connection"));
            });
        }
    }
}