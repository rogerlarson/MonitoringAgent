CREATE TABLE [dbo].[service_types] (
    [service_type_id]   INT            IDENTITY (1, 1) NOT NULL,
    [service_type_name] NVARCHAR (50)  NOT NULL,
    [description]       NVARCHAR (200) NULL,
    [created_date_utc]  DATETIME2 (7)  DEFAULT (sysutcdatetime()) NOT NULL,
    PRIMARY KEY CLUSTERED ([service_type_id] ASC),
    CONSTRAINT [uq_service_types_name] UNIQUE NONCLUSTERED ([service_type_name] ASC)
);

