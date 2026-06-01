using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MonitoringAgent.Api.Data.Converters;

public sealed class UtcDateTimeConverter
    : ValueConverter<DateTime, DateTime>
{
    public UtcDateTimeConverter()
        : base(
            v => v,
            v => DateTime.SpecifyKind(
                v,
                DateTimeKind.Utc))
    {
    }
}