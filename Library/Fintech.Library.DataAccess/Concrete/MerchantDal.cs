using Fintech.Library.DataAccess.Concrete.Repository;
using Fintech.Library.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fintech.Library.DataAccess.Concrete
{
    public class MerchantDal : DapperRepositoryBase, IMerchantDal
    {
        private readonly IDbConnection _dbConnection;
        public MerchantDal(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        //public async Task<IEnumerable<MerchantDto>> GetAllAsync(int pageIndex, int pageSize)
        //{
        //    string sqlQuery = "sp_MerchantAPI_MerchantGetAll";
        //    var p = new
        //    {
        //        PageIndex = pageIndex,
        //        PageSize = pageSize,
        //    };

        //    var result = await _dbConnection.QueryAsync<MerchantDto>(
        //        sql: sqlQuery,
        //        param: p,
        //        commandType: CommandType.StoredProcedure);

        //    return result;
        //}



        //public async Task<MerchantDto> GetById(int merchantId)
        //{
        //    var resultData = await this.SelectAsync<MerchantDto>(x => x.Id == merchantId);

        //    if (resultData != null && resultData.Data.Any())
        //        return resultData.Data.FirstOrDefault();
        //    else return new MerchantDto();

        //    // return await _dbConnection.QueryFirstOrDefaultAsync<MerchantDto>(
        //    //sql: $"SELECT * FROM  {typeof(MerchantDto).GetTableName()} WITH(NOLOCK) WHERE IsDeleted = 0 and Id = @MerchantId",
        //    //param: new { MerchantId = merchantId });
        //}
        
        //public async Task<MerchantDto> GetByParameters(string emailAdress, string taxId)
        //{
        //    if(_dbConnection.State != ConnectionState.Open)
        //        _dbConnection.Open();
        //    var sqlQuery = "SELECT * FROM Merchants WITH(NOLOCK) WHERE ContactEmail = @EmailAdress OR TaxNumber = @TaxId";
        //    var result = await _dbConnection.QuerySingleOrDefaultAsync<MerchantDto>(
        //        sql: sqlQuery,
        //        param: new
        //        {
        //            EmailAdress= emailAdress,
        //            TaxId = taxId
        //        },
        //        commandType:CommandType.Text
        //        );
        //    return result;
        //}
    }
}
