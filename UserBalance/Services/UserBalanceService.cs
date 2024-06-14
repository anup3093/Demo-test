using UserBalance.DataBase;
using UserBalance.Repositories;
using Dapper;
using UserBalance.Domain;
namespace UserBalance.Services
{
    public class UserBalanceService : IUserBalanceService
    {

        private readonly IDbConnectionFactory _connectionFactory;
        public UserBalanceService(IDbConnectionFactory connectionFactory)
        {

            _connectionFactory = connectionFactory;
        }

        public async Task<int> GetUserBalanceAsync(int userId)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var query = "SELECT Balance FROM Users WHERE UserId = @UserId";
            var balance = await connection.QuerySingleOrDefaultAsync<int?>(query, new { UserId = userId });
            return balance ?? 0;
        }
        public async Task CreditUserBalanceAsync(int userId, int amount)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var query = "UPDATE Users SET Balance = Balance + @Amount WHERE UserId = @UserId";
            await connection.ExecuteAsync(query, new { UserId = userId, Amount = amount });
        }

        public async Task DebitUserBalanceAsync(int userId, int amount)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var query = "UPDATE Users SET Balance = Balance - @Amount WHERE UserId = @UserId";
            await connection.ExecuteAsync(query, new { UserId = userId, Amount = amount });
        }

        public async Task AddUserAsync(User user)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            await connection.ExecuteAsync(
                "INSERT INTO Users (UserId, Balance, IsVerified, PhoneNumber) VALUES (@UserId, @Balance, @IsVerified, @PhoneNumber)",
                new { UserId = user.Id, Balance = user.Balance, IsVerified = user.IsVerified, PhoneNumber = user.PhoneNumber });
        }

    }



}