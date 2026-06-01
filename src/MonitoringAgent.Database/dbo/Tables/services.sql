CREATE TABLE [dbo].[services] (
    [service_id]        INT            IDENTITY (1, 1) NOT NULL,
    [service_name]      NVARCHAR (100) NOT NULL,
    [service_type_id]   INT            NULL,
    [collector_name]    NVARCHAR (100) NULL,
    [registration_mode] NVARCHAR (20)  CONSTRAINT [DF__services__regist__68487DD7] DEFAULT ('manual') NOT NULL,
    [created_date_utc]  DATETIME2 (7)  CONSTRAINT [DF__services__create__403A8C7D] DEFAULT (sysutcdatetime()) NOT NULL,
    CONSTRAINT [PK__services__3E0DB8AF73E18FB8] PRIMARY KEY CLUSTERED ([service_id] ASC),
    CONSTRAINT [ck_services_registration_mode] CHECK ([registration_mode]='detected' OR [registration_mode]='global' OR [registration_mode]='manual'),
    CONSTRAINT [fk_services_service_types] FOREIGN KEY ([service_type_id]) REFERENCES [dbo].[service_types] ([service_type_id])
);

