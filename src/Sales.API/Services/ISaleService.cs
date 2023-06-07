using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sales.API.Models;

namespace Sales.API.Services {
    public interface ISaleService
    {
        List<Sale> GetSalesByProductId(Guid productId);
        List<Sale> GetSalesBySellerId(Guid sellerId);
        void AddSale(Guid sellerId, Guid productId, int amount, int initialPrice);
        void UpdateSaleAmount(Guid saleId, int amount);
        void UpdateSalePrice(Guid saleId, int newPrice);
    }
}