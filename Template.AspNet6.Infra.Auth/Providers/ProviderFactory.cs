using Microsoft.ApplicationInsights;
using Microsoft.Extensions.Configuration;
using Template.AspNet6.Domain.Entities.Users;
using Template.AspNet6.Infra.Auth.Providers.Custom;
using Template.AspNet6.Infra.Auth.Providers.Google;
using Template.AspNet6.Infra.Auth.Providers.Microsoft;

namespace Template.AspNet6.Infra.Auth.Providers;

public class ProviderFactory
{
    private readonly IConfiguration _config;
    private readonly TelemetryClient _telemetry;
    private readonly IUserFactory _userFactory;

    public ProviderFactory(IConfiguration config, IUserFactory userFactory, TelemetryClient telemetry)
    {
        _config = config;
        _userFactory = userFactory;
        _telemetry = telemetry;
    }

    public IProvider Create(ProviderType providerType)
    {
        return providerType switch
        {
            ProviderType.Custom => new CustomSsoProvider(_config, _userFactory, _telemetry),
            ProviderType.Google => new GoogleSsoProvider(_config, _userFactory, _telemetry),
            ProviderType.Microsoft => new MicrosoftSsoProvider(_config, _userFactory, _telemetry),
            _ => throw new ArgumentOutOfRangeException(nameof(providerType), providerType, null)
        };
    }
}

public interface IProvider
{
    Task<User> GetUserAsync(string code);
}
