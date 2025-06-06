﻿using Microsoft.AspNetCore.Mvc;
using Nop.Core.Domain.Vendors;
using Nop.Web.Factories;
using Nop.Web.Framework.Components;

namespace Nop.Web.Components;

public partial class VendorNavigationViewComponent : NopViewComponent
{
    protected readonly ICatalogModelFactory _catalogModelFactory;
    protected readonly VendorSettings _vendorSettings;

    public VendorNavigationViewComponent(ICatalogModelFactory catalogModelFactory,
        VendorSettings vendorSettings)
    {
        _catalogModelFactory = catalogModelFactory;
        _vendorSettings = vendorSettings;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        if (_vendorSettings.VendorsBlockItemsToDisplay == 0)
            return Content("");

        var model = await _catalogModelFactory.PrepareVendorNavigationModelAsync();
        if (!model.Vendors.Any())
            return Content("");

        return View(model);
    }
}