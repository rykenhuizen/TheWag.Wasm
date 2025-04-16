using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using TheWag.Functions.EF;
using TheWag.Models;

namespace TheWag.Functions.Endpoints
{
    public class Product(ILogger<Product> logger, WagDbContext context, IMapper mapper)
    {
        private readonly ILogger<Product> _logger = logger;
        private readonly WagDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        [Function("GetAllProducts")]
        public IActionResult GetAll([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req)
        {
            _logger.LogInformation("GetAllProducts called");
            var result = _mapper.ProjectTo<ProductDTO>(_context.Products).ToList();

            return new OkObjectResult(result);
        }

        [Function("CreateProduct")]
        public IActionResult Create([HttpTrigger(AuthorizationLevel.Anonymous, "post")][FromBody] HttpRequest req)
        {
            _logger.LogInformation("CreateProduct called");
            var product = req.ReadFromJsonAsync<ProductDTO>().Result;

            var p = _mapper.Map<EF.Product>(product);
            //var r = _context.Products.Add(_mapper.Map<Functions.EF.Product>(p));
            _context.Products.Add(p);
            _context.SaveChanges();

            return new OkObjectResult("Product created");
        }
    }
}
