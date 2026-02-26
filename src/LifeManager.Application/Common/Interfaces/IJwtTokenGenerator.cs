using LifeManager.Domain.Entities;

namespace LifeManager.Application.Common.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}
