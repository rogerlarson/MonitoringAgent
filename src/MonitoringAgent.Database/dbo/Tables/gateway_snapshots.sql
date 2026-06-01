CREATE TABLE [dbo].[gateway_snapshots] (
    [gateway_snapshot_id] BIGINT        IDENTITY (1, 1) NOT NULL,
    [server_service_id]   INT           NOT NULL,
    [snapshot_utc]        DATETIME2 (7) NOT NULL,
    [reachable]           BIT           NOT NULL,
    [http_status_code]    INT           NOT NULL,
    [response_ms]         BIGINT        NOT NULL,
    [created_date_utc]    DATETIME2 (7) DEFAULT (sysutcdatetime()) NOT NULL,
    PRIMARY KEY CLUSTERED ([gateway_snapshot_id] ASC),
    CONSTRAINT [fk_gateway_snapshots_server_service] FOREIGN KEY ([server_service_id]) REFERENCES [dbo].[server_services] ([server_service_id])
);


GO
CREATE NONCLUSTERED INDEX [ix_gateway_snapshots_server_service_snapshot]
    ON [dbo].[gateway_snapshots]([server_service_id] ASC, [snapshot_utc] DESC);

