using Dapper;
using FreeCourse.Shared.DTOs;
using Npgsql;
using System.Data;

namespace FreeCourse.Services.Discount.Services;

public class DiscountService : IDiscountService
{
    private readonly IConfiguration _configuration;
    private readonly IDbConnection _dbConnection;

    public DiscountService(IConfiguration configuration, IDbConnection dbConnection)
    {
        _configuration = configuration;
        _dbConnection = new NpgsqlConnection(_configuration.GetConnectionString("PostgreSql"));
    }



    public async Task<Response<NoContent>> Delete(int id)
    {
        var status = await _dbConnection.ExecuteAsync($"DELETE FROM discount WHERE id = {id}");

        return status > 0
               ? Response<NoContent>.Success(204)
               : Response<NoContent>.Fail("Discount is not found!", 404);
    }

    public async Task<Response<List<Entities.Discount>>> GetAllAsync()
    {
        var discounts = await _dbConnection.QueryAsync<Entities.Discount>("SELECT * FROM discount");

        return Response<List<Entities.Discount>>.Success(discounts.ToList(), 200);
    }

    public async Task<Response<Entities.Discount>> GetByCodeAndUserId(string code, string userId)
    {
        var discount = (await _dbConnection.QueryAsync<Entities.Discount>($"SELECT * FROM discount WHERE userid = {userId} AND code = {code}")).FirstOrDefault();

        return discount == null
               ? Response<Entities.Discount>.Fail("Discount is not found!", 404)
               : Response<Entities.Discount>.Success(discount, 200);
    }

    public async Task<Response<Entities.Discount>> GetByIdAsync(int id)
    {
        var discount = (await _dbConnection.QueryAsync<Entities.Discount>($"SELECT * FROM discount WHERE id = {id}")).SingleOrDefault();

        return discount == null
               ? Response<Entities.Discount>.Fail("Discount is not found!", 404)
               : Response<Entities.Discount>.Success(discount, 200);
    }

    public async Task<Response<NoContent>> Save(Entities.Discount discount)
    {
        var status = await _dbConnection.ExecuteAsync($"INSERT INTO discount (userid, rate, code) " +
                                                      $"VALUES({discount.UserId},{discount.Rate},{discount.Code})");

        return status > 0
               ? Response<NoContent>.Success(204)
               : Response<NoContent>.Fail("An error ocurred while adding!", 500);
    }

    public async Task<Response<NoContent>> Update(Entities.Discount discount)
    {
        var status = await _dbConnection.ExecuteAsync($"UPDATE discount SET " +
                                                      $"userid={discount.UserId}," +
                                                      $"code={discount.Code}," +
                                                      $"rate={discount.Rate} " +
                                                      $"WHERE id = {discount.Id}");

        return status > 0
               ? Response<NoContent>.Success(204)
               : Response<NoContent>.Fail("Discount is not found!", 404);
    }
}
