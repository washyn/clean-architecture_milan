using Volo.Abp.DependencyInjection;

namespace Application.Abstractions.Authentication;

public interface IPasswordHasher : ITransientDependency
{
    string Hash(string password);

    bool Verify(string password, string passwordHash);
}
