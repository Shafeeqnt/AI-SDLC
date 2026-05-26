using GMS.Domain.Entities;

namespace GMS.Application.Common.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user, IReadOnlyCollection<Role> roles);
}
