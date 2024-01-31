using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geradovana.ScrapingService.Infrastructure.Interfaces
{
    internal interface IReadAllPagesStrategy
    {
        Task<Product[]> ReadAllPages(string requestUrl, int pageCount);
    }
}