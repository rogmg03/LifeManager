using LifeManager.Application.Common.Behaviors;
using LifeManager.Application.Common.Interfaces;
using LifeManager.Application.Features.Auth.DTOs;
using LifeManager.Domain.Entities;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.Auth.Commands;

public record GoogleLoginCommand(string IdToken) : IRequest<AuthResponseDto>, IBaseCommand;

public class GoogleLoginCommandHandler : IRequestHandler<GoogleLoginCommand, AuthResponseDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGoogleAuthService _googleAuth;
    private readonly IJwtTokenGenerator _jwtGenerator;

    public GoogleLoginCommandHandler(
        IUnitOfWork unitOfWork,
        IGoogleAuthService googleAuth,
        IJwtTokenGenerator jwtGenerator)
    {
        _unitOfWork = unitOfWork;
        _googleAuth = googleAuth;
        _jwtGenerator = jwtGenerator;
    }

    public async Task<AuthResponseDto> Handle(
        GoogleLoginCommand request,
        CancellationToken cancellationToken)
    {
        var googleUser = await _googleAuth.ValidateGoogleTokenAsync(request.IdToken, cancellationToken);

        var user = await _unitOfWork.Users.GetByGoogleIdAsync(googleUser.GoogleId, cancellationToken);

        if (user is null)
        {
            // New user — create with default settings
            user = new User
            {
                Email = googleUser.Email,
                Name = googleUser.Name,
                AvatarUrl = googleUser.AvatarUrl,
                GoogleId = googleUser.GoogleId,
                Settings = new UserSettings()
            };
            await _unitOfWork.Users.AddAsync(user, cancellationToken);
        }
        else
        {
            // Existing user — update profile info
            user.Name = googleUser.Name;
            user.AvatarUrl = googleUser.AvatarUrl;
            _unitOfWork.Users.Update(user);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var token = _jwtGenerator.GenerateToken(user);

        return new AuthResponseDto(
            Token: token,
            UserId: user.Id,
            Email: user.Email,
            Name: user.Name,
            AvatarUrl: user.AvatarUrl);
    }
}
