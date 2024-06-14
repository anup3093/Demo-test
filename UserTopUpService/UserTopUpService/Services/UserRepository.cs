using UserTopUpService.DataBase;
using UserTopUpService.Domain;
using Dapper;
using UserTopUpService.Repositories;

namespace UserTopUpService.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public UserRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<User?> GetUserAsync(int userId)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var query = @"
                        SELECT *
                        FROM Users u
                        LEFT JOIN Beneficiaries b ON u.Id = b.UserId
                        WHERE u.Id = @Id";

            var userDictionary = new Dictionary<int, User>();

            await connection.QueryAsync<User, Beneficiary, User>(query,
                (user, beneficiary) =>
                {
                    if (!userDictionary.TryGetValue(user.Id, out var userEntry))
                    {
                        userEntry = user;
                        userEntry.Beneficiaries = new List<Beneficiary>();
                        userDictionary.Add(userEntry.Id, userEntry);
                    }

                    if (beneficiary != null)
                    {
                        userEntry.Beneficiaries.Add(beneficiary);
                    }

                    return userEntry;
                },
                new { Id = userId });

            return userDictionary.Values.FirstOrDefault();
        }



        public async Task UpdateUserAsync(User user)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var result = await connection.ExecuteAsync(
                @"UPDATE Users SET Balance = @Balance WHERE Id = @Id",
                user);
        }
        public async Task AddUserAsync(User user)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            await connection.ExecuteAsync(
                "INSERT INTO Users (Id, Balance, IsVerified, PhoneNumber) VALUES (@Id, @Balance, @IsVerified, @PhoneNumber)",
                new { Id = user.Id, Balance = user.Balance, IsVerified = user.IsVerified, PhoneNumber = user.PhoneNumber });
        }
    }

}
