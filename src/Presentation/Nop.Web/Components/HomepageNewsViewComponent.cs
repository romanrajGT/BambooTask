﻿using Microsoft.AspNetCore.Mvc;
using Nop.Core.Domain.News;
using Nop.Web.Factories;
using Nop.Web.Framework.Components;

namespace Nop.Web.Components;

public partial class HomepageNewsViewComponent : NopViewComponent
{
    protected readonly INewsModelFactory _newsModelFactory;
    protected readonly NewsSettings _newsSettings;

    public HomepageNewsViewComponent(INewsModelFactory newsModelFactory, NewsSettings newsSettings)
    {
        _newsModelFactory = newsModelFactory;
        _newsSettings = newsSettings;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        if (!_newsSettings.Enabled || !_newsSettings.ShowNewsOnMainPage)
            return Content("");

        var model = await _newsModelFactory.PrepareHomepageNewsItemsModelAsync();
        return View(model);
    }
}