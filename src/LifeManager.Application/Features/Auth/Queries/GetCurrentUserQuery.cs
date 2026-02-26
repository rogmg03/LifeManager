using LifeManager.Application.Common.Exceptions;
using LifeManager.Application.Common.Interfaces;
using LifeManager.Application.Features.Auth.DTOs;
using LifeManager.Domain.Interfaces.Repositories;
using MediatR;

namespace LifeManager.Application.Features.Auth.Queries;

public record GetCurrentUserQuery : IRequest<UserDto>;

public class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, UserDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;

    public GetCurrentUserQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUser)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<UserDto> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(_currentUser.UserId, cancellationToken)
            ?? throw new NotFoundException("User", _currentUser.UserId);

        return new UserDto(
            Id: user.Id,
            Email: user.Email,
            Name: user.Name,
            AvatarUrl: user.AvatarUrl,
            Theme: user.Settings?.Theme.ToString() ?? "Light",
            TimeZone: user.Settings?.TimeZone ?? "UTC");
    }
}
