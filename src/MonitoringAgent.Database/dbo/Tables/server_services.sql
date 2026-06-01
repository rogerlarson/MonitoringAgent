CREATE TABLE [dbo].[server_services] (
    [server_service_id]     INT            IDENTITY (1, 1) NOT NULL,
    [server_id]             INT            NOT NULL,
    [service_id]            INT            NOT NULL,
    [service_instance_name] NVARCHAR (100) NULL,
    [is_enabled]            BIT            DEFAULT ((1)) NOT NULL,
    [created_date_utc]      DATETIME2 (7)  DEFAULT (sysutcdatetime()) NOT NULL,
    PRIMARY KEY CLUSTERED ([server_service_id] ASC),
    CONSTRAINT [fk_server_services_server] FOREIGN KEY ([server_id]) REFERENCES [dbo].[servers] ([server_id]),
    CONSTRAINT [fk_server_services_service] FOREIGN KEY ([service_id]) REFERENCES [dbo].[services] ([service_id])
);

