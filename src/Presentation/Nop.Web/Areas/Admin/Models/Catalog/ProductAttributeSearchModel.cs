﻿using Nop.Web.Framework.Models;

namespace Nop.Web.Areas.Admin.Models.Catalog;

/// <summary>
/// Represents a product attribute search model
/// </summary>
public partial record ProductAttributeSearchModel : BaseSearchModel
{
    public string SearchTerm { get; set; }
}