CREATE TABLE [dbo].[host_snapshots] (
    [snapshot_id]                    BIGINT          IDENTITY (1, 1) NOT NULL,
    [server_id]                      INT             NOT NULL,
    [snapshot_utc]                   DATETIME2 (7)   NOT NULL,
    [cpu_percent]                    DECIMAL (9, 2)  NOT NULL,
    [memory_percent]                 DECIMAL (9, 2)  NOT NULL,
    [available_memory_mb]            BIGINT          NOT NULL,
    [process_count]                  INT             NOT NULL,
    [system_uptime_minutes]          BIGINT          NOT NULL,
    [context_switches_per_sec]       DECIMAL (18, 2) NOT NULL,
    [page_faults_per_sec]            DECIMAL (18, 2) NOT NULL,
    [system_drive]                   NVARCHAR (10)   NULL,
    [disk_percent_used]              DECIMAL (9, 2)  NOT NULL,
    [disk_free_gb]                   DECIMAL (18, 2) NOT NULL,
    [disk_reads_per_sec]             DECIMAL (18, 2) NOT NULL,
    [disk_writes_per_sec]            DECIMAL (18, 2) NOT NULL,
    [disk_read_latency_ms]           DECIMAL (18, 2) NOT NULL,
    [disk_write_latency_ms]          DECIMAL (18, 2) NOT NULL,
    [disk_queue_length]              DECIMAL (18, 2) NOT NULL,
    [avg_disk_queue_length]          DECIMAL (18, 4) CONSTRAINT [DF__host_snap__avg_d__1DB06A4F] DEFAULT ((0)) NOT NULL,
    [primary_network_interface]      NVARCHAR (200)  NULL,
    [network_bytes_received_per_sec] DECIMAL (18, 2) NOT NULL,
    [network_bytes_sent_per_sec]     DECIMAL (18, 2) NOT NULL,
    [network_receive_errors]         BIGINT          NOT NULL,
    [network_send_errors]            BIGINT          NOT NULL,
    [network_receive_discards]       BIGINT          NOT NULL,
    [network_send_discards]          BIGINT          NOT NULL,
    [tcp_retransmissions_per_sec]    DECIMAL (18, 2) NOT NULL,
    [created_date_utc]               DATETIME2 (7)   CONSTRAINT [df_health_snapshots_created_date_utc] DEFAULT (sysutcdatetime()) NOT NULL,
    CONSTRAINT [pk_host_snapshots] PRIMARY KEY CLUSTERED ([snapshot_id] ASC),
    CONSTRAINT [fk_host_snapshots_servers] FOREIGN KEY ([server_id]) REFERENCES [dbo].[servers] ([server_id])
);


GO
CREATE NONCLUSTERED INDEX [ix_health_snapshots_server_snapshot]
    ON [dbo].[host_snapshots]([server_id] ASC, [snapshot_utc] DESC);


GO
CREATE NONCLUSTERED INDEX [ix_health_snapshots_snapshot_utc]
    ON [dbo].[host_snapshots]([snapshot_utc] DESC);


GO
CREATE NONCLUSTERED INDEX [ix_health_snapshots_server_id_snapshot_utc]
    ON [dbo].[host_snapshots]([server_id] ASC, [snapshot_utc] DESC);

