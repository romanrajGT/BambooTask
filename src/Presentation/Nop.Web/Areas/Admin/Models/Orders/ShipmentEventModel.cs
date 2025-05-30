﻿using Nop.Web.Framework.Models;

namespace Nop.Web.Areas.Admin.Models.Orders;

/// <summary>
/// Represents a shipment event model
/// </summary>
public partial record ShipmentStatusEventModel : BaseNopModel
{
    #region Properties

    public string Status { get; set; }

    public string EventName { get; set; }

    public string Location { get; set; }

    public string Country { get; set; }

    public DateTime? Date { get; set; }

    #endregion
}