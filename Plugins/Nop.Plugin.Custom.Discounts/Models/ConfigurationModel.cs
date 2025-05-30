using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Custom.Discounts.Models;

public record ConfigurationModel
{
    public int DiscountId { get; set; }
    public int RequirementId { get; set; }
}