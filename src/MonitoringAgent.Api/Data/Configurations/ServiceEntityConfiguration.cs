using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MonitoringAgent.Api.Data.Entities;

namespace MonitoringAgent.Api.Data.Configurations;

public sealed class ServiceEntityConfiguration
    : IEntityTypeConfiguration<ServiceEntity>
{
    public void Configure(
        EntityTypeBuilder<ServiceEntity> builder)
    {
        builder.ToTable("services");

        builder.HasKey(
            x => x.ServiceId);

        builder.Property(x => x.ServiceId)
            .HasColumnName("service_id");

        builder.Property(x => x.ServiceName)
            .HasColumnName("service_name");

        builder.Property(x => x.ServiceTypeId)
            .HasColumnName("service_type_id");

        builder.Property(x => x.CollectorName)
            .HasColumnName("collector_name");

        builder.Property(x => x.RegistrationMode)
            .HasColumnName("registration_mode")
            .HasConversion<string>();

        builder.Property(x => x.CreatedDateUtc)
            .HasColumnName("created_date_utc");

        builder.HasOne(x => x.ServiceType)
            .WithMany(x => x.Services)
            .HasForeignKey(x => x.ServiceTypeId);
    }
}