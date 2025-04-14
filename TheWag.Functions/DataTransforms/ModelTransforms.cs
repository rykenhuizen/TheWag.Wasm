using TheWag.Models;
using TheWag.Functions.EF;

namespace TheWag.Functions.DataTransforms
{
    internal class ModelTransforms
    {
        public static Order CartToEFOrder(CustomerCart cart, EF.Customer customer)
        {
            //var customer = new TheWag.Functions.Models.Customer() { Email = cart.Customer.Email };
            var orderDetails = new List<OrderDetail>();
            foreach (var item in cart.Items)
            {
                var orderDetail = new OrderDetail
                {
                    FkProductId = item.Product.Id.Value,
                    Qty = item.Quantity,
                };
                orderDetails.Add(orderDetail);
            }

            var order = new Order
            {
                FkCustomer = customer,
                Date = DateTime.UtcNow,
                //Total = cart.Items.Sum(x => x.Product.Price * x.Quantity),
                OrderDetails = orderDetails,
            };

            return order;
        }
    }
}
