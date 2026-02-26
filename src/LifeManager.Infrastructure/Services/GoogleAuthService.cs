using Google.Apis.Auth;
using LifeManager.Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;

namespace LifeManager.Infrastructure.Services;

public class GoogleAuthService : IGoogleAuthService
{
    private readonly IConfiguration _configuration;

    public GoogleAuthService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<GoogleUserInfo> ValidateGoogleTokenAsync(
        string idToken,
        CancellationToken cancellationToken = default)
    {
        var settings = new GoogleJsonWebSignature.ValidationSettings
        {
            Audience = [_configuration["Google:ClientId"]!]
        };

        var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);

        return new GoogleUserInfo(
            GoogleId: payload.Subject,
            Email: payload.Email,
            Name: payload.Name,
            AvatarUrl: payload.Picture);
    }
}
