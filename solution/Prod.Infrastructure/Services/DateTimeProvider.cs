using Prod.Application.Common.Interfaces.Services;

namespace Prod.Infrastructure.Services;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}