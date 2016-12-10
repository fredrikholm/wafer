![Wafer](https://thumbs.dreamstime.com/x/two-wafers-vector-20470684.jpg)

# Wafer - a Web API Integration Testing Host Runner

Wafer provides an in-memory testing host for your ASP.NET Web API. It creates an HttpServer for you and provide you with a simple way to issue "real" requests against your endpoints, like this:  

```cs
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
```

## What about the name?

Web API Integration Testing Host Runner --> W.A.I.T.H.R. --> Waithr --> Wafer.
