using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Nop.Core;
using Nop.Core.Domain.Orders;
using Nop.Services.Configuration;
using Nop.Services.Customers;
using Nop.Services.Discounts;
using Nop.Services.Localization;
using Nop.Services.Orders;
using Nop.Services.Plugins;

namespace Nop.Plugin.Custom.Discounts;

public partial class CustomDiscountsRequirementRule : BasePlugin, IDiscountRequirementRule
{
    #region Fields
    private readonly IActionContextAccessor _actionContextAccessor;
    private readonly ICustomerService _customerService;
    private readonly IDiscountService _discountService;
    private readonly ILocalizationService _localizationService;
    private readonly IOrderService _orderService;
    private readonly ISettingService _settingService;
    private readonly IUrlHelperFactory _urlHelperFactory;
    private readonly IWebHelper _webHelper;
    #endregion

    #region Ctor
    public CustomDiscountsRequirementRule(IActionContextAccessor actionContextAccessor,
        ICustomerService customerService,
        IDiscountService discountService,
        ILocalizationService localizationService,
        IOrderService orderService,
        ISettingService settingService,
        IUrlHelperFactory urlHelperFactory,
        IWebHelper webHelper)
    {
        _actionContextAccessor = actionContextAccessor;
        _customerService = customerService;
        _discountService = discountService;
        _localizationService = localizationService;
        _orderService = orderService;
        _settingService = settingService;
        _urlHelperFactory = urlHelperFactory;
        _webHelper = webHelper;
    }
    #endregion

    #region Methods
    /// <summary>
    /// Check discount requirement
    /// </summary>
    public async Task<DiscountRequirementValidationResult> CheckRequirementAsync(DiscountRequirementValidationRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        //invalid by default
        var result = new DiscountRequirementValidationResult();

        if (request.Customer == null || await _customerService.IsGuestAsync(request.Customer))
            return result;

        var orders = await _orderService.SearchOrdersAsync(request.Store.Id,
            customerId: request.Customer.Id,
            osIds: new List<int> { (int)OrderStatus.Complete });
        var noOfOrders = orders.Count();
        if (noOfOrders >= 3)
        {
            result.IsValid = true;
        }

        return result;
    }

    /// <summary>
    /// Get URL for rule configuration
    /// </summary>
    public string GetConfigurationUrl(int discountId, int? discountRequirementId)
    {
        var urlHelper = _urlHelperFactory.GetUrlHelper(_actionContextAccessor.ActionContext);

        return urlHelper.Action("Configure", "CustomDiscounts",
            new { discountId, discountRequirementId }, _webHelper.GetCurrentRequestProtocol());
    }

    /// <summary>
    /// Install the plugin
    /// </summary>
    public override async Task InstallAsync()
    {
        await base.InstallAsync();
    }

    /// <summary>
    /// Uninstall the plugin
    /// </summary>
    public override async Task UninstallAsync()
    {
        //discount requirements
        var discountRequirements = (await _discountService.GetAllDiscountRequirementsAsync())
            .Where(discountRequirement => discountRequirement.DiscountRequirementRuleSystemName == DiscountRequirementDefaults.SYSTEM_NAME);

        foreach (var discountRequirement in discountRequirements)
            await _discountService.DeleteDiscountRequirementAsync(discountRequirement, false);

        //locales
        await _localizationService.DeleteLocaleResourcesAsync("Plugins.Custom.Discounts");

        await base.UninstallAsync();
    }

    #endregion
}