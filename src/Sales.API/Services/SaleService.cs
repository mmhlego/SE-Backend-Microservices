using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sales.API.Data;
using Sales.API.Models;

namespace Sales.API.Services
{
    public class SaleService : ISaleService
    {
        private readonly SalesContext _context;

        public SaleService(SalesContext context)
        {
            _context = context;
        }

        public List<Sale> GetSalesByProductId(Guid productId)
        {
            return _context.Sales.Where(s => s.ProductId == productId).ToList();
        }

        public List<Sale> GetSalesByUserId(Guid userId)
        {
            return _context.Sales.Where(s => s.UserId == userId).ToList();
        }

        public void AddSale(Guid userId, Guid productId, int amount, int initialPrice)
        {
            var sale = new Sale()
            {    
                UserId = userId,
                ProductId = productId,
                Amount =amount
                

            };
            SalePrice salePrice = new SalePrice()
            {
                SaleId = sale.Id,
                Price = initialPrice,
                UpdateDate = DateTime.Now
            };
        

            _context.Sales.Add(sale);
            _context.SalePrices.Add(salePrice);
            _context.SaveChanges();
        }
        public void UpdateSaleAmount(Guid saleId, int amount)
        {
            var sale = _context.Sales.Find(saleId);
            if (sale != null)
            {
                sale.Amount = amount;
                _context.SaveChanges();
            }
        }

        public void UpdateSalePrice(Guid saleId, int newPrice)
        {
            var sale = _context.Sales.Find(saleId);
            if (sale != null)
            {
                SalePrice salePrice = new SalePrice()
                {
                    Price = newPrice,
                    SaleId = sale.Id,
                    UpdateDate = DateTime.Now


                };
                _context.SalePrices.Add(salePrice);
                _context.SaveChanges();
            }
        }
        public Sale? GetSaleById(Guid id)
        {
            return _context.Sales.FirstOrDefault(s => s.Id == id);

        }
    }

}