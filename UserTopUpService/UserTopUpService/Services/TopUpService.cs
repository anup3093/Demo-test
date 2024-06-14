using System.Net.Http;
using UserTopUpService.Domain;
using UserTopUpService.Repositories;

namespace UserTopUpService.Services
{
    public class TopUpService : ITopUpService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IUserRepository _userRepository;
        private readonly IBeneficiaryRepository _beneficiaryRepository;
        private readonly IUserBalanceService _userBalanceService;
        private readonly ITopUpHistoryRepository _topUpHistoryRepository;

        public TopUpService(IHttpClientFactory httpClientFactory, IUserRepository userRepository, IBeneficiaryRepository beneficiaryRepository, IUserBalanceService userBalanceService, ITopUpHistoryRepository topUpHistoryRepository)
        {
            _userRepository = userRepository;
            _beneficiaryRepository = beneficiaryRepository;
            _userBalanceService = userBalanceService;
            _topUpHistoryRepository = topUpHistoryRepository;
            _httpClientFactory = httpClientFactory;
        }
        public async Task<Beneficiary> AddBeneficiaryAsync(Beneficiary request)
        {
            var user = await _userRepository.GetUserAsync(request.UserId);

            if (user == null)
            {
                throw new Exception("User not found.");
            }

            if (user.Beneficiaries.Count >= 5)
            {
                throw new Exception("User has already added the maximum number of beneficiaries.");
            }

            var beneficiary = new Beneficiary
            {
                Nickname = request.Nickname,
                UserId = user.Id
            };

            await _beneficiaryRepository.AddBeneficiaryAsync(beneficiary);

            return beneficiary;
        }

        public async Task<List<Beneficiary>> GetBeneficiariesAsync(int UserId)
        {
            var user = await _userRepository.GetUserAsync(UserId);

            if (user == null)
            {
                throw new Exception("User not found.");
            }

            var beneficiaries = await _beneficiaryRepository.GetBeneficiariesAsync(user.Id);

            return beneficiaries;
        }

        public List<int> GetTopUpOptions()
        {
            return new List<int> { 5, 10, 20, 30, 50, 75, 100 };
        }

        public async Task<TopUpResult> TopUpAsync(TopUpRequest request)
        {
            var user = await _userRepository.GetUserAsync(request.UserId);
            var beneficiary = await _beneficiaryRepository.GetBeneficiaryAsync(request.BeneficiaryNickname);

            if (user == null || beneficiary == null)
            {
                return new TopUpResult { Success = false, Message = "User or beneficiary not found." };
            }
            // user's balance from the external service
            var balance = await _userBalanceService.GetUserBalanceAsync(user.Id);

            // Update the user's balance
            user.Balance = balance;
            await _userRepository.UpdateUserAsync(user);

            var totalTopUpForBeneficiaryThisMonth = await _topUpHistoryRepository.GetTotalTopUpForBeneficiaryThisMonth(beneficiary.Id);
            var totalTopUpForUserThisMonth = await _topUpHistoryRepository.GetTotalTopUpForUserThisMonth(user.Id);

            var limitPerBeneficiary = user.IsVerified == 1 ? 500 : 1000;
            var limitForAllBeneficiaries = 3000;

            if (totalTopUpForBeneficiaryThisMonth + request.TopUpAmount > limitPerBeneficiary ||
                totalTopUpForUserThisMonth + request.TopUpAmount > limitForAllBeneficiaries)
            {
                return new TopUpResult { Success = false, Message = "Monthly top-up limit exceeded." };
            }

            if (user.Balance < request.TopUpAmount + 1) // +1 for the transaction fee
            {
                return new TopUpResult { Success = false, Message = "Insufficient balance." };
            }


            await _userBalanceService.DebitUserBalanceAsync(user.Id, request.TopUpAmount + 1);
            // Add to top-up history
            var topUpHistory = new TopUpHistory
            {
                UserId = user.Id,
                BeneficiaryId = beneficiary.Id,
                Amount = request.TopUpAmount,
                Date = DateTime.Now
            };
            await _topUpHistoryRepository.AddTopUpHistoryAsync(topUpHistory);

            // Perform the top-up transaction
            var httpClient = _httpClientFactory.CreateClient();
            var topUpApiResponse = await httpClient.PostAsJsonAsync("https://localhost/topup", new { Amount = request.TopUpAmount, PhoneNumber = beneficiary.PhoneNumber });

            if (!topUpApiResponse.IsSuccessStatusCode)
            {
                return new TopUpResult { Success = false, Message = "Top-up transaction failed." };
            }
            return new TopUpResult { Success = true, Message = "Top-up successful.", NewBalance = user.Balance };
        }
        public async Task<User> AddUserAsync(User user)
        {
            await _userRepository.AddUserAsync(user);
            return user;
        }
    }
}