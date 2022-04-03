using Microsoft.ApplicationInsights;
using Microsoft.Extensions.Configuration;
using Template.AspNet6.Domain.Entities.Users;
using Template.AspNet6.Domain.ValueObjects.Email;

namespace Template.AspNet6.Infra.Auth.Providers.Google;

public class GoogleSsoProvider : IProvider
{
    private readonly GoogleAuthProxy _proxy;
    private readonly IUserFactory _userFactory;

    public GoogleSsoProvider(IConfiguration config, IUserFactory userFactory, TelemetryClient telemetry)
    {
        _userFactory = userFactory;
        _proxy = new GoogleAuthProxy(config, telemetry);
    }

    public async Task<User> GetUserAsync(string code)
    {
        var userInfo = await _proxy.GetUserInfoAsync(code);

        var user = _userFactory.NewActivatedUser(userInfo.given_name, userInfo.family_name, new Email(userInfo.email), userInfo.picture);
        return user;
    }
}
