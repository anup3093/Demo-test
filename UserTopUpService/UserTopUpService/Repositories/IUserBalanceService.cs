namespace UserTopUpService.Repositories
{
    public interface IUserBalanceService
    {
        Task<int> GetUserBalanceAsync(int userId);
        Task DebitUserBalanceAsync(int userId, int amount);
        Task CreditUserBalanceAsync(int userId, int amount);
    }
}