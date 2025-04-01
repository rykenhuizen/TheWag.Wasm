using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using TheWag.Api.WagDB.EF;
using TheWag.Models;

namespace TheWag.Functions
{
    public class Data
    {
        private readonly ILogger<Data> _logger;
        private readonly WagDbContext _context;
        private readonly IMapper _mapper;

        public Data(ILogger<Data> logger, WagDbContext context, IMapper mapper)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;


        }

        [Function("GetAllProducts")]
        public IActionResult GetAll([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            var result = _mapper.ProjectTo<ProductDTO>(_context.Products).ToList();
            
            return new OkObjectResult(result);
        }

        [Function("CreateProduct")]
        public IActionResult Create([HttpTrigger(AuthorizationLevel.Function, "post")][FromBody] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            var product = req.ReadFromJsonAsync<ProductDTO>().Result;

            var p = _mapper.Map<Product>(product);
            var r = _context.Products.Add(_mapper.Map<Product>(p));
            _context.SaveChanges();

            return new OkObjectResult("Product created");
        }
    }
}
