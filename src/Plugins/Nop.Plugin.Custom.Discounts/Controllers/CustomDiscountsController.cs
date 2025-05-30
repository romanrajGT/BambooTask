using Microsoft.AspNetCore.Mvc;
using Nop.Core.Domain.Discounts;
using Nop.Plugin.Custom.Discounts.Models;
using Nop.Services.Configuration;
using Nop.Services.Discounts;
using Nop.Services.Security;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Plugin.Custom.Discounts.Controllers;

[AuthorizeAdmin]
[Area(AreaNames.ADMIN)]
[AutoValidateAntiforgeryToken]
public class CustomDiscountsController : BasePluginController
{
    #region Fields

    private readonly IDiscountService _discountService;
    private readonly ISettingService _settingService;

    #endregion

    #region Ctor

    public CustomDiscountsController(IDiscountService discountService,
        ISettingService settingService)
    {
        _discountService = discountService;
        _settingService = settingService;
    }

    #endregion

    #region Utilities

    private IEnumerable<string> GetErrorsFromModelState()
    {
        return ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
    }

    #endregion

    #region Methods

    [CheckPermission(StandardPermission.Promotions.DISCOUNTS_VIEW)]
    public async Task<IActionResult> Configure(int discountId, int? discountRequirementId)
    {
        var discount = await _discountService.GetDiscountByIdAsync(discountId) ?? throw new ArgumentException("Discount could not be loaded");

        //check whether the discount requirement exists
        if (discountRequirementId.HasValue && await _discountService.GetDiscountRequirementByIdAsync(discountRequirementId.Value) is null)
            return Content("Failed to load requirement.");

        var model = new ConfigurationModel
        {
            RequirementId = discountRequirementId ?? 0,
            DiscountId = discount.Id
        };
        
        return View("~/Plugins/Custom.Discounts/Views/Configure.cshtml", model);
    }

    [HttpPost]
    [CheckPermission(StandardPermission.Promotions.DISCOUNTS_CREATE_EDIT_DELETE)]
    public async Task<IActionResult> Configure(ConfigurationModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(new { Errors = GetErrorsFromModelState() });

        //load the discount
        var discount = await _discountService.GetDiscountByIdAsync(model.DiscountId);
        if (discount == null)
            return NotFound(new { Errors = new[] { "Discount could not be loaded" } });

        //get the discount requirement
        var discountRequirement = await _discountService.GetDiscountRequirementByIdAsync(model.RequirementId);

        //the discount requirement does not exist, so create a new one
        if (discountRequirement == null)
        {
            discountRequirement = new DiscountRequirement
            {
                DiscountId = discount.Id,
                DiscountRequirementRuleSystemName = DiscountRequirementDefaults.SYSTEM_NAME
            };

            await _discountService.InsertDiscountRequirementAsync(discountRequirement);
        }
        
        return Ok(new { NewRequirementId = discountRequirement.Id });
    }

    #endregion
}