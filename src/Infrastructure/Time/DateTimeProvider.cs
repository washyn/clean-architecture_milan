using SharedKernel;
using Volo.Abp.DependencyInjection;

namespace Infrastructure.Time;

internal sealed class DateTimeProvider : IDateTimeProvider, ITransientDependency
{
    public DateTime UtcNow => DateTime.UtcNow;
}
