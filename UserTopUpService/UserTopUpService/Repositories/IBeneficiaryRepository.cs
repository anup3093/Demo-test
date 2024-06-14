using UserTopUpService.Domain;

namespace UserTopUpService.Repositories
{
    public interface IBeneficiaryRepository
    {
        Task<Beneficiary> GetBeneficiaryAsync(string nickname);
        Task<List<Beneficiary>> GetBeneficiariesAsync(int userId);
        Task AddBeneficiaryAsync(Beneficiary beneficiary);

    }
}
