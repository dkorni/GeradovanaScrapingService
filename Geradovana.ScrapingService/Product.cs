using Geradovana.ScrapingService.Domain;
using Geradovana.ScrapingService.Domain.Enums;

namespace Geradovana.ScrapingService
{
    public record Product(
        string ProductName,
        decimal Price,
        ProductType ProductType
        );
}
