using UserTopUpService.Domain;
namespace UserTopUpService.Repositories
{
    public interface ITopUpHistoryRepository
    {
        Task<int> GetTotalTopUpForUserThisMonth(int userId);
        Task<int> GetTotalTopUpForBeneficiaryThisMonth(int beneficiaryId);
        Task AddTopUpHistoryAsync(TopUpHistory topUpHistory);
    }
}