﻿using Nop.Core.Domain.Catalog;
using Nop.Services.Caching;

namespace Nop.Services.Catalog.Caching;

/// <summary>
/// Represents a product attribute value picture cache event consumer
/// </summary>
public partial class ProductAttributeValuePictureCacheEventConsumer : CacheEventConsumer<ProductAttributeValuePicture>
{
    /// <summary>
    /// Clear cache data
    /// </summary>
    /// <param name="entity">Entity</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    protected override async Task ClearCacheAsync(ProductAttributeValuePicture entity)
    {
        await RemoveAsync(NopCatalogDefaults.ProductAttributeValuePicturesByValueCacheKey, entity.ProductAttributeValueId);
    }
}