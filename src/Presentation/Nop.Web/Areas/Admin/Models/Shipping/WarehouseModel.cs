﻿using Nop.Web.Areas.Admin.Models.Common;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.Shipping;

/// <summary>
/// Represents a warehouse model
/// </summary>
public partial record WarehouseModel : BaseNopEntityModel
{
    #region Ctor

    public WarehouseModel()
    {
        Address = new AddressModel();
    }

    #endregion

    #region Properties

    [NopResourceDisplayName("Admin.Configuration.Shipping.Warehouses.Fields.Name")]
    public string Name { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Shipping.Warehouses.Fields.AdminComment")]
    public string AdminComment { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Shipping.Warehouses.Fields.Address")]
    public AddressModel Address { get; set; }

    #endregion
}