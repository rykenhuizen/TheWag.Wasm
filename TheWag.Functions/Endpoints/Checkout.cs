using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using TheWag.Functions.EF;
using TheWag.Models;
using TheWag.Functions.DataTransforms;
using TheWag.Functions.Services;

namespace TheWag.Functions.Endpoints
{
    public class Checkout(ILogger<Checkout> logger, WagDbContext context, EmailService emailService)
    {
        private readonly ILogger<Checkout> _logger = logger;
        private readonly WagDbContext _context = context;
        private readonly EmailService _emailService = emailService;

        [Function("Checkout")]
        public async Task<IActionResult> CheckoutAndEmail([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req)
        {
            try
            {
                var cart = await req.ReadFromJsonAsync<CustomerCart>();
                if (cart == null)
                {
                    _logger.LogError("Cart data is null.");
                    return new BadRequestObjectResult("Cart data is null.");
                }
                if (cart.Customer == null)
                {
                    _logger.LogError("Cart customer is null.");
                    return new BadRequestObjectResult("Customer is null.");
                }

                var customer = _context.Customers.FirstOrDefault(x => x.Email == cart.Customer.Email);
                if (customer == null)
                {
                    customer = new EF.Customer() { Email = cart.Customer.Email };
                    _context.Customers.Add(customer);
                    _context.SaveChanges();
                }

                var efOrder = ModelTransforms.CartToEFOrder(cart, customer);
                _context.Orders.Add(efOrder);
                _context.SaveChanges();

                await _emailService.Send(efOrder.Id, cart);

                return new OkObjectResult(efOrder.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking out customer.");
                return new BadRequestObjectResult("Error checking out customer.");
            }
            
        }

    }
}
