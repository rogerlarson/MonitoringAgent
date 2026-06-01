using Microsoft.EntityFrameworkCore;
using MonitoringAgent.Api.Data.Entities;
using MonitoringAgent.Api.Data.Converters;

namespace MonitoringAgent.Api.Data;

public sealed class MonitoringDbContext
    : DbContext
{
    public MonitoringDbContext(
        DbContextOptions<MonitoringDbContext> options)
        : base(options)
    {
    }

    public DbSet<ServerEntity> Servers =>
        Set<ServerEntity>();

    public DbSet<HostSnapshotEntity> HostSnapshots =>
        Set<HostSnapshotEntity>();

    public DbSet<ServiceTypeEntity> ServiceTypes =>
    Set<ServiceTypeEntity>();

    public DbSet<ServiceEntity> Services =>
        Set<ServiceEntity>();

    public DbSet<ServerServiceEntity> ServerServices =>
        Set<ServerServiceEntity>();

    public DbSet<IgnitionSnapshotEntity> IgnitionSnapshots =>
        Set<IgnitionSnapshotEntity>();

    public DbSet<GatewaySnapshotEntity> GatewaySnapshots =>
        Set<GatewaySnapshotEntity>();

    public DbSet<AlertRuleEntity> AlertRules =>
    Set<AlertRuleEntity>();

    public DbSet<AlertEventEntity> AlertEvents =>
        Set<AlertEventEntity>();

    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(MonitoringDbContext).Assembly);
        
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                if (property.ClrType == typeof(DateTime))
                {
                    property.SetValueConverter(
                        new UtcDateTimeConverter());
                }

                if (property.ClrType == typeof(DateTime?))
                {
                    property.SetValueConverter(
                        new NullableUtcDateTimeConverter());
                }
            }
        }

        base.OnModelCreating(modelBuilder);
    }
}