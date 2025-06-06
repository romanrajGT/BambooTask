﻿using FluentMigrator.Builders.Create.Table;
using Nop.Core.Domain.Customers;
using Nop.Data.Extensions;

namespace Nop.Data.Mapping.Builders.Customers;

/// <summary>
/// Represents a reward points history entity builder
/// </summary>
public partial class RewardPointsHistoryBuilder : NopEntityBuilder<RewardPointsHistory>
{
    #region Methods

    /// <summary>
    /// Apply entity configuration
    /// </summary>
    /// <param name="table">Create table expression builder</param>
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(RewardPointsHistory.CustomerId)).AsInt32().ForeignKey<Customer>();
    }

    #endregion
}