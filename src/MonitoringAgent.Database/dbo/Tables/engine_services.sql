CREATE TABLE [dbo].[engine_services] (
    [engine_service_id] INT              IDENTITY (1, 1) NOT NULL,
    [service_name]      NVARCHAR (200)   NOT NULL,
    [status]            NVARCHAR (50)    NOT NULL,
    [started_utc]       DATETIME2 (7)    NOT NULL,
    [last_run_utc]      DATETIME2 (7)    NULL,
    [last_success_utc]  DATETIME2 (7)    NULL,
    [run_count]         BIGINT           DEFAULT ((0)) NOT NULL,
    [error_count]       BIGINT           DEFAULT ((0)) NOT NULL,
    [last_duration_ms]  BIGINT           NULL,
    [last_error]        NVARCHAR (MAX)   NULL,
    [instance_id]       UNIQUEIDENTIFIER NOT NULL,
    [current_state]     NVARCHAR (100)   NULL,
    PRIMARY KEY CLUSTERED ([engine_service_id] ASC)
);

