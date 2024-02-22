using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geradovana.ScrapingService.Infrastructure.Interfaces
{
    public interface IReadAllPagesStrategy
    {
        Task<Product[]> ReadAllPages(string requestUrl, int pageCount);
    }
}