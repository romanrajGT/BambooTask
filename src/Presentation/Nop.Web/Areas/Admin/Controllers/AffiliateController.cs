﻿using Microsoft.AspNetCore.Mvc;
using Nop.Core.Domain.Affiliates;
using Nop.Core.Domain.Common;
using Nop.Services.Affiliates;
using Nop.Services.Common;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Web.Areas.Admin.Factories;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Areas.Admin.Models.Affiliates;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Web.Areas.Admin.Controllers;

public partial class AffiliateController : BaseAdminController
{
    #region Fields

    protected readonly IAddressService _addressService;
    protected readonly IAffiliateModelFactory _affiliateModelFactory;
    protected readonly IAffiliateService _affiliateService;
    protected readonly ICustomerActivityService _customerActivityService;
    protected readonly ILocalizationService _localizationService;
    protected readonly INotificationService _notificationService;
    protected readonly IPermissionService _permissionService;

    #endregion

    #region Ctor

    public AffiliateController(IAddressService addressService,
        IAffiliateModelFactory affiliateModelFactory,
        IAffiliateService affiliateService,
        ICustomerActivityService customerActivityService,
        ILocalizationService localizationService,
        INotificationService notificationService,
        IPermissionService permissionService)
    {
        _addressService = addressService;
        _affiliateModelFactory = affiliateModelFactory;
        _affiliateService = affiliateService;
        _customerActivityService = customerActivityService;
        _localizationService = localizationService;
        _notificationService = notificationService;
        _permissionService = permissionService;
    }

    #endregion

    #region Methods

    public virtual IActionResult Index()
    {
        return RedirectToAction("List");
    }

    [CheckPermission(StandardPermission.Promotions.AFFILIATES_VIEW)]
    public virtual async Task<IActionResult> List()
    {
        //prepare model
        var model = await _affiliateModelFactory.PrepareAffiliateSearchModelAsync(new AffiliateSearchModel());

        return View(model);
    }

    [HttpPost]
    [CheckPermission(StandardPermission.Promotions.AFFILIATES_VIEW)]
    public virtual async Task<IActionResult> List(AffiliateSearchModel searchModel)
    {
        //prepare model
        var model = await _affiliateModelFactory.PrepareAffiliateListModelAsync(searchModel);

        return Json(model);
    }

    [CheckPermission(StandardPermission.Promotions.AFFILIATES_CREATE_EDIT_DELETE)]
    public virtual async Task<IActionResult> Create()
    {
        //prepare model
        var model = await _affiliateModelFactory.PrepareAffiliateModelAsync(new AffiliateModel(), null);

        return View(model);
    }

    [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
    [FormValueRequired("save", "save-continue")]
    [CheckPermission(StandardPermission.Promotions.AFFILIATES_CREATE_EDIT_DELETE)]
    public virtual async Task<IActionResult> Create(AffiliateModel model, bool continueEditing)
    {
        if (ModelState.IsValid)
        {
            var address = model.Address.ToEntity<Address>();

            address.CreatedOnUtc = DateTime.UtcNow;

            //some validation
            if (address.CountryId == 0)
                address.CountryId = null;
            if (address.StateProvinceId == 0)
                address.StateProvinceId = null;

            await _addressService.InsertAddressAsync(address);

            var affiliate = model.ToEntity<Affiliate>();

            //validate friendly URL name
            var friendlyUrlName = await _affiliateService.ValidateFriendlyUrlNameAsync(affiliate, model.FriendlyUrlName);
            affiliate.FriendlyUrlName = friendlyUrlName;
            affiliate.AddressId = address.Id;

            await _affiliateService.InsertAffiliateAsync(affiliate);

            //activity log
            await _customerActivityService.InsertActivityAsync("AddNewAffiliate",
                string.Format(await _localizationService.GetResourceAsync("ActivityLog.AddNewAffiliate"), affiliate.Id), affiliate);

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Affiliates.Added"));

            return continueEditing ? RedirectToAction("Edit", new { id = affiliate.Id }) : RedirectToAction("List");
        }

        //prepare model
        model = await _affiliateModelFactory.PrepareAffiliateModelAsync(model, null, true);

        //if we got this far, something failed, redisplay form
        return View(model);
    }

    [CheckPermission(StandardPermission.Promotions.AFFILIATES_VIEW)]
    public virtual async Task<IActionResult> Edit(int id)
    {
        //try to get an affiliate with the specified id
        var affiliate = await _affiliateService.GetAffiliateByIdAsync(id);
        if (affiliate == null || affiliate.Deleted)
            return RedirectToAction("List");

        //prepare model
        var model = await _affiliateModelFactory.PrepareAffiliateModelAsync(null, affiliate);

        return View(model);
    }

    [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
    [CheckPermission(StandardPermission.Promotions.AFFILIATES_CREATE_EDIT_DELETE)]
    public virtual async Task<IActionResult> Edit(AffiliateModel model, bool continueEditing)
    {
        //try to get an affiliate with the specified id
        var affiliate = await _affiliateService.GetAffiliateByIdAsync(model.Id);
        if (affiliate == null || affiliate.Deleted)
            return RedirectToAction("List");

        if (ModelState.IsValid)
        {
            var address = await _addressService.GetAddressByIdAsync(affiliate.AddressId);
            address = model.Address.ToEntity(address);

            //some validation
            if (address.CountryId == 0)
                address.CountryId = null;
            if (address.StateProvinceId == 0)
                address.StateProvinceId = null;

            await _addressService.UpdateAddressAsync(address);

            affiliate = model.ToEntity(affiliate);

            //validate friendly URL name
            var friendlyUrlName = await _affiliateService.ValidateFriendlyUrlNameAsync(affiliate, model.FriendlyUrlName);
            affiliate.FriendlyUrlName = friendlyUrlName;
            affiliate.AddressId = address.Id;

            await _affiliateService.UpdateAffiliateAsync(affiliate);

            //activity log
            await _customerActivityService.InsertActivityAsync("EditAffiliate",
                string.Format(await _localizationService.GetResourceAsync("ActivityLog.EditAffiliate"), affiliate.Id), affiliate);

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Affiliates.Updated"));

            if (!continueEditing)
                return RedirectToAction("List");

            return RedirectToAction("Edit", new { id = affiliate.Id });
        }

        //prepare model
        model = await _affiliateModelFactory.PrepareAffiliateModelAsync(model, affiliate, true);

        //if we got this far, something failed, redisplay form
        return View(model);
    }

    //delete
    [HttpPost]
    [CheckPermission(StandardPermission.Promotions.AFFILIATES_CREATE_EDIT_DELETE)]
    public virtual async Task<IActionResult> Delete(int id)
    {
        //try to get an affiliate with the specified id
        var affiliate = await _affiliateService.GetAffiliateByIdAsync(id);
        if (affiliate == null)
            return RedirectToAction("List");

        await _affiliateService.DeleteAffiliateAsync(affiliate);

        //activity log
        await _customerActivityService.InsertActivityAsync("DeleteAffiliate",
            string.Format(await _localizationService.GetResourceAsync("ActivityLog.DeleteAffiliate"), affiliate.Id), affiliate);

        _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Affiliates.Deleted"));

        return RedirectToAction("List");
    }

    [HttpPost]
    [CheckPermission(StandardPermission.Promotions.AFFILIATES_VIEW)]
    public virtual async Task<IActionResult> AffiliatedOrderListGrid(AffiliatedOrderSearchModel searchModel)
    {
        //try to get an affiliate with the specified id
        var affiliate = await _affiliateService.GetAffiliateByIdAsync(searchModel.AffliateId)
                        ?? throw new ArgumentException("No affiliate found with the specified id");

        //prepare model
        var model = await _affiliateModelFactory.PrepareAffiliatedOrderListModelAsync(searchModel, affiliate);

        return Json(model);
    }

    [HttpPost]
    [CheckPermission(StandardPermission.Promotions.AFFILIATES_VIEW)]
    public virtual async Task<IActionResult> AffiliatedCustomerList(AffiliatedCustomerSearchModel searchModel)
    {
        //try to get an affiliate with the specified id
        var affiliate = await _affiliateService.GetAffiliateByIdAsync(searchModel.AffliateId)
                        ?? throw new ArgumentException("No affiliate found with the specified id");

        //prepare model
        var model = await _affiliateModelFactory.PrepareAffiliatedCustomerListModelAsync(searchModel, affiliate);

        return Json(model);
    }

    #endregion
}