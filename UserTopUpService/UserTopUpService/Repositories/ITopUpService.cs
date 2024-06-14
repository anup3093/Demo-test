using UserTopUpService.Domain;

namespace UserTopUpService.Repositories
{
    public interface ITopUpService
    {

        Task<Beneficiary> AddBeneficiaryAsync(Beneficiary request);
        Task<List<Beneficiary>> GetBeneficiariesAsync(int userId);
        List<int> GetTopUpOptions();
        Task<TopUpResult> TopUpAsync(TopUpRequest request);
        Task<User> AddUserAsync(User user);


    }
}