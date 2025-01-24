using Volo.Abp.DependencyInjection;

namespace Application.Abstractions.Authentication;

public interface IUserContext : ITransientDependency
{
    Guid UserId { get; }
}
