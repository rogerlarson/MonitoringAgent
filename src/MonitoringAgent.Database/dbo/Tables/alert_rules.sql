CREATE TABLE [dbo].[alert_rules] (
    [alert_rule_id]    INT             IDENTITY (1, 1) NOT NULL,
    [rule_name]        NVARCHAR (100)  NOT NULL,
    [metric_name]      NVARCHAR (100)  NOT NULL,
    [operator]         NVARCHAR (20)   NOT NULL,
    [threshold_value]  DECIMAL (18, 2) NULL,
    [severity]         NVARCHAR (20)   NOT NULL,
    [sustain_seconds]  INT             CONSTRAINT [DF__alert_rul__susta__02084FDA] DEFAULT ((0)) NOT NULL,
    [repeat_seconds]   INT             CONSTRAINT [DF__alert_rul__repea__02FC7413] DEFAULT ((3600)) NOT NULL,
    [auto_close]       BIT             CONSTRAINT [DF__alert_rul__auto___03F0984C] DEFAULT ((1)) NOT NULL,
    [email_enabled]    BIT             CONSTRAINT [DF_alert_rules_send_email] DEFAULT ((0)) NOT NULL,
    [is_enabled]       BIT             CONSTRAINT [DF__alert_rul__is_en__7B5B524B] DEFAULT ((1)) NOT NULL,
    [created_date_utc] DATETIME2 (7)   CONSTRAINT [DF__alert_rul__creat__7C4F7684] DEFAULT (sysutcdatetime()) NOT NULL,
    CONSTRAINT [PK__alert_ru__27F94DF302E28D24] PRIMARY KEY CLUSTERED ([alert_rule_id] ASC),
    CONSTRAINT [ck_alert_rules_operator] CHECK ([operator]='NotEqual' OR [operator]='Equal' OR [operator]='LessThanOrEqual' OR [operator]='LessThan' OR [operator]='GreaterThanOrEqual' OR [operator]='GreaterThan'),
    CONSTRAINT [ck_alert_rules_severity] CHECK ([severity]='Critical' OR [severity]='Warning' OR [severity]='Information')
);

