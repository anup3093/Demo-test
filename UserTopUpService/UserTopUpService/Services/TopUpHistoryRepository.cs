using Dapper;
using UserTopUpService.DataBase;
using UserTopUpService.Domain;
using UserTopUpService.Repositories;

namespace UserTopUpService.Services
{
    public class TopUpHistoryRepository : ITopUpHistoryRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public TopUpHistoryRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<int> GetTotalTopUpForUserThisMonth(int userId)
        {
            var startOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var totalTopUp = await connection.QuerySingleOrDefaultAsync<int>(
                "SELECT SUM(Amount) FROM TopUpHistories WHERE UserId = @UserId AND Date >= @StartOfMonth",
                new { UserId = userId, StartOfMonth = startOfMonth });
            return totalTopUp;
        }

        public async Task<int> GetTotalTopUpForBeneficiaryThisMonth(int beneficiaryId)
        {
            try
            {
                using var connection = await _connectionFactory.CreateConnectionAsync();
                var totalTopUp = await connection.QuerySingleOrDefaultAsync<int?>(
                    @"SELECT SUM(Amount)
            FROM TopUpHistories
            WHERE BeneficiaryId = @BeneficiaryId
            AND strftime('%Y-%m', Date) = strftime('%Y-%m', 'now')",
                    new { BeneficiaryId = beneficiaryId });

                return totalTopUp ?? 0;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task AddTopUpHistoryAsync(TopUpHistory topUpHistory)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            await connection.ExecuteAsync(
                "INSERT INTO TopUpHistories (UserId, BeneficiaryId, Amount, Date) VALUES (@UserId, @BeneficiaryId, @Amount, @Date)",
                new { UserId = topUpHistory.UserId, BeneficiaryId = topUpHistory.BeneficiaryId, Amount = topUpHistory.Amount, Date = topUpHistory.Date });
        }
    }
}