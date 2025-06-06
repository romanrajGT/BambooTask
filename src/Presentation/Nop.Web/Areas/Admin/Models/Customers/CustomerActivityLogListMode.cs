﻿using Nop.Web.Framework.Models;

namespace Nop.Web.Areas.Admin.Models.Customers;

/// <summary>
/// Represents a customer activity log list model
/// </summary>
public partial record CustomerActivityLogListModel : BasePagedListModel<CustomerActivityLogModel>
{
}