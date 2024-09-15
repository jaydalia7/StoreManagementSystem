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
    public class UserService : IUserService
    {

        private readonly IUserRepository _userRepository;
        private readonly IHelperService _helperService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly HttpContext _httpContext;
        private readonly ILogger<UserService> _logger;
        public UserService(IUserRepository userRepository, IHelperService helperService, IMapper mapper, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _helperService = helperService;
            _mapper = mapper;
            _configuration = configuration;
            _httpContext = httpContextAccessor.HttpContext;
            _logger = logger;
        }
        public async Task<(UserDisplayModel, StoreManagmentError)> CreateUser(UserAddModel user, CancellationToken cancellationToken = default)
        {
            string? error;
            try
            {
                //Validation
                UserValidation validationRules = new UserValidation();
                ValidationResult validationResult = validationRules.Validate(user);
                if (!validationResult.IsValid)
                {
                    var Errormessage = "";
                    foreach (ValidationFailure validationFailure in validationResult.Errors)
                    {
                        Errormessage += validationFailure.ErrorMessage;
                    }
                    _logger.LogError("{Message}", Errormessage);
                    return (new NullUserDisplayModel(), new StoreManagmentError(Errormessage));
                }

                //Checking User Role 
                var userRole = _httpContext.User.GetUserRole();
                if (!userRole.Equals("SuperAdmin") && user.IsAdmin)
                {
                    error = "Error Only Super Admin Can Add Admin";
                    _logger.LogError("{Message}", error);
                    return (new NullUserDisplayModel(), new StoreManagmentError(error));
                }

                //CheckExistByEmail
                var CheckExistsByEmail = await _userRepository.GetUserEmailAddress(user.EmailAddress, cancellationToken);
                if (CheckExistsByEmail != null)
                {
                    error = "Oops Email Address Already Exists";
                    _logger.LogError("{Message}", error);
                    return (new NullUserDisplayModel(), new StoreManagmentError(error));
                }

                //EncryptPassword
                var encryptedPassword = _helperService.PasswordSalt(user.Password);
                var addUser = UserBuilder.Convert(user, encryptedPassword);

                //AddUserData
                var userId = await _userRepository.AddUserData(addUser, cancellationToken);
                if (userId <= 0)
                {
                    error = "Error While Adding User";
                    _logger.LogError("{Message}", error);
                    return (new NullUserDisplayModel(), new StoreManagmentError(error));
                }
                var userData = await _userRepository.GetUserById(userId, cancellationToken);

                var getUserData = _mapper.Map<UserDisplayModel>(userData);

                _logger.LogInformation("{Message}", JsonConvert.SerializeObject(getUserData));

                return (getUserData, new StoreManagmentError(string.Empty));
            }
            catch (Exception ex)
            {
                error = "UserService InsertAdmin API Error " + ex.Message;
                _logger.LogError("{Message}", error);
                return (new NullUserDisplayModel(), new StoreManagmentError(error));
            }
        }

        public async Task<(List<UserDisplayModel>, StoreManagmentError)> GetActiveUsers(CancellationToken cancellationToken = default)
        {
            string? error;
            try
            {
                //Checking User Role
                var userRole = _httpContext.User.GetUserRole();
                var usersData = await _userRepository.GetActiveUsers(userRole, cancellationToken);
                if (usersData == null)
                {
                    error = "Acitve Users Record Not Found";
                    _logger.LogError("{Message}", error);
                    return (new NullUserListDisplayModel(), new StoreManagmentError(error));
                }
                var listUserData = _mapper.Map<List<UserDisplayModel>>(usersData);

                _logger.LogInformation("{Message}", JsonConvert.SerializeObject(listUserData));

                return (listUserData, new StoreManagmentError(string.Empty));
            }
            catch (Exception ex)
            {
                error = "UserService GetAllUser API Error " + ex.Message;
                _logger.LogError("{Message}", error);
                return (new NullUserListDisplayModel(), new StoreManagmentError(error));
            }
        }

        public async Task<(List<UserDisplayModel>, StoreManagmentError)> GetAllUsers(CancellationToken cancellationToken = default)
        {
            string? error;
            try
            {
                //Checking User Role
                var userRole = _httpContext.User.GetUserRole();
                var usersData = await _userRepository.GetAllUser(userRole, cancellationToken);
                if (usersData == null)
                {
                    error = "Users Record Not Found";
                    _logger.LogError("{Message}", error);
                    return (new NullUserListDisplayModel(), new StoreManagmentError(error));
                }
                var listActiveUserData = _mapper.Map<List<UserDisplayModel>>(usersData);

                _logger.LogInformation("{Message}", JsonConvert.SerializeObject(listActiveUserData));
                return (listActiveUserData, new StoreManagmentError(string.Empty));
            }
            catch (Exception ex)
            {
                error = "UserService GetAllUser API Error " + ex.Message;
                _logger.LogError("{Message}", error);
                return (new NullUserListDisplayModel(), new StoreManagmentError(error));
            }
        }

        public async Task<(string, StoreManagmentError)> Login(UserLoginModel userLogin, CancellationToken cancellationToken = default)
        {
            string? error;
            try
            {

                //Validation
                LoginValidation validationRules = new LoginValidation();
                ValidationResult validationResult = validationRules.Validate(userLogin);
                if (!validationResult.IsValid)
                {
                    var Errormessage = "";
                    foreach (ValidationFailure validationFailure in validationResult.Errors)
                    {
                        Errormessage += validationFailure.ErrorMessage;
                    }
                    _logger.LogError("{Message}", Errormessage);
                    return (string.Empty, new StoreManagmentError(Errormessage));
                }
                var isSuperAdmin = false;

                //GetUserData
                var userData = await _userRepository.GetUserEmailAddress(userLogin.EmailAddress);
                if (userData == null)
                {
                    error = "Email Address Not Found";
                    _logger.LogError("{Message}", error);
                    return (string.Empty, new StoreManagmentError(error));
                }

                //Encrypting Password
                var encryptedPassword = _helperService.PasswordSalt(userLogin.Password);
                if (encryptedPassword != userData.Password)
                {
                    error = "Please Enter Valid Password";
                    _logger.LogError("{Message}", error);
                    return (string.Empty, new StoreManagmentError(error));
                }
                if (userData.EmailAddress.Equals(_configuration["SuperAdmin:EmailAddress"]))
                {
                    isSuperAdmin = true;
                }

                //Generate Token
                var token = _helperService.Authenticate(userData, isSuperAdmin);
                _logger.LogInformation("{Message}", "Token Generated : " + token);
                return (token, new StoreManagmentError(string.Empty));
            }

            catch (Exception ex)
            {
                error = "UserService InsertAdmin API Error " + ex.Message;
                _logger.LogError("{Message}", error);
                return (string.Empty, new StoreManagmentError(error));
            }
        }

        public async Task<(string, StoreManagmentError)> RemoveUser(int userId, CancellationToken cancellationToken = default)
        {
            string? error;
            try
            {
                //Getting User Data
                var userData = await _userRepository.GetUserById(userId, cancellationToken);
                if (userData == null)
                {
                    error = "User Record Not Found By Id : " + userId;
                    _logger.LogError("{Message}", error);
                    return (string.Empty, new StoreManagmentError(error));
                }

                //Checking User Role
                var userRole = _httpContext.User.GetUserRole();
                if (!userRole.Equals("SuperAdmin") && userData.IsAdmin)
                    return (string.Empty, new StoreManagmentError("Error Only Super Admin Can Remove Admin"));

                var user = await _userRepository.RemoveUser(userData, cancellationToken);
                _logger.LogInformation("{Message}", JsonConvert.SerializeObject(user));
                if (!user)
                {
                    error = "Error While Removing User";
                    _logger.LogError("{Message}", error);
                    return (string.Empty, new StoreManagmentError(error));
                }
                else
                {
                    _logger.LogInformation("User Removed Successfully");
                    return ("User Removed Successfully", new StoreManagmentError(string.Empty));
                }
            }
            catch (Exception ex)
            {
                error = "UserService RemoveUser API Error " + ex.Message;
                _logger.LogError("{Message}", error);
                return (string.Empty, new StoreManagmentError(error));
            }
        }
    }
}
