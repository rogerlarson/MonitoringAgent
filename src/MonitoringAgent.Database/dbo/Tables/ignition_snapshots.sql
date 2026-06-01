CREATE TABLE [dbo].[ignition_snapshots] (
    [ignition_snapshot_id] BIGINT         IDENTITY (1, 1) NOT NULL,
    [server_service_id]    INT            NOT NULL,
    [snapshot_utc]         DATETIME2 (7)  NOT NULL,
    [service_running]      BIT            NOT NULL,
    [process_running]      BIT            NOT NULL,
    [ignition_version]     NVARCHAR (100) NULL,
    [java_version]         NVARCHAR (100) NULL,
    [cpu_percent]          DECIMAL (9, 2) NOT NULL,
    [memory_mb]            BIGINT         NOT NULL,
    [thread_count]         INT            NOT NULL,
    [handle_count]         INT            NOT NULL,
    [uptime_minutes]       BIGINT         NOT NULL,
    [process_id]           INT            NOT NULL,
    [process_name]         NVARCHAR (100) NULL,
    [created_date_utc]     DATETIME2 (7)  CONSTRAINT [DF__ignition___creat__5BE2A6F2] DEFAULT (sysutcdatetime()) NOT NULL,
    CONSTRAINT [PK__ignition__4DC43B6E3AFC6DC5] PRIMARY KEY CLUSTERED ([ignition_snapshot_id] ASC),
    CONSTRAINT [fk_ignition_snapshots_server_service] FOREIGN KEY ([server_service_id]) REFERENCES [dbo].[server_services] ([server_service_id])
);


GO
CREATE NONCLUSTERED INDEX [ix_ignition_snapshots_server_service_snapshot]
    ON [dbo].[ignition_snapshots]([server_service_id] ASC, [snapshot_utc] DESC);

