using AutoMapper;
using FluentValidation.Results;
using Newtonsoft.Json;
using StoreManagementSystem.Builder;
using StoreManagementSystem.Extensions;
using StoreManagementSystem.Models.ViewModels;
using StoreManagementSystem.Repository.IRepository;
using StoreManagementSystem.Services.IServices;
using StoreManagementSystem.Validation;

namespace StoreManagementSystem.Services
{
    public class PurchaseProductService : IPurchaseProductService
    {
        private readonly HttpContext _httpContext;
        private readonly IPurchaseProductRepository _purchaseProductRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<PurchaseProductService> _logger;
        private readonly IProductRepository _productRepository;

        public PurchaseProductService(IHttpContextAccessor httpContextAccessor, IPurchaseProductRepository purchaseProductRepository, IMapper mapper, ILogger<PurchaseProductService> logger, IProductRepository productRepository)
        {
            _httpContext = httpContextAccessor.HttpContext;
            _purchaseProductRepository = purchaseProductRepository;
            _mapper = mapper;
            _logger = logger;
            _productRepository = productRepository;
        }

        public async Task<(PurchaseProductDisplayModel, StoreManagmentError)> CreatePurchaseProduct(PurchaseProductAddModel purchaseProduct, CancellationToken cancellationToken = default)
        {
            string? error;
            try
            {
                //Getting User Id
                var userId = _httpContext.User.GetUserId();

                //Validation
                ProductPurchaseValidation validationRules = new ProductPurchaseValidation();
                ValidationResult validationResult = validationRules.Validate(purchaseProduct);
                if (!validationResult.IsValid)
                {
                    var Errormessage = "";
                    foreach (ValidationFailure validationFailure in validationResult.Errors)
                    {
                        Errormessage += validationFailure.ErrorMessage;
                    }
                    _logger.LogError("{Message}", Errormessage);
                    return (new NullPurchaseProductDisplayModel(), new StoreManagmentError(Errormessage));
                }
                var productData = await _productRepository.GetProductById(purchaseProduct.ProductId, cancellationToken);
                if (productData == null)
                {
                    error = "Product Not Found with Id: " + purchaseProduct.ProductId;
                    _logger.LogError("{Message}", error);
                    return (new NullPurchaseProductDisplayModel(), new StoreManagmentError(error));
                }

                //AddPurchaseProduct
                var addPurchaseProduct = PurchaseProductBuilder.Convert(purchaseProduct, userId);
                var purchaseProductId = await _purchaseProductRepository.AddPurchaseProductData(addPurchaseProduct, cancellationToken);
                if (purchaseProductId <= 0)
                {
                    error = "Error While Adding PurchaseProduct";
                    _logger.LogError("{Message}", error);
                    return (new NullPurchaseProductDisplayModel(), new StoreManagmentError(error));
                }
                var purchaseProductData = await _purchaseProductRepository.GetPurchaseProductById(purchaseProductId, cancellationToken);
                var getPurchaseProduct = _mapper.Map<PurchaseProductDisplayModel>(purchaseProductData);
                _logger.LogInformation("{Message}", JsonConvert.SerializeObject(getPurchaseProduct, Formatting.Indented));
                return (getPurchaseProduct, new StoreManagmentError(string.Empty));
            }
            catch (Exception ex)
            {
                error = "PurchaseProductService CreatePurchaseProduct API Error " + ex.Message;
                _logger.LogError("{Message}", error);
                return (new NullPurchaseProductDisplayModel(), new StoreManagmentError(error));
            }
        }

        public async Task<(List<PurchaseProductDisplayModel>, StoreManagmentError)> GetAllPurchaseProduct(CancellationToken cancellationToken = default)
        {
            string? error;
            try
            {
                var purchaseProductData = await _purchaseProductRepository.GetAllPurchaseProduct(cancellationToken);
                if (purchaseProductData.Count <= 0)
                {
                    error = "Purchase Product Records Not Found";
                    _logger.LogError("{Message}", error);
                    return (new NullPurchaseProductListDisplayModel(), new StoreManagmentError(error));
                }
                var purchaseProducts = _mapper.Map<List<PurchaseProductDisplayModel>>(purchaseProductData);

                _logger.LogInformation("{Message}", JsonConvert.SerializeObject(purchaseProducts));

                return (purchaseProducts, new StoreManagmentError(string.Empty));
            }
            catch (Exception ex)
            {
                error = "PurchaseProductService GetAllPurchaseProduct API Error " + ex.Message;
                _logger.LogError("{Message}", error);
                return (new NullPurchaseProductListDisplayModel(), new StoreManagmentError(error));
            }
        }

        public async Task<(string, StoreManagmentError)> RemovePurchaseProduct(int purchaseProductId, CancellationToken cancellationToken = default)
        {
            string? error;
            try
            {
                //Getting User Id
                var userId = _httpContext.User.GetUserId();

                var purchaseProductData = await _purchaseProductRepository.GetPurchaseProductById(purchaseProductId, cancellationToken);
                if (purchaseProductData == null)
                {
                    error = "Purchase Product Records Not Found By Id : " + purchaseProductId;
                    _logger.LogError("{Message}", error);
                    return (string.Empty, new StoreManagmentError(error));
                }

                //Removing PurchaseProductData
                var IsPurchaseProductRemove = await _purchaseProductRepository.RemovePurchaseProduct(purchaseProductData, userId, cancellationToken);
                if (!IsPurchaseProductRemove)
                {
                    error = "Error while Deleting Purchase Product";
                    _logger.LogError("{Message}", error);
                    return (string.Empty, new StoreManagmentError(error));
                }

                return ("Purchase Product Record Deleted Successfully", new StoreManagmentError(string.Empty));
            }
            catch (Exception ex)
            {
                error = "PurchaseProductService RemovePurchaseProduct API Error " + ex.Message;
                _logger.LogError("{Message}", error);
                return (string.Empty, new StoreManagmentError(error));
            }
        }

        public async Task<(PurchaseProductDisplayModel, StoreManagmentError)> UpdatePurchaseProduct(int purchaseProductId, PurchaseProductAddModel updatePurchaseProduct, CancellationToken cancellationToken = default)
        {
            string? error;
            try
            {
                //Getting User Id
                var userId = _httpContext.User.GetUserId();
                //Validation
                ProductPurchaseValidation validationRules = new ProductPurchaseValidation();
                ValidationResult validationResult = validationRules.Validate(updatePurchaseProduct);
                if (!validationResult.IsValid)
                {
                    var Errormessage = "";
                    foreach (ValidationFailure validationFailure in validationResult.Errors)
                    {
                        Errormessage += validationFailure.ErrorMessage;
                    }
                    _logger.LogError("{Message}", Errormessage);
                    return (new NullPurchaseProductDisplayModel(), new StoreManagmentError(Errormessage));
                }

                //Checking PurchaseProductId
                var productData = await _productRepository.GetProductById(updatePurchaseProduct.ProductId, cancellationToken);
                if (productData == null)
                {
                    error = "Product Not Found with Id: " + updatePurchaseProduct.ProductId;
                    _logger.LogError("{Message}", error);
                    return (new NullPurchaseProductDisplayModel(), new StoreManagmentError(error));
                }
                var purchaseProductData = await _purchaseProductRepository.GetPurchaseProductById(purchaseProductId, cancellationToken);
                if (purchaseProductData == null)
                {
                    error = "Purchase Product Not Found With Id : " + purchaseProductId;
                    _logger.LogError("{Message}", error);
                    return (new NullPurchaseProductDisplayModel(), new StoreManagmentError(error));
                }

                //UpdatePurchaseProductData
                var updatePurchaseProductData = _mapper.Map(updatePurchaseProduct, purchaseProductData);

                var updatePurchasedProduct = await _purchaseProductRepository.UpdatePurchaseProduct(updatePurchaseProductData, userId, cancellationToken);

                if (updatePurchasedProduct == null)
                {
                    error = "Error while Updating PurchaseProduct";
                    _logger.LogError("{Message}", error);
                    return (new NullPurchaseProductDisplayModel(), new StoreManagmentError(error));
                }

                var getUpdatedPurchaseProduct = _mapper.Map<PurchaseProductDisplayModel>(updatePurchasedProduct);
                _logger.LogInformation("{Message}", JsonConvert.SerializeObject(getUpdatedPurchaseProduct, Formatting.Indented));
                return (getUpdatedPurchaseProduct, new StoreManagmentError(string.Empty));
            }
            catch (Exception ex)
            {
                error = "PurchaseProductService UpdatePurchaseProduct API Error " + ex.Message;
                _logger.LogError("{Message}", error);
                return (new NullPurchaseProductDisplayModel(), new StoreManagmentError(error));
            }
        }
    }
}
