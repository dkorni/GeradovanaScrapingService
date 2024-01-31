using Geradovana.ScrapingService.Application.Common.Providers;
using Geradovana.ScrapingService.Infrastructure.Providers.Scrapers;
using Geradovana.ScrapingService.Infrastructure.Providers.Scrapers.ScrapperStrategies.ReadAllPages;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geradovana.ScrapingService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            var productCategoryProvider = new ProductCategoryProvider(new ParallelReadAllPagesStrategy());
            services.AddSingleton<IProductCategoryProvider>(productCategoryProvider);

            return services;
        }
    }
}