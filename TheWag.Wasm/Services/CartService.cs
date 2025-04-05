using TheWag.Models;
using TheWag.Wasm.Util;

namespace TheWag.Wasm.Services
{
    public class CartService
    {
        private readonly SessionStorage _session;
        private readonly AppSettings _appSettings;
        private readonly ILogger<CartService> _logger;
        public CustomerCart Cart { get; private set; }

        public CartService(SessionStorage sessionStorageAccessor, AppSettings appSettings, ILogger<CartService> logger)
        {
            _session = sessionStorageAccessor;
            _appSettings = appSettings;
            _logger = logger;
            Cart = new CustomerCart();
            SetCartFromSessioinAsync();

        }

        public void DecrementItem(ProductDTO product)
        {
            var item = Cart.Items.FirstOrDefault(x => x.Product == product);
            if (item != null)
            {
                if (item.Quantity > 1)
                {
                    item.Quantity--;
                }
                else
                {
                    Cart.Items.Remove(item);
                }
                SaveCartToSessionAsync();
            }
        }

        public void IncrementItem(ProductDTO product)
        {
            var item = Cart.Items.FirstOrDefault(x => x.Product.Id == product.Id);
            if (item == null)
            {
                Cart.Items.Add(new CartItem() { Product = product, Quantity = 1 });
            }
            else
            {
                item.Quantity++;
            }
            SaveCartToSessionAsync();
        }

        private async void SetCartFromSessioinAsync()
        {
            var cart = await _session.GetValueAsync<CustomerCart>(_appSettings.CartSessionKey);
            if (cart == null)
            {
                SaveCartToSessionAsync();
            }
            else
            {
                Cart = cart;
            }
        }

        private async void SaveCartToSessionAsync()
        {
            await _session.SetValueAsync(_appSettings.CartSessionKey, Cart);
        }

        public async void ClearCartAsync()
        {
            await _session.SetValueAsync(_appSettings.CartSessionKey, new CustomerCart());
        }

        public int GetItemCount(int productid)
        {
            return Cart.Items.FirstOrDefault(x => x.Product.Id == productid)?.Quantity ?? 0;
        }

        public decimal GetTotalPrice()
        {
            return Cart.Items.Sum(x => x.Product.Price * x.Quantity);
        }
    }
}
