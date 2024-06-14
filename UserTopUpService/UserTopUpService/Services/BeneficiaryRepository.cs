using Dapper;
using UserTopUpService.DataBase;
using UserTopUpService.Domain;
using UserTopUpService.Repositories;

namespace UserTopUpService.Services
{
    public class BeneficiaryRepository : IBeneficiaryRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public BeneficiaryRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<Beneficiary?> GetBeneficiaryAsync(string nickname)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            return await connection.QuerySingleOrDefaultAsync<Beneficiary>(
                "SELECT * FROM Beneficiaries WHERE Nickname = @Nickname", new { Nickname = nickname });
        }
        public async Task<List<Beneficiary>> GetBeneficiariesAsync(int userId)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var beneficiaries = await connection.QueryAsync<Beneficiary>(
                "SELECT * FROM Beneficiaries WHERE UserId = @userId LIMIT 1", new { userId });
            return beneficiaries.ToList();
        }
        public async Task AddBeneficiaryAsync(Beneficiary beneficiary)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            await connection.ExecuteAsync(
                "INSERT INTO Beneficiaries (Nickname, UserId) VALUES (@Nickname, @UserId)",
                new { Nickname = beneficiary.Nickname, UserId = beneficiary.UserId });
        }


    }
}