using System;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.Data.Items;
using Sitecore.DependencyInjection;
using Sitecore.Diagnostics;
using Sitecore.Pipelines.ResolveRenderingDatasource;
using Sitecore.XA.Foundation.Abstractions;
using Sitecore.XA.Foundation.LocalDatasources.Services;
using Sitecore.XA.Foundation.Multisite.Extensions;
using Sitecore.XA.Foundation.SitecoreExtensions.Extensions;

namespace Sitecore.Support.XA.Foundation.LocalDatasources.Pipelines.ResolveRenderingDatasource
{
    public class PageRelativeDatasource
    {
        public void Process(ResolveRenderingDatasourceArgs args)
        {
            Assert.IsNotNull(args, "args");

            if (args.Datasource.StartsWith(Sitecore.XA.Foundation.LocalDatasources.Constants.PageRelativePrefix, StringComparison.InvariantCultureIgnoreCase) || args.Datasource.StartsWith(Sitecore.XA.Foundation.LocalDatasources.Constants.LocalPrefix, StringComparison.InvariantCultureIgnoreCase))
            {
                Item contextItem = args.GetContextItem();
                if (contextItem != null)
                {
                    if (!ServiceLocator.ServiceProvider.GetService<IContext>().Site.IsSxaSite())
                    {
                        return;
                    }

                    args.Datasource = Sitecore.DependencyInjection.ServiceLocator.ServiceProvider.GetService<ILocalDatasourceService>().ExpandPageRelativePath(args.Datasource, contextItem.Paths.FullPath);
                    return;
                }

                args.Datasource = string.Empty;
            }
        }
    }
}