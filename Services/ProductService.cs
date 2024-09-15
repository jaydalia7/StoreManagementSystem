using AutoMapper;
using FluentValidation.Results;
using Newtonsoft.Json;
using StoreManagementSystem.Builder;
using StoreManagementSystem.Extensions;
using StoreManagementSystem.Models.ViewModels;
using StoreManagementSystem.Repository.IRepository;
using StoreManagementSystem.Services.IServices;
using StoreManagementSystem.Validation;
using static StoreManagementSystem.Models.ViewModels.ProductDisplayModel;

namespace StoreManagementSystem.Services
{
    public class ProductService : IProductService
    {
        private readonly ILogger<ProductService> _logger;
        private readonly HttpContext _httpContext;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        public ProductService(ILogger<ProductService> logger, IHttpContextAccessor httpContextAccessor, IProductRepository productRepository, IMapper mapper)
        {
            _logger = logger;
            _httpContext = httpContextAccessor.HttpContext;
            _productRepository = productRepository;
            _mapper = mapper;
        }
        public async Task<(ProductDisplayModel, StoreManagmentError)> CreateProduct(ProductAddModel product, CancellationToken cancellationToken = default)
        {
            string? error;
            try
            {
                //Validation
                ProductValidation validationRules = new ProductValidation();
                ValidationResult validationResult = validationRules.Validate(product);
                if (!validationResult.IsValid)
                {
                    var Errormessage = "";
                    foreach (ValidationFailure validationFailure in validationResult.Errors)
                    {
                        Errormessage += validationFailure.ErrorMessage;
                    }
                    _logger.LogError("{Message}", Errormessage);
                    return (new NullProductDisplayModel(), new StoreManagmentError(Errormessage));
                }

                //Checking User Role 
                var userRole = _httpContext.User.GetUserRole();
                if (!userRole.Equals("SuperAdmin") && !userRole.Equals("Admin"))
                {
                    error = "Error Only Super Admin Or Admin Can Add Product";
                    _logger.LogError("{Message}", error);
                    return (new NullProductDisplayModel(), new StoreManagmentError(error));
                }

                var CheckProductExistOrNot = await _productRepository.CheckProductByName(product.Name);
                if (CheckProductExistOrNot)
                {
                    error = "Oops Product Already Exists";
                    _logger.LogError("{Message}", error);
                    return (new NullProductDisplayModel(), new StoreManagmentError(error));
                }

                //AddProductData
                var addProduct = ProductBuilder.Convert(product);
                var productId = await _productRepository.AddProductData(addProduct, cancellationToken);
                if (productId <= 0)
                {
                    error = "Error While Adding Product";
                    _logger.LogError("{Message}", error);
                    return (new NullProductDisplayModel(), new StoreManagmentError(error));
                }
                var productData = await _productRepository.GetProductById(productId, cancellationToken);
                var getproductData = _mapper.Map<ProductDisplayModel>(productData);
                _logger.LogInformation("{Message}", JsonConvert.SerializeObject(getproductData, Formatting.Indented));
                return (getproductData, new StoreManagmentError(string.Empty));
            }
            catch (Exception ex)
            {
                error = "ProductService CreateProduct API Error " + ex.Message;
                _logger.LogError("{Message}", error);
                return (new NullProductDisplayModel(), new StoreManagmentError(error));
            }
        }

        public async Task<(List<ProductDisplayModel>, StoreManagmentError)> GetAllProducts(CancellationToken cancellationToken = default)
        {
            string? error;
            try
            {
                //Checking User Role 
                var userRole = _httpContext.User.GetUserRole();
                if (!userRole.Equals("SuperAdmin") && !userRole.Equals("Admin"))
                {
                    error = "Error Only Super Admin Or Admin Can Add Product";
                    _logger.LogError("{Message}", error);
                    return (new NullProductListDisplayModel(), new StoreManagmentError(error));
                }
                var productData = await _productRepository.GetAllProducts(cancellationToken);
                var productsData = _mapper.Map<List<ProductDisplayModel>>(productData);

                _logger.LogInformation("{Message}", JsonConvert.SerializeObject(productsData));

                return (productsData, new StoreManagmentError(string.Empty));
            }
            catch (Exception ex)
            {
                error = "ProductService GetAllProducts API Error " + ex.Message;
                _logger.LogError("{Message}", error);
                return (new NullProductListDisplayModel(), new StoreManagmentError(error));
            }
        }

        public async Task<(string, StoreManagmentError)> RemoveProduct(int productId, CancellationToken cancellationToken = default)
        {
            string? error;
            try
            {
                var productData = await _productRepository.GetProductById(productId, cancellationToken);
                if (productData == null)
                {
                    error = "Error While Getting ProductById : " + productId;
                    _logger.LogError("{Message}", error);
                    return (string.Empty, new StoreManagmentError(error));
                }
                //Removing ProductData
                var IsProductRemove = await _productRepository.RemoveProduct(productData, cancellationToken);
                if (!IsProductRemove)
                {
                    error = "Error while Deleting Product";
                    _logger.LogError("{Message}", error);
                    return (string.Empty, new StoreManagmentError(error));
                }
                return ("Product Deleted Successfully", new StoreManagmentError(string.Empty));

            }
            catch (Exception ex)
            {
                error = "ProductService UpdateProduct API Error " + ex.Message;
                _logger.LogError("{Message}", error);
                return (string.Empty, new StoreManagmentError(error));
            }
        }

        public async Task<(ProductDisplayModel, StoreManagmentError)> UpdateProduct(int productId, ProductAddModel productUpdateModel, CancellationToken cancellationToken = default)
        {
            string? error;
            try
            {
                //Validation
                ProductValidation validationRules = new ProductValidation();
                ValidationResult validationResult = validationRules.Validate(productUpdateModel);
                if (!validationResult.IsValid)
                {
                    var Errormessage = "";
                    foreach (ValidationFailure validationFailure in validationResult.Errors)
                    {
                        Errormessage += validationFailure.ErrorMessage;
                    }
                    _logger.LogError("{Message}", Errormessage);
                    return (new NullProductDisplayModel(), new StoreManagmentError(Errormessage));
                }

                //GettingProduct
                var productData = await _productRepository.GetProductById(productId, cancellationToken);
                if (productData == null)
                {
                    error = "Error While Getting ProductById : " + productId;
                    _logger.LogError("{Message}", error);
                    return (new NullProductDisplayModel(), new StoreManagmentError(error));
                }
                //UpdateProductData
                var updateProduct = ProductBuilder.Convert(productUpdateModel);
                var updatedProduct = await _productRepository.UpdateProductData(productId, updateProduct, cancellationToken);

                if (updatedProduct == null)
                {
                    error = "Error while Updating Product";
                    _logger.LogError("{Message}", error);
                    return (new NullProductDisplayModel(), new StoreManagmentError(error));
                }

                var getUpdatedProductData = _mapper.Map<ProductDisplayModel>(updatedProduct);
                _logger.LogInformation("{Message}", JsonConvert.SerializeObject(getUpdatedProductData, Formatting.Indented));
                return (getUpdatedProductData, new StoreManagmentError(string.Empty));
            }
            catch (Exception ex)
            {
                error = "ProductService UpdateProduct API Error " + ex.Message;
                _logger.LogError("{Message}", error);
                return (new NullProductDisplayModel(), new StoreManagmentError(error));
            }
        }
    }
}
