export interface AlertRuleResponse
{
    alertRuleId: number;

    ruleName: string;

    metricName: string;

    operator: string;

    threshold: number;

    severity: string;

    sustainSeconds: number;

    repeatSeconds: number;

    autoClose: boolean;

    emailEnabled: boolean;

    enabled: boolean;
}