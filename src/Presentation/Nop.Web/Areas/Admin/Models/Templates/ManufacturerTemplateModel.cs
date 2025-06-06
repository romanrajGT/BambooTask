﻿using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.Templates;

/// <summary>
/// Represents a manufacturer template model
/// </summary>
public partial record ManufacturerTemplateModel : BaseNopEntityModel
{
    #region Properties

    [NopResourceDisplayName("Admin.System.Templates.Manufacturer.Name")]
    public string Name { get; set; }

    [NopResourceDisplayName("Admin.System.Templates.Manufacturer.ViewPath")]
    public string ViewPath { get; set; }

    [NopResourceDisplayName("Admin.System.Templates.Manufacturer.DisplayOrder")]
    public int DisplayOrder { get; set; }

    #endregion
}