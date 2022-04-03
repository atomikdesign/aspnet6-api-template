using Microsoft.ApplicationInsights;
using Microsoft.Extensions.Configuration;
using Template.AspNet6.Domain.Entities.Users;
using Template.AspNet6.Domain.ValueObjects.Email;

namespace Template.AspNet6.Infra.Auth.Providers.Custom;

public class CustomSsoProvider : IProvider
{
    private readonly CustomOpenIdProxy _proxy;
    private readonly IUserFactory _userFactory;

    public CustomSsoProvider(IConfiguration config, IUserFactory userFactory, TelemetryClient telemetry)
    {
        _userFactory = userFactory;
        _proxy = new CustomOpenIdProxy(config, telemetry);
    }

    public async Task<User> GetUserAsync(string code)
    {
        var accessToken = await _proxy.GetAccessTokenAsync(code);
        var userInfo = await _proxy.GetIdentityInformationsAsync(accessToken);

        var user = _userFactory.NewActivatedUser(userInfo.firstname, userInfo.lastname, new Email(userInfo.emailcontact));
        return user;
    }
}
