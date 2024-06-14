using Dapper;
using Microsoft.Data.Sqlite;

namespace UserBalance.DataBase
{
    public class DatabaseInitializer
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public DatabaseInitializer(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task InitializeAsync()
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            await connection.ExecuteAsync(
                @"
                 CREATE TABLE IF NOT EXISTS Users (
                    UserId INTEGER PRIMARY KEY,
                    Balance INTEGER NOT NULL,
                    IsVerified INTEGER NOT NULL,
                    PhoneNumber TEXT NOT NULL
                );");

        }
    }
}