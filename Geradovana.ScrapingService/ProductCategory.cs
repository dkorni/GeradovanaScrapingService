using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geradovana.ScrapingService.Domain
{
    public record ProductCategory(
        string name, 
        string[]? subcategories);
}