/*
===============================================================================
NullableUtcDateTimeConverter
===============================================================================

Author: Roger Larson
Created: 06/07/2026

Purpose:
Ensures nullable DateTime values loaded from the database
are treated as UTC timestamps.

Responsibilities:
- Preserve null DateTime values
- Apply DateTimeKind.Utc when reading values
- Provide consistent timestamp handling
- Prevent timezone ambiguity

Used By:
- Entity Framework Core
- MonitoringDbContext

Problem Solved:
SQL Server does not preserve DateTimeKind information.

Without this converter:

    2026-06-07 12:00:00

may be interpreted as:

    Local
    Unspecified
    UTC

depending on usage.

This converter guarantees all nullable DateTime values
are materialized as UTC.

Notes:
Only affects values being read from the database.

Null values remain null.

===============================================================================
*/

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MonitoringAgent.Data.Converters;

/// <summary>
/// Converts nullable DateTime values to UTC
/// when materialized from the database.
/// </summary>
public sealed class NullableUtcDateTimeConverter
    : ValueConverter<DateTime?, DateTime?>
{
    /// <summary>
    /// Creates a new UTC DateTime converter.
    /// </summary>
    public NullableUtcDateTimeConverter()
        : base(

            // -------------------------------------------------------------
            // Write to Database
            // -------------------------------------------------------------
            //
            // Store the value as-is.
            //
            v => v,

            // -------------------------------------------------------------
            // Read from Database
            // -------------------------------------------------------------
            //
            // Restore UTC DateTimeKind.
            //
            v =>
                v == null
                    ? null
                    : DateTime.SpecifyKind(
                        v.Value,
                        DateTimeKind.Utc))
    {
    }
}