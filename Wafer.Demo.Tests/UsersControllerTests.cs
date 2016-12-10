using System;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Wafer.Demo.Tests
{
    public class UsersControllerTests
    {
        private readonly HostRunner _hostRunner;

        public UsersControllerTests()
        {
            _hostRunner = new HostRunner(new Uri("http://localhost"), WebApiConfig.Register);
        }

        [Fact]
        public async Task Can_Get_List_Of_Users()
        {
            var response = await _hostRunner.GetAsync("api/users");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
