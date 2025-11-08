using Dapper;
using System.Data;

namespace Dapper_Crud.Models
{
    public class CountryRepository : ICountryRepo
    {
        private readonly IDbConnection _db;

        public CountryRepository(IDbConnection db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Country>> GetAllAsync()
        {
            var procedure = "dbo.usp_GetCountries";
            return await _db.QueryAsync<Country>(
                procedure,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<Country?> GetByIdAsync(int id)
        {
            var query = "SELECT * FROM dbo.Country WHERE CountryId = @Id";
            return await _db.QueryFirstOrDefaultAsync<Country>(query, new { Id = id });
        }

        public async Task<int> CreateAsync(Country country)
        {
            var query = @"INSERT INTO dbo.Country (CountryName)
                          VALUES (@CountryName);
                          SELECT CAST(SCOPE_IDENTITY() as int)";
            return await _db.ExecuteScalarAsync<int>(query, country);
        }

        public async Task<bool> UpdateAsync(Country country)
        {
            var query = @"UPDATE dbo.Country 
                          SET CountryName = @CountryName, ModifiedAt = SYSUTCDATETIME()
                          WHERE CountryId = @CountryId";
            var rows = await _db.ExecuteAsync(query, country);
            return rows > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var query = "DELETE FROM dbo.Country WHERE CountryId = @Id";
            var rows = await _db.ExecuteAsync(query, new { Id = id });
            return rows > 0;
        }
    }
}
