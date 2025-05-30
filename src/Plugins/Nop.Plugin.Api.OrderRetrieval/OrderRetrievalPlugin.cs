using Nop.Core;
using Nop.Core.Domain.Cms;
using Nop.Services.Plugins;
using System.Threading.Tasks;

namespace Nop.Plugin.Api.OrderRetrieval
{
    public class OrderRetrievalPlugin : BasePlugin
    {
        public override async Task InstallAsync()
        {
            await base.InstallAsync();
        }

        public override async Task UninstallAsync()
        {
            await base.UninstallAsync();
        }
    }
}