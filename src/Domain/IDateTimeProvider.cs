using Volo.Abp.DependencyInjection;

namespace SharedKernel
{
    public interface IDateTimeProvider : ITransientDependency
    {
        public DateTime UtcNow { get; }
    }
}
