﻿using Microsoft.AspNetCore.Mvc;
using Nop.Web.Factories;
using Nop.Web.Framework.Components;

namespace Nop.Web.Components;

public partial class TopMenuViewComponent : NopViewComponent
{
    protected readonly ICatalogModelFactory _catalogModelFactory;

    public TopMenuViewComponent(ICatalogModelFactory catalogModelFactory)
    {
        _catalogModelFactory = catalogModelFactory;
    }

    public async Task<IViewComponentResult> InvokeAsync(int? productThumbPictureSize)
    {
        var model = await _catalogModelFactory.PrepareTopMenuModelAsync();
        return View(model);
    }
}