﻿using FluentValidation;
using Nop.Core.Domain.Catalog;
using Nop.Services.Localization;
using Nop.Web.Areas.Admin.Models.Catalog;
using Nop.Web.Framework.Validators;

namespace Nop.Web.Areas.Admin.Validators.Catalog;

public partial class ProductTagValidator : BaseNopValidator<ProductTagModel>
{
    public ProductTagValidator(ILocalizationService localizationService)
    {
        RuleFor(x => x.Name).NotEmpty().WithMessageAwait(localizationService.GetResourceAsync("Admin.Catalog.ProductTags.Fields.Name.Required"));

        SetDatabaseValidationRules<ProductTag>();
    }
}