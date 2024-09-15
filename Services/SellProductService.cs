using AutoMapper;
using FluentValidation.Results;
using Newtonsoft.Json;
using StoreManagementSystem.Builder;
using StoreManagementSystem.Extensions;
using StoreManagementSystem.Models.Domains;
using StoreManagementSystem.Models.ViewModels;
using StoreManagementSystem.Repository.IRepository;
using StoreManagementSystem.Services.IServices;
using StoreManagementSystem.Validation;

namespace StoreManagementSystem.Services
{
    public class SellProductService : ISellProductService
    {
        private readonly HttpContext _httpContext;
        private readonly IMapper _mapper;
        private readonly ILogger<SellProductService> _logger;
        private readonly ISellProductRepository _sellProductRepository;
        private readonly IProductRepository _productRepository;
        private readonly IPurchaseProductRepository _purchaseProductRepository;
        public SellProductService(IHttpContextAccessor httpContextAccessor, IMapper mapper, ILogger<SellProductService> logger, ISellProductRepository sellProductRepository, IProductRepository productRepository, IPurchaseProductRepository purchaseProductRepository)
        {
            _httpContext = httpContextAccessor.HttpContext;
            _mapper = mapper;
            _logger = logger;
            _sellProductRepository = sellProductRepository;
            _productRepository = productRepository;
            _purchaseProductRepository = purchaseProductRepository;
        }

        public async Task<(SellProductDisplayModel, StoreManagmentError)> CreateSellProduct(SellProductAddModel sellProduct, CancellationToken cancellationToken = default)
        {
            string? error;
            try
            {
                var stock = 0;
                //Getting User Id
                var userId = _httpContext.User.GetUserId();

                //Validation
                SellProductValidation validationRules = new SellProductValidation();
                ValidationResult validationResult = validationRules.Validate(sellProduct);
                if (!validationResult.IsValid)
                {
                    var Errormessage = "";
                    foreach (ValidationFailure validationFailure in validationResult.Errors)
                    {
                        Errormessage += validationFailure.ErrorMessage;
                    }
                    _logger.LogError("{Message}", Errormessage);
                    return (new NullSellProductDisplayModel(), new StoreManagmentError(Errormessage));
                }

                //Checking SellProductId
                var productData = await _productRepository.GetProductById(sellProduct.ProductId, cancellationToken);
                if (productData == null)
                {
                    error = "Product Not Found with Id: " + sellProduct.ProductId;
                    _logger.LogError("{Message}", error);
                    return (new NullSellProductDisplayModel(), new StoreManagmentError(error));
                }

                //Checking in Stock
                var purchasedQuantity = await _purchaseProductRepository.GetPurchaseProductByProductId(productData.Id);
                stock = purchasedQuantity.Sum(x => x.Quantity) - sellProduct.Quantity;

                if (stock <= 0)
                {
                    error = $"Error Product {productData.Name} has not in Stock for this much Quantity : {sellProduct.Quantity}";
                    _logger.LogError("{Message}", error);
                    return (new NullSellProductDisplayModel(), new StoreManagmentError(error));
                }

                //AddSellProduct
                var addSellProduct = SellProductBuilder.Convert(sellProduct, userId);
                var sellProductId = await _sellProductRepository.AddSellProductData(addSellProduct, cancellationToken);
                if (sellProductId <= 0)
                {
                    error = "Error While Adding SellProduct";
                    _logger.LogError("{Message}", error);
                    return (new NullSellProductDisplayModel(), new StoreManagmentError(error));
                }
                var sellProductData = await _sellProductRepository.GetSellProductById(sellProductId, cancellationToken);
                var getSellProduct = _mapper.Map<SellProductDisplayModel>(sellProductData);
                _logger.LogInformation("{Message}", JsonConvert.SerializeObject(getSellProduct), Formatting.Indented);
                return (getSellProduct, new StoreManagmentError(string.Empty));
            }
            catch (Exception ex)
            {
                error = "SellProductService CreatePurchaseProduct API Error " + ex.Message;
                _logger.LogError("{Message}", error);
                return (new NullSellProductDisplayModel(), new StoreManagmentError(error));
            }
        }

        public async Task<(List<SellProductDisplayModel>, StoreManagmentError)> GetAllSellProduct(CancellationToken cancellationToken = default)
        {
            string? error;
            try
            {
                var sellProductData = await _sellProductRepository.GetAllSellProduct(cancellationToken);
                if (sellProductData.Count <= 0)
                {
                    error = "Sell Product Records Not Found";
                    _logger.LogError("{Message}", error);
                    return (new NullSellProductListDisplayModel(), new StoreManagmentError(error));
                }
                var sellProducts = _mapper.Map<List<SellProductDisplayModel>>(sellProductData);

                _logger.LogInformation("{Message}", JsonConvert.SerializeObject(sellProducts));

                return (sellProducts, new StoreManagmentError(string.Empty));
            }
            catch (Exception ex)
            {
                error = "SellProductService GetAllSellProduct API Error " + ex.Message;
                _logger.LogError("{Message}", error);
                return (new NullSellProductListDisplayModel(), new StoreManagmentError(error));
            }
        }

        public async Task<(List<StockProductDisplayModel>, StoreManagmentError)> ProductStock(CancellationToken cancellationToken = default)
        {
            string? error;
            try
            {

                var productsData = await _productRepository.GetAllProducts(cancellationToken);
                if (productsData == null)
                {
                    error = "Products Not Found ";
                    _logger.LogError("{Message}", error);
                    return (new NullStockProductListDisplayModel(), new StoreManagmentError(error));
                }
                var result = new List<StockProductDisplayModel>();
                foreach (Product item in productsData)
                {
                    var purchasedQuantity = await _purchaseProductRepository.GetPurchaseProductByProductId(item.Id);

                    var sellQuantity = await _sellProductRepository.GetSellProductByProductId(item.Id);

                    var stock = purchasedQuantity?.Sum(x => x.Quantity) - sellQuantity?.Sum(x => x.Quantity);

                    var sellProducts = _mapper.Map<StockProductDisplayModel>(item);
                    sellProducts.QuantityInStock = !stock.HasValue ? 0 : stock.Value;
                    result.Add(sellProducts);

                }
                return (result, new StoreManagmentError(string.Empty));

            }
            catch (Exception ex)
            {
                error = "SellProductService ProductStock API Error " + ex.Message;
                _logger.LogError("{Message}", error);
                return (new NullStockProductListDisplayModel(), new StoreManagmentError(error));
            }
        }

        public async Task<(string, StoreManagmentError)> RemoveSellProduct(int sellProductId, CancellationToken cancellationToken = default)
        {
            string? error;
            try
            {
                //Getting User Id
                var userId = _httpContext.User.GetUserId();

                var sellProductData = await _sellProductRepository.GetSellProductById(sellProductId, cancellationToken);
                if (sellProductData == null)
                {
                    error = "Purchase Product Not Found With Id : " + sellProductId;
                    _logger.LogError("{Message}", error);
                    return (string.Empty, new StoreManagmentError(error));
                }

                //Removing SellProductData
                var IsPurchaseProductRemove = await _sellProductRepository.RemoveSellProduct(sellProductData, userId, cancellationToken);
                if (!IsPurchaseProductRemove)
                {
                    error = "Error while Deleting Sell Product";
                    _logger.LogError("{Message}", error);
                    return (string.Empty, new StoreManagmentError(error));
                }
                return ("Sell Product Record Deleted Successfully", new StoreManagmentError(string.Empty));
            }
            catch (Exception ex)
            {
                error = "SellProductService RemoveSellProduct API Error " + ex.Message;
                _logger.LogError("{Message}", error);
                return (string.Empty, new StoreManagmentError(error));

            }
        }

        public async Task<(SellProductDisplayModel, StoreManagmentError)> UpdateSellProduct(int sellProductId, SellProductAddModel updateSellProduct, CancellationToken cancellationToken = default)
        {
            string? error;
            try
            {
                //Getting User Id
                var userId = _httpContext.User.GetUserId();

                //Validation
                SellProductValidation validationRules = new SellProductValidation();
                ValidationResult validationResult = validationRules.Validate(updateSellProduct);
                if (!validationResult.IsValid)
                {
                    var Errormessage = "";
                    foreach (ValidationFailure validationFailure in validationResult.Errors)
                    {
                        Errormessage += validationFailure.ErrorMessage;
                    }
                    _logger.LogError("{Message}", Errormessage);
                    return (new NullSellProductDisplayModel(), new StoreManagmentError(Errormessage));
                }
                //Checking SellProductId
                var productData = await _productRepository.GetProductById(updateSellProduct.ProductId, cancellationToken);
                if (productData == null)
                {
                    error = "Product Not Found with Id: " + updateSellProduct.ProductId;
                    _logger.LogError("{Message}", error);
                    return (new NullSellProductDisplayModel(), new StoreManagmentError(error));
                }
                var sellProductData = await _sellProductRepository.GetSellProductById(sellProductId, cancellationToken);
                if (sellProductData == null)
                {
                    error = "Purchase Product Not Found With Id : " + sellProductId;
                    _logger.LogError("{Message}", error);
                    return (new NullSellProductDisplayModel(), new StoreManagmentError(error));
                }

                //UpdateSellProductData
                var updateSellProductData = _mapper.Map(updateSellProduct, sellProductData);
                var updatedSellProduct = await _sellProductRepository.UpdateSellProduct(updateSellProductData, userId, cancellationToken);
                if (updatedSellProduct == null)
                {
                    error = "Error While Updating SellProduct";
                    _logger.LogError("{Message}", error);
                    return (new NullSellProductDisplayModel(), new StoreManagmentError(error));
                }

                var getUpdatedSellProduct = _mapper.Map<SellProductDisplayModel>(updatedSellProduct);
                _logger.LogInformation("{Message}", JsonConvert.SerializeObject(getUpdatedSellProduct, Formatting.Indented));
                return (getUpdatedSellProduct, new StoreManagmentError(string.Empty));
            }
            catch (Exception ex)
            {

                error = "SellProductService UpdateSellProduct API Error " + ex.Message;
                _logger.LogError("{Message}", error);
                return (new NullSellProductDisplayModel(), new StoreManagmentError(error));
            }
        }
    }
}
