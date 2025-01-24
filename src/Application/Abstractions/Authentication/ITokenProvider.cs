using Domain.Users;
using Volo.Abp.DependencyInjection;

namespace Application.Abstractions.Authentication;

public interface ITokenProvider : ITransientDependency
{
    string Create(User user);
}
