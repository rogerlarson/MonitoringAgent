/*
===============================================================================
UtcDateTimeConverter
===============================================================================

Author: Roger Larson
Created: 06/07/2026

Purpose:
Ensures DateTime values loaded from the database
are treated as UTC timestamps.

Responsibilities:
- Apply DateTimeKind.Utc to database values
- Provide consistent timestamp handling
- Prevent timezone ambiguity
- Support Entity Framework date conversions

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

This converter guarantees all DateTime values
are materialized as UTC.

Notes:
Only affects values being read from the database.

For nullable DateTime values see:

    NullableUtcDateTimeConverter

===============================================================================
*/

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MonitoringAgent.Data.Converters;

/// <summary>
/// Converts DateTime values to UTC when
/// materialized from the database.
/// </summary>
public sealed class UtcDateTimeConverter
    : ValueConverter<DateTime, DateTime>
{
    /// <summary>
    /// Creates a new UTC DateTime converter.
    /// </summary>
    public UtcDateTimeConverter()
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
            v => DateTime.SpecifyKind(
                v,
                DateTimeKind.Utc))
    {
    }
}