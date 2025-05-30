﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Plugin.Shipping.UPS.Domain;
using Nop.Plugin.Shipping.UPS.Models;
using Nop.Plugin.Shipping.UPS.Services;
using Nop.Services;
using Nop.Services.Configuration;
using Nop.Services.Directory;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Plugin.Shipping.UPS.Controllers;

[AuthorizeAdmin]
[Area(AreaNames.ADMIN)]
[AutoValidateAntiforgeryToken]
public class UPSShippingController : BasePluginController
{
    #region Fields

    private readonly ILocalizationService _localizationService;
    private readonly IMeasureService _measureService;
    private readonly INotificationService _notificationService;
    private readonly IPermissionService _permissionService;
    private readonly ISettingService _settingService;
    private readonly UPSService _upsService;
    private readonly UPSSettings _upsSettings;

    #endregion

    #region Ctor

    public UPSShippingController(ILocalizationService localizationService,
        IMeasureService measureService,
        INotificationService notificationService,
        IPermissionService permissionService,
        ISettingService settingService,
        UPSService upsService,
        UPSSettings upsSettings)
    {
        _localizationService = localizationService;
        _measureService = measureService;
        _notificationService = notificationService;
        _permissionService = permissionService;
        _settingService = settingService;
        _upsService = upsService;
        _upsSettings = upsSettings;
    }

    #endregion

    #region Methods

    [CheckPermission(StandardPermission.Configuration.MANAGE_SHIPPING_SETTINGS)]
    public async Task<IActionResult> Configure()
    {
        //prepare common model
        var model = new UPSShippingModel
        {
            AccountNumber = _upsSettings.AccountNumber,
            ClientId = _upsSettings.ClientId,
            ClientSecret = _upsSettings.ClientSecret,
            UseSandbox = _upsSettings.UseSandbox,
            AdditionalHandlingCharge = _upsSettings.AdditionalHandlingCharge,
            InsurePackage = _upsSettings.InsurePackage,
            CustomerClassification = (int)_upsSettings.CustomerClassification,
            PickupType = (int)_upsSettings.PickupType,
            PackagingType = (int)_upsSettings.PackagingType,
            SaturdayDeliveryEnabled = _upsSettings.SaturdayDeliveryEnabled,
            PassDimensions = _upsSettings.PassDimensions,
            PackingPackageVolume = _upsSettings.PackingPackageVolume,
            PackingType = (int)_upsSettings.PackingType,
            Tracing = _upsSettings.Tracing,
            WeightType = _upsSettings.WeightType,
            DimensionsType = _upsSettings.DimensionsType
        };

        //prepare offered delivery services
        var servicesCodes = _upsSettings.CarrierServicesOffered?.Split(':', StringSplitOptions.RemoveEmptyEntries)
            .Select(idValue => idValue.Trim('[', ']')).ToList() ?? new List<string>();

        //prepare available options
        model.AvailableCustomerClassifications = (await CustomerClassification.DailyRates.ToSelectListAsync(false))
            .Select(item => new SelectListItem(item.Text, item.Value)).ToList();
        model.AvailablePickupTypes = (await PickupType.DailyPickup.ToSelectListAsync(false))
            .Select(item => new SelectListItem(item.Text, item.Value)).ToList();
        model.AvailablePackagingTypes = (await PackagingType.CustomerSuppliedPackage.ToSelectListAsync(false))
            .Select(item => new SelectListItem(item.Text?.TrimStart('_'), item.Value)).ToList();
        model.AvaliablePackingTypes = (await PackingType.PackByDimensions.ToSelectListAsync(false))
            .Select(item => new SelectListItem(item.Text, item.Value)).ToList();
        model.AvailableCarrierServices = (await DeliveryService.Standard.ToSelectListAsync(false))
            .Select(item =>
            {
                var serviceCode = _upsService.GetUpsCode((DeliveryService)int.Parse(item.Value));
                return new SelectListItem($"UPS {item.Text?.TrimStart('_')}", serviceCode, servicesCodes.Contains(serviceCode));
            }).ToList();
        model.AvaliableWeightTypes = new List<SelectListItem> { new SelectListItem("LBS", "LBS"), new SelectListItem("KGS", "KGS") };
        model.AvaliableDimensionsTypes = new List<SelectListItem> { new SelectListItem("IN", "IN"), new SelectListItem("CM", "CM") };

        //check measures
        var weightSystemName = _upsSettings.WeightType switch { "LBS" => "lb", "KGS" => "kg", _ => null };
        if (await _measureService.GetMeasureWeightBySystemKeywordAsync(weightSystemName) == null)
            _notificationService.ErrorNotification($"Could not load '{weightSystemName}' <a href=\"{Url.Action("List", "Measure")}\" target=\"_blank\">measure weight</a>", false);

        var dimensionSystemName = _upsSettings.DimensionsType switch { "IN" => "inches", "CM" => "centimeters", _ => null };
        if (await _measureService.GetMeasureDimensionBySystemKeywordAsync(dimensionSystemName) == null)
            _notificationService.ErrorNotification($"Could not load '{dimensionSystemName}' <a href=\"{Url.Action("List", "Measure")}\" target=\"_blank\">measure dimension</a>", false);

        return View("~/Plugins/Shipping.UPS/Views/Configure.cshtml", model);
    }

    [HttpPost]
    [CheckPermission(StandardPermission.Configuration.MANAGE_SHIPPING_SETTINGS)]
    public async Task<IActionResult> Configure(UPSShippingModel model)
    {
        if (!ModelState.IsValid)
            return await Configure();

        //save settings
        _upsSettings.AccountNumber = model.AccountNumber;
        _upsSettings.ClientId = model.ClientId;
        _upsSettings.ClientSecret = model.ClientSecret;
        _upsSettings.UseSandbox = model.UseSandbox;
        _upsSettings.AdditionalHandlingCharge = model.AdditionalHandlingCharge;
        _upsSettings.CustomerClassification = (CustomerClassification)model.CustomerClassification;
        _upsSettings.PickupType = (PickupType)model.PickupType;
        _upsSettings.PackagingType = (PackagingType)model.PackagingType;
        _upsSettings.InsurePackage = model.InsurePackage;
        _upsSettings.SaturdayDeliveryEnabled = model.SaturdayDeliveryEnabled;
        _upsSettings.PassDimensions = model.PassDimensions;
        _upsSettings.PackingPackageVolume = model.PackingPackageVolume;
        _upsSettings.PackingType = (PackingType)model.PackingType;
        _upsSettings.Tracing = model.Tracing;
        _upsSettings.WeightType = model.WeightType;
        _upsSettings.DimensionsType = model.DimensionsType;

        //use default services if no one is selected 
        if (!model.CarrierServices.Any())
        {
            model.CarrierServices = new List<string>
            {
                _upsService.GetUpsCode(DeliveryService.Ground),
                _upsService.GetUpsCode(DeliveryService.WorldwideExpedited),
                _upsService.GetUpsCode(DeliveryService.Standard),
                _upsService.GetUpsCode(DeliveryService._3DaySelect)
            };
        }
        _upsSettings.CarrierServicesOffered = string.Join(':', model.CarrierServices.Select(service => $"[{service}]"));

        await _settingService.SaveSettingAsync(_upsSettings);

        _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Plugins.Saved"));

        return await Configure();
    }

    #endregion
}