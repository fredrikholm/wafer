using System;
using System.Net;
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
        public void Can_Get_List_Of_Users()
        {
            var response = _hostRunner.Get("api/users");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
