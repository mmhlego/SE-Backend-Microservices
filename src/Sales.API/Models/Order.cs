#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations.Schema;
using SharedModels;

namespace Sales.API.Models {
    public class Order {
        public Guid Id { get; set; } = Guid.NewGuid();
        public OrderStates State { get; set; } = OrderStates.Filling;

        public long TotalPrice {
            get {
                long price = 0;

                foreach (var item in OrderItems) {
                    price += item.Amount * item.SalePrice.Price;
                }

                return price;
            }
        }

        public Guid CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }

        public Guid? PurchaseId { get; set; } = null;

        public List<OrderItem> OrderItems { get; set; }
    }
}