using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MonitoringAgent.Api.Data.Entities;

namespace MonitoringAgent.Api.Data.Configurations;

public sealed class ServiceTypeEntityConfiguration
    : IEntityTypeConfiguration<ServiceTypeEntity>
{
    public void Configure(
        EntityTypeBuilder<ServiceTypeEntity> builder)
    {
        builder.ToTable("service_types");

        builder.HasKey(
            x => x.ServiceTypeId);

        builder.Property(x => x.ServiceTypeId)
            .HasColumnName("service_type_id");

        builder.Property(x => x.ServiceTypeName)
            .HasColumnName("service_type_name");

        builder.Property(x => x.Description)
            .HasColumnName("description");

        builder.Property(x => x.CreatedDateUtc)
            .HasColumnName("created_date_utc");
    }
}