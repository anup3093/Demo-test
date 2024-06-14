using UserTopUpService.Domain;
namespace UserTopUpService.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetUserAsync(int userId);
        Task UpdateUserAsync(User user);
        Task AddUserAsync(User user);
    }
}
