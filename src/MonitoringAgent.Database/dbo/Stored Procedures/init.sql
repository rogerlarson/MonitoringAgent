-- =============================================
-- Author:		Roger Larson
-- Create date: 06/07/2026
-- Description:	Initial seed data for this database...
-- =============================================
CREATE PROCEDURE [dbo].[init]
AS
BEGIN
    BEGIN TRANSACTION;

    -- ============================================================================
    -- SERVICE TYPES
    -- ============================================================================

    IF NOT EXISTS
    (
        SELECT 1
        FROM dbo.service_types
        WHERE service_type_name = 'SCADA'
    )
    BEGIN
        INSERT INTO dbo.service_types
        (
            service_type_name,
            description,
            created_date_utc
        )
        VALUES
        (
            'SCADA',
            'Industrial control systems',
            GETUTCDATE()
        );
    END;

    IF NOT EXISTS
    (
        SELECT 1
        FROM dbo.service_types
        WHERE service_type_name = 'Web'
    )
    BEGIN
        INSERT INTO dbo.service_types
        (
            service_type_name,
            description,
            created_date_utc
        )
        VALUES
        (
            'Web',
            'HTTP services',
            GETUTCDATE()
        );
    END;

    IF NOT EXISTS
    (
        SELECT 1
        FROM dbo.service_types
        WHERE service_type_name = 'WindowsService'
    )
    BEGIN
        INSERT INTO dbo.service_types
        (
            service_type_name,
            description,
            created_date_utc
        )
        VALUES
        (
            'WindowsService',
            'Windows services',
            GETUTCDATE()
        );
    END;

    IF NOT EXISTS
    (
        SELECT 1
        FROM dbo.service_types
        WHERE service_type_name = 'Database'
    )
    BEGIN
        INSERT INTO dbo.service_types
        (
            service_type_name,
            description,
            created_date_utc
        )
        VALUES
        (
            'Database',
            'Database servers',
            GETUTCDATE()
        );
    END;

    IF NOT EXISTS
    (
        SELECT 1
        FROM dbo.service_types
        WHERE service_type_name = 'Broker'
    )
    BEGIN
        INSERT INTO dbo.service_types
        (
            service_type_name,
            description,
            created_date_utc
        )
        VALUES
        (
            'Broker',
            'Messaging systems',
            GETUTCDATE()
        );
    END;

    -- ============================================================================
    -- SERVICES
    -- ============================================================================

    DECLARE @ScadaTypeId INT =
    (
        SELECT service_type_id
        FROM dbo.service_types
        WHERE service_type_name = 'SCADA'
    );

    DECLARE @WebTypeId INT =
    (
        SELECT service_type_id
        FROM dbo.service_types
        WHERE service_type_name = 'Web'
    );

    IF NOT EXISTS
    (
        SELECT 1
        FROM dbo.services
        WHERE service_name = 'Ignition'
    )
    BEGIN
        INSERT INTO dbo.services
        (
            service_name,
            service_type_id,
            collector_name,
            registration_mode,
            created_date_utc
        )
        VALUES
        (
            'Ignition',
            @ScadaTypeId,
            'IgnitionCollector',
            'global',
            GETUTCDATE()
        );
    END;

    IF NOT EXISTS
    (
        SELECT 1
        FROM dbo.services
        WHERE service_name = 'Gateway'
    )
    BEGIN
        INSERT INTO dbo.services
        (
            service_name,
            service_type_id,
            collector_name,
            registration_mode,
            created_date_utc
        )
        VALUES
        (
            'Gateway',
            @WebTypeId,
            'GatewayCollector',
            'global',
            GETUTCDATE()
        );
    END;

    IF NOT EXISTS
    (
        SELECT 1
        FROM dbo.services
        WHERE service_name = 'Test Service'
    )
    BEGIN
        INSERT INTO dbo.services
        (
            service_name,
            service_type_id,
            collector_name,
            registration_mode,
            created_date_utc
        )
        VALUES
        (
            'Test Service',
            @ScadaTypeId,
            'TestCollector',
            'manual',
            GETUTCDATE()
        );
    END;

    -- ============================================================================
    -- ALERT RULES
    -- ============================================================================

    IF NOT EXISTS
    (
        SELECT 1
        FROM dbo.alert_rules
        WHERE rule_name = 'High CPU'
    )
    BEGIN
        INSERT INTO dbo.alert_rules
        (
            rule_name,
            metric_name,
            operator,
            threshold_value,
            severity,
            sustain_seconds,
            repeat_seconds,
            auto_close,
            email_enabled,
            is_enabled,
            created_date_utc
        )
        VALUES
        (
            'High CPU',
            'cpu_percent',
            'GreaterThan',
            80,
            'Warning',
            300,
            3600,
            1,
            1,
            1,
            GETUTCDATE()
        );
    END;

    IF NOT EXISTS
    (
        SELECT 1
        FROM dbo.alert_rules
        WHERE rule_name = 'High Memory'
    )
    BEGIN
        INSERT INTO dbo.alert_rules
        (
            rule_name,
            metric_name,
            operator,
            threshold_value,
            severity,
            sustain_seconds,
            repeat_seconds,
            auto_close,
            email_enabled,
            is_enabled,
            created_date_utc
        )
        VALUES
        (
            'High Memory',
            'memory_percent',
            'GreaterThan',
            85,
            'Warning',
            300,
            3600,
            1,
            1,
            1,
            GETUTCDATE()
        );
    END;

    IF NOT EXISTS
    (
        SELECT 1
        FROM dbo.alert_rules
        WHERE rule_name = 'Disk Full'
    )
    BEGIN
        INSERT INTO dbo.alert_rules
        VALUES
        (
            'Disk Full',
            'disk_percent_used',
            'GreaterThan',
            95,
            'Critical',
            300,
            3600,
            1,
            1,
            1,
            GETUTCDATE()
        );
    END;

    IF NOT EXISTS
    (
        SELECT 1
        FROM dbo.alert_rules
        WHERE rule_name = 'Gateway Down'
    )
    BEGIN
        INSERT INTO dbo.alert_rules
        VALUES
        (
            'Gateway Down',
            'reachable',
            'Equal',
            0,
            'Critical',
            30,
            1800,
            1,
            1,
            1,
            GETUTCDATE()
        );
    END;

    IF NOT EXISTS
    (
        SELECT 1
        FROM dbo.alert_rules
        WHERE rule_name = 'Ignition Down'
    )
    BEGIN
        INSERT INTO dbo.alert_rules
        VALUES
        (
            'Ignition Down',
            'process_running',
            'Equal',
            0,
            'Critical',
            30,
            1800,
            1,
            1,
            1,
            GETUTCDATE()
        );
    END;

    IF NOT EXISTS
    (
        SELECT 1
        FROM dbo.alert_rules
        WHERE rule_name = 'Host Offline'
    )
    BEGIN
        INSERT INTO dbo.alert_rules
        VALUES
        (
            'Host Offline',
            'heartbeat',
            'GreaterThan',
            120,
            'Critical',
            120,
            1800,
            1,
            1,
            1,
            GETUTCDATE()
        );
    END;

    IF NOT EXISTS
    (
        SELECT 1
        FROM dbo.alert_rules
        WHERE rule_name = 'Disk Warning'
    )
    BEGIN
        INSERT INTO dbo.alert_rules
        VALUES
        (
            'Disk Warning',
            'disk_percent_used',
            'GreaterThan',
            85,
            'Warning',
            300,
            3600,
            1,
            1,
            1,
            GETUTCDATE()
        );
    END;

    IF NOT EXISTS
    (
        SELECT 1
        FROM dbo.alert_rules
        WHERE rule_name = 'CPU Critical'
    )
    BEGIN
        INSERT INTO dbo.alert_rules
        VALUES
        (
            'CPU Critical',
            'cpu_percent',
            'GreaterThan',
            95,
            'Critical',
            300,
            1800,
            1,
            1,
            1,
            GETUTCDATE()
        );
    END;

    IF NOT EXISTS
    (
        SELECT 1
        FROM dbo.alert_rules
        WHERE rule_name = 'Memory Critical'
    )
    BEGIN
        INSERT INTO dbo.alert_rules
        VALUES
        (
            'Memory Critical',
            'memory_percent',
            'GreaterThan',
            95,
            'Critical',
            300,
            1800,
            1,
            1,
            1,
            GETUTCDATE()
        );
    END;

    IF NOT EXISTS
    (
        SELECT 1
        FROM dbo.alert_rules
        WHERE rule_name = 'Gateway Slow'
    )
    BEGIN
        INSERT INTO dbo.alert_rules
        VALUES
        (
            'Gateway Slow',
            'gateway_response_ms',
            'GreaterThan',
            5000,
            'Warning',
            60,
            1800,
            1,
            1,
            1,
            GETUTCDATE()
        );
    END;

    IF NOT EXISTS
    (
        SELECT 1
        FROM dbo.alert_rules
        WHERE rule_name = 'Host Snapshot Stale'
    )
    BEGIN
        INSERT INTO dbo.alert_rules
        VALUES
        (
            'Host Snapshot Stale',
            'host_snapshot_age_seconds',
            'GreaterThan',
            120,
            'Warning',
            0,
            3600,
            1,
            0,
            1,
            GETUTCDATE()
        );
    END;

    IF NOT EXISTS
    (
        SELECT 1
        FROM dbo.alert_rules
        WHERE rule_name = 'Gateway Snapshot Stale'
    )
    BEGIN
        INSERT INTO dbo.alert_rules
        VALUES
        (
            'Gateway Snapshot Stale',
            'gateway_snapshot_age_seconds',
            'GreaterThan',
            120,
            'Warning',
            0,
            3600,
            1,
            0,
            1,
            GETUTCDATE()
        );
    END;

    IF NOT EXISTS
    (
        SELECT 1
        FROM dbo.alert_rules
        WHERE rule_name = 'Ignition Snapshot Stale'
    )
    BEGIN
        INSERT INTO dbo.alert_rules
        VALUES
        (
            'Ignition Snapshot Stale',
            'ignition_snapshot_age_seconds',
            'GreaterThan',
            120,
            'Warning',
            0,
            3600,
            1,
            0,
            1,
            GETUTCDATE()
        );
    END;

    IF NOT EXISTS
    (
        SELECT 1
        FROM dbo.alert_rules
        WHERE rule_name = 'Engine Worker Failed'
    )
    BEGIN
        INSERT INTO dbo.alert_rules
        VALUES
        (
            'Engine Worker Failed',
            'engine_worker_last_success_seconds',
            'GreaterThan',
            120,
            'Critical',
            120,
            3600,
            1,
            0,
            1,
            GETUTCDATE()
        );
    END;

    COMMIT TRANSACTION;
END