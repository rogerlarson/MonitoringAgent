CREATE TABLE [dbo].[servers] (
    [server_id]                INT            IDENTITY (1, 1) NOT NULL,
    [server_name]              NVARCHAR (100) NOT NULL,
    [operating_system]         NVARCHAR (100) NULL,
    [operating_system_version] NVARCHAR (200) NULL,
    [processor_count]          INT            NULL,
    [total_memory_mb]          BIGINT         NULL,
    [domain_name]              NVARCHAR (255) CONSTRAINT [DF_servers_domain_name] DEFAULT ('') NULL,
    [agent_version]            NVARCHAR (255) CONSTRAINT [DF_servers_agent_version] DEFAULT ('') NULL,
    [created_date_utc]         DATETIME2 (7)  CONSTRAINT [df_servers_created_date_utc] DEFAULT (sysutcdatetime()) NOT NULL,
    [last_seen_utc]            DATETIME2 (7)  NULL,
    [status]                   NVARCHAR (20)  CONSTRAINT [df_servers_status] DEFAULT ('Unknown') NOT NULL,
    CONSTRAINT [pk_servers] PRIMARY KEY CLUSTERED ([server_id] ASC),
    CONSTRAINT [uq_servers_server_name] UNIQUE NONCLUSTERED ([server_name] ASC)
);

