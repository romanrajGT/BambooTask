﻿using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.Stores;

/// <summary>
/// Represents a store model
/// </summary>
public partial record StoreModel : BaseNopEntityModel, ILocalizedModel<StoreLocalizedModel>
{
    #region Ctor

    public StoreModel()
    {
        Locales = new List<StoreLocalizedModel>();
        AvailableLanguages = new List<SelectListItem>();
    }

    #endregion

    #region Properties

    [NopResourceDisplayName("Admin.Configuration.Stores.Fields.Name")]
    public string Name { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Stores.Fields.Url")]
    public string Url { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Stores.Fields.SslEnabled")]
    public virtual bool SslEnabled { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Stores.Fields.Hosts")]
    public string Hosts { get; set; }

    //default language
    [NopResourceDisplayName("Admin.Configuration.Stores.Fields.DefaultLanguage")]
    public int DefaultLanguageId { get; set; }

    public IList<SelectListItem> AvailableLanguages { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Stores.Fields.DisplayOrder")]
    public int DisplayOrder { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Stores.Fields.CompanyName")]
    public string CompanyName { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Stores.Fields.CompanyAddress")]
    public string CompanyAddress { get; set; }

    [DataType(DataType.PhoneNumber)]
    [NopResourceDisplayName("Admin.Configuration.Stores.Fields.CompanyPhoneNumber")]
    public string CompanyPhoneNumber { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Stores.Fields.CompanyVat")]
    public string CompanyVat { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Stores.Fields.DefaultMetaKeywords")]
    public string DefaultMetaKeywords { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Stores.Fields.DefaultMetaDescription")]
    public string DefaultMetaDescription { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Stores.Fields.DefaultTitle")]
    public string DefaultTitle { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Stores.Fields.HomepageTitle")]
    public string HomepageTitle { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Stores.Fields.HomepageDescription")]
    public string HomepageDescription { get; set; }

    public IList<StoreLocalizedModel> Locales { get; set; }

    #endregion
}

public partial record StoreLocalizedModel : ILocalizedLocaleModel
{
    public int LanguageId { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Stores.Fields.Name")]
    public string Name { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Stores.Fields.DefaultMetaKeywords")]
    public string DefaultMetaKeywords { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Stores.Fields.DefaultMetaDescription")]
    public string DefaultMetaDescription { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Stores.Fields.DefaultTitle")]
    public string DefaultTitle { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Stores.Fields.HomepageTitle")]
    public string HomepageTitle { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Stores.Fields.HomepageDescription")]
    public string HomepageDescription { get; set; }
}