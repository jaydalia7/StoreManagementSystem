using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreManagementSystem.Models.ViewModels;
using StoreManagementSystem.Services.IServices;
using System.ComponentModel.DataAnnotations;
using static StoreManagementSystem.Models.ViewModels.ProductDisplayModel;

namespace StoreManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<UserController> _logger;
        private readonly IPurchaseProductService _purchaseProductService;
        private readonly ISellProductService _sellProductService;
        public ProductController(IProductService productService, ILogger<UserController> logger, IPurchaseProductService purchaseProductService, ISellProductService sellProductService)
        {
            _productService = productService;
            _logger = logger;
            _purchaseProductService = purchaseProductService;
            _sellProductService = sellProductService;
        }

        #region Product
        [HttpGet]
        [Authorize(Roles = "SuperAdmin,Admin")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ProductDisplayModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(StoreManagmentError))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(Nullable))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(Nullable))]
        public async Task<IActionResult> GetAllProducts(CancellationToken cancellationToken = default)
        {
            using (_logger.BeginScope("{ControllerName}{APIName}", nameof(ProductController), nameof(GetAllProducts)))
            {
                var result = await _productService.GetAllProducts(cancellationToken);
                return result.Item1 is not NullProductListDisplayModel ? Ok(result.Item1) : BadRequest(result.Item2);
            }
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin,Admin")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductDisplayModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(StoreManagmentError))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(Nullable))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(Nullable))]
        public async Task<IActionResult> CreateProduct(ProductAddModel product, CancellationToken cancellationToken = default)
        {
            using (_logger.BeginScope("{ControllerName}{APIName}", nameof(ProductController), nameof(CreateProduct)))
            {
                var result = await _productService.CreateProduct(product, cancellationToken);
                return result.Item1 is not NullProductDisplayModel ? Ok(result.Item1) : BadRequest(result.Item2);
            }
        }

        [HttpPut]
        [Authorize(Roles = "SuperAdmin,Admin")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductDisplayModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(StoreManagmentError))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(Nullable))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(Nullable))]
        public async Task<IActionResult> UpdateProduct([Required] int productId, ProductAddModel productUpdateModel, CancellationToken cancellationToken = default)
        {
            using (_logger.BeginScope("{ControllerName}{APIName}", nameof(ProductController), nameof(UpdateProduct)))
            {
                var result = await _productService.UpdateProduct(productId, productUpdateModel, cancellationToken);
                return result.Item1 is not NullProductDisplayModel ? Ok(result.Item1) : BadRequest(result.Item2);
            }
        }

        [HttpDelete]
        [Authorize(Roles = "SuperAdmin,Admin")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(StoreManagmentError))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(Nullable))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(Nullable))]
        public async Task<IActionResult> RemoveProduct([Required] int productId, CancellationToken cancellationToken = default)
        {
            using (_logger.BeginScope("{ControllerName}{APIName}", nameof(ProductController), nameof(RemoveProduct)))
            {
                var result = await _productService.RemoveProduct(productId, cancellationToken);
                return !string.IsNullOrEmpty(result.Item1) ? Ok(result.Item1) : BadRequest(result.Item2);
            }

        }


        #endregion Product

        #region Purchase Product
        [HttpGet("PurchaseProducts")]
        [Authorize(Roles = "SuperAdmin,Admin")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<PurchaseProductDisplayModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(StoreManagmentError))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(Nullable))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(Nullable))]
        public async Task<IActionResult> GetPurchaseProduct(CancellationToken cancellationToken = default)
        {
            using (_logger.BeginScope("{ControllerName}{APIName}", nameof(ProductController), nameof(GetPurchaseProduct)))
            {
                var result = await _purchaseProductService.GetAllPurchaseProduct(cancellationToken);
                return result.Item1 is not NullPurchaseProductListDisplayModel ? Ok(result.Item1) : BadRequest(result.Item2);
            }
        }

        [HttpPost("PurchaseProduct")]
        [Authorize(Roles = "SuperAdmin,Admin")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PurchaseProductDisplayModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(StoreManagmentError))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(Nullable))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(Nullable))]
        public async Task<IActionResult> CreatePurchaseProduct(PurchaseProductAddModel purchaseProduct, CancellationToken cancellationToken = default)
        {
            using (_logger.BeginScope("{ControllerName}{APIName}", nameof(ProductController), nameof(CreatePurchaseProduct)))
            {
                var result = await _purchaseProductService.CreatePurchaseProduct(purchaseProduct, cancellationToken);
                return result.Item1 is not NullPurchaseProductDisplayModel ? Ok(result.Item1) : BadRequest(result.Item2);
            }
        }

        [HttpPut("PurchaseProduct")]
        [Authorize(Roles = "SuperAdmin")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PurchaseProductDisplayModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(StoreManagmentError))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(Nullable))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(Nullable))]
        public async Task<IActionResult> UpdatePruchaseProduct([Required] int purchasedProductId, PurchaseProductAddModel updatePurchaseProduct, CancellationToken cancellationToken = default)
        {
            using (_logger.BeginScope("{ControllerName}{APIName}", nameof(ProductController), nameof(CreatePurchaseProduct)))
            {
                var result = await _purchaseProductService.UpdatePurchaseProduct(purchasedProductId, updatePurchaseProduct, cancellationToken);
                return result.Item1 is not NullPurchaseProductDisplayModel ? Ok(result.Item1) : BadRequest(result.Item2);
            }
        }

        [HttpDelete("PurchaseProduct")]
        [Authorize(Roles = "SuperAdmin")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(StoreManagmentError))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(Nullable))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(Nullable))]
        public async Task<IActionResult> RemovePurchaseProduct([Required] int purchaseProductId, CancellationToken cancellationToken = default)
        {
            var result = await _purchaseProductService.RemovePurchaseProduct(purchaseProductId, cancellationToken);
            return !string.IsNullOrEmpty(result.Item1) ? Ok(result.Item1) : BadRequest(result.Item2);
        }

        #endregion Purchase Product

        #region Sell Product
        [HttpGet("SellProducts")]
        [Authorize(Roles = "SuperAdmin,Admin,User")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<SellProductDisplayModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(StoreManagmentError))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(Nullable))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(Nullable))]
        public async Task<IActionResult> GetSellProduct(CancellationToken cancellationToken = default)
        {
            using (_logger.BeginScope("{Controller}{APIName}", nameof(ProductController), nameof(GetSellProduct)))
            {
                var result = await _sellProductService.GetAllSellProduct(cancellationToken);
                return result.Item1 is not NullSellProductListDisplayModel ? Ok(result.Item1) : BadRequest(result.Item2);
            }
        }
        
        [HttpPost("SellProduct")]
        [Authorize(Roles = "SuperAdmin,Admin,User")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SellProductDisplayModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(StoreManagmentError))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(Nullable))]
        public async Task<IActionResult> CreateSellProduct(SellProductAddModel sellProduct, CancellationToken cancellationToken = default)
        {
            using (_logger.BeginScope("{Controller}{APIName}", nameof(ProductController), nameof(GetSellProduct)))
            {
                var result = await _sellProductService.CreateSellProduct(sellProduct, cancellationToken);
                return result.Item1 is not NullSellProductDisplayModel ? Ok(result.Item1) : BadRequest(result.Item2);
            }
        }


        [HttpPut("SellProduct")]
        [Authorize(Roles = "SuperAdmin,Admin,User")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SellProductDisplayModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(StoreManagmentError))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(Nullable))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(Nullable))]
        public async Task<IActionResult> UpdateSellProduct([Required] int sellProductId, SellProductAddModel updateSellProduct, CancellationToken cancellationToken = default)
        {
            using (_logger.BeginScope("{Controller}{APIName}", nameof(ProductController), nameof(GetSellProduct)))
            {
                var result = await _sellProductService.UpdateSellProduct(sellProductId, updateSellProduct, cancellationToken);
                return result.Item1 is not NullSellProductDisplayModel ? Ok(result.Item1) : BadRequest(result.Item2);
            }
        }

        [HttpDelete("SellProduct")]
        [Authorize(Roles = "SuperAdmin,Admin,User")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(StoreManagmentError))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(Nullable))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(Nullable))]
        public async Task<IActionResult> RemoveSellProduct([Required] int sellProductId, CancellationToken cancellationToken = default)
        {
            using (_logger.BeginScope("{Controller}{APIName}", nameof(ProductController), nameof(GetSellProduct)))
            {
                var result = await _sellProductService.RemoveSellProduct(sellProductId, cancellationToken);
                return !string.IsNullOrEmpty(result.Item1) ? Ok(result.Item1) : BadRequest(result.Item2);
            }
        }

        [HttpGet("ProductStock")]
        [Authorize(Roles = "SuperAdmin,Admin,User")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<StockProductDisplayModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(StoreManagmentError))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(Nullable))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(Nullable))]
        public async Task<IActionResult> ProductStock(CancellationToken cancellationToken = default)
        {
            using (_logger.BeginScope("{Controller}{APIName}}", nameof(ProductController), nameof(ProductStock)))
            {
                var result = await _sellProductService.ProductStock(cancellationToken);
                return result.Item1 is not NullStockProductListDisplayModel ? Ok(result.Item1) : BadRequest(result.Item2);
            }
        }

        #endregion Sell Product
    }
}