using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sales.API.Models;

namespace Sales.API.Services
{
	public interface ISaleService
	{
		List<Sale> GetSalesByProductId(Guid productId);
		List<Sale> GetSalesByUserId(Guid userId);
		Sale? GetSaleById(Guid id);
		void AddSale(Guid userId, Guid productId, int amount, long initialPrice);
		void UpdateSaleAmount(Guid saleId, int amount);
		void UpdateSalePrice(Guid saleId, long newPrice);
		List<Sale> GetSales();

        List<SalePrice> GetPriceHistoryBySaleId(Guid id);
	}
};