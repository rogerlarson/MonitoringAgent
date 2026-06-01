using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MonitoringAgent.Api.Data.Converters;

public sealed class NullableUtcDateTimeConverter
    : ValueConverter<DateTime?, DateTime?>
{
    public NullableUtcDateTimeConverter()
        : base(
            v => v,
            v =>
                v == null
                    ? null
                    : DateTime.SpecifyKind(
                        v.Value,
                        DateTimeKind.Utc))
    {
    }
}