using Dapper;
using Microsoft.AspNetCore.Connections;
using Microsoft.Data.Sqlite;

namespace UserTopUpService.DataBase
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
                    Id INTEGER PRIMARY KEY,
                    Balance INTEGER NOT NULL,
                    IsVerified INTEGER NOT NULL,
                    PhoneNumber TEXT NOT NULL
                );

                CREATE TABLE IF NOT EXISTS Beneficiaries (
                    Id INTEGER PRIMARY KEY,
                    UserId INTEGER NOT NULL,
                    Nickname TEXT NOT NULL,
                    FOREIGN KEY(UserId) REFERENCES Users(Id)
                );

            CREATE TABLE IF NOT EXISTS TopUpHistories (
                Id INTEGER PRIMARY KEY,
                UserId INTEGER NOT NULL,
                BeneficiaryId INTEGER NOT NULL,
                Amount INTEGER NOT NULL,
                Date TEXT NOT NULL,
                FOREIGN KEY(UserId) REFERENCES Users(Id),
                FOREIGN KEY(BeneficiaryId) REFERENCES Beneficiaries(Id)
            );");

        }
    }
}
