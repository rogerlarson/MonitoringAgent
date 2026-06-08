// Styles
import "./AlertRuleEditor.css";

// React
import {
    useState
} from "react";

// Models
import type {
    AlertRuleResponse
} from "../models/AlertRuleResponse";

/**
 * Available metrics that can be monitored
 * by alert rules.
 */
const ALERT_METRICS = [
    "host_snapshot_age_seconds",
    "gateway_snapshot_age_seconds",
    "ignition_snapshot_age_seconds",
    "cpu_percent",
    "memory_percent",
    "disk_percent",
    "gateway_response_ms"
];

type Props = {
    rule?: AlertRuleResponse;

    onSave: (
        rule: AlertRuleResponse
    ) => void;

    onClose: () => void;
};

/**
 * Alert Rule Editor
 *
 * Modal dialog used to:
 * - Create new alert rules
 * - Edit existing alert rules
 *
 * Allows configuration of:
 * - Metric
 * - Threshold
 * - Severity
 * - Notification settings
 * - Auto-close behavior
 */
export default function AlertRuleEditor({
    rule,
    onSave,
    onClose
}: Props) {

    // -------------------------------------------------------------------------
    // State
    // -------------------------------------------------------------------------

    const [form, setForm] =
        useState<AlertRuleResponse>(
            rule ?? {
                alertRuleId: 0,
                ruleName: "",
                metricName: "",
                operator: "GreaterThan",
                threshold: 0,
                severity: "Warning",
                enabled: true,
                emailEnabled: false,
                autoClose: true,
                sustainSeconds: 300,
                repeatSeconds: 3600
            }
        );

    // -------------------------------------------------------------------------
    // Helpers
    // -------------------------------------------------------------------------

    const updateField = <
        K extends keyof AlertRuleResponse
    >(
        field: K,
        value: AlertRuleResponse[K]
    ) => {

        setForm({
            ...form,
            [field]: value
        });
    };

    // -------------------------------------------------------------------------
    // Render
    // -------------------------------------------------------------------------

    return (

        <div className="alert-rule-editor-overlay">

            <div className="alert-rule-editor">

                <h2>
                    {
                        form.alertRuleId > 0
                            ? "Edit Alert Rule"
                            : "New Alert Rule"
                    }
                </h2>

                {/* ---------------------------------------------------------
                    Rule Name
                   --------------------------------------------------------- */}

                <div className="alert-rule-field">

                    <label>
                        Rule Name
                    </label>

                    <input
                        value={form.ruleName}
                        onChange={e =>
                            updateField(
                                "ruleName",
                                e.target.value
                            )
                        }
                    />

                </div>

                {/* ---------------------------------------------------------
                    Metric
                   --------------------------------------------------------- */}

                <div className="alert-rule-field">

                    <label>
                        Metric
                    </label>

                    <select
                        value={form.metricName}
                        onChange={e =>
                            updateField(
                                "metricName",
                                e.target.value
                            )
                        }
                    >

                        {ALERT_METRICS.map(metric => (

                            <option
                                key={metric}
                                value={metric}
                            >
                                {metric}
                            </option>

                        ))}

                    </select>

                </div>

                {/* ---------------------------------------------------------
                    Operator
                   --------------------------------------------------------- */}

                <div className="alert-rule-field">

                    <label>
                        Operator
                    </label>

                    <select
                        value={form.operator}
                        onChange={e =>
                            updateField(
                                "operator",
                                e.target.value
                            )
                        }
                    >

                        <option value="Equal">
                            Equal
                        </option>

                        <option value="GreaterThan">
                            Greater Than
                        </option>

                        <option value="LessThan">
                            Less Than
                        </option>

                    </select>

                </div>

                {/* ---------------------------------------------------------
                    Threshold
                   --------------------------------------------------------- */}

                <div className="alert-rule-field">

                    <label>
                        Threshold
                    </label>

                    <input
                        type="number"
                        value={form.threshold}
                        onChange={e =>
                            updateField(
                                "threshold",
                                Number(
                                    e.target.value
                                )
                            )
                        }
                    />

                </div>

                {/* ---------------------------------------------------------
                    Severity
                   --------------------------------------------------------- */}

                <div className="alert-rule-field">

                    <label>
                        Severity
                    </label>

                    <select
                        value={form.severity}
                        onChange={e =>
                            updateField(
                                "severity",
                                e.target.value
                            )
                        }
                    >

                        <option value="Info">
                            Info
                        </option>

                        <option value="Warning">
                            Warning
                        </option>

                        <option value="Critical">
                            Critical
                        </option>

                    </select>

                </div>

                {/* ---------------------------------------------------------
                    Sustain Seconds
                   --------------------------------------------------------- */}

                <div className="alert-rule-field">

                    <label>
                        Sustain Seconds
                    </label>

                    <input
                        type="number"
                        value={form.sustainSeconds}
                        onChange={e =>
                            updateField(
                                "sustainSeconds",
                                Number(
                                    e.target.value
                                )
                            )
                        }
                    />

                </div>

                {/* ---------------------------------------------------------
                    Repeat Seconds
                   --------------------------------------------------------- */}

                <div className="alert-rule-field">

                    <label>
                        Repeat Seconds
                    </label>

                    <input
                        type="number"
                        value={form.repeatSeconds}
                        onChange={e =>
                            updateField(
                                "repeatSeconds",
                                Number(
                                    e.target.value
                                )
                            )
                        }
                    />

                </div>

                {/* ---------------------------------------------------------
                    Enabled
                   --------------------------------------------------------- */}

                <div className="alert-rule-checkbox">

                    <label>

                        <input
                            type="checkbox"
                            checked={form.enabled}
                            onChange={e =>
                                updateField(
                                    "enabled",
                                    e.target.checked
                                )
                            }
                        />

                        Enabled

                    </label>

                </div>

                {/* ---------------------------------------------------------
                    Email Notifications
                   --------------------------------------------------------- */}

                <div className="alert-rule-checkbox">

                    <label>

                        <input
                            type="checkbox"
                            checked={form.emailEnabled}
                            onChange={e =>
                                updateField(
                                    "emailEnabled",
                                    e.target.checked
                                )
                            }
                        />

                        Send Email

                    </label>

                </div>

                {/* ---------------------------------------------------------
                    Auto Close
                   --------------------------------------------------------- */}

                <div className="alert-rule-checkbox">

                    <label>

                        <input
                            type="checkbox"
                            checked={form.autoClose}
                            onChange={e =>
                                updateField(
                                    "autoClose",
                                    e.target.checked
                                )
                            }
                        />

                        Auto Close

                    </label>

                </div>

                {/* ---------------------------------------------------------
                    Actions
                   --------------------------------------------------------- */}

                <div className="alert-rule-actions">

                    <button
                        onClick={() =>
                            onSave(form)
                        }
                    >
                        Save
                    </button>

                    <button
                        onClick={onClose}
                    >
                        Cancel
                    </button>

                </div>

            </div>

        </div>
    );
}