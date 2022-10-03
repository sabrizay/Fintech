using Fintech.Library.Entities.Models;
using System.Linq.Expressions;

namespace Fintech.Library.DataAccess.Concrete.Repository;

public interface IGenericRepository
{
    Task<T> GetByIdAsync<T>(int id);
    Task<BaseResponse<int>> InsertAsync<T>(T t);
    Task<BaseResponse<int>> UpdateAsync<T>(T t);
    Task<int> DeleteByIdAsync<T>(int id);
    Task<BaseResponse< IEnumerable<T>>> GetAllAsync<T>();
    Task<int> SaveRangeAsync<T>(IEnumerable<T> list);
    string LangCode { get; set; }
    public Task<BaseResponse<List<T>>> SelectAsync<T>(Expression<Func<T, bool>> where);
    public Task<BaseResponse<T>> FirstAsync<T>(Expression<Func<T, bool>> where);
    public Task<BaseResponse<bool>> ExistAsync<T>(Expression<Func<T, bool>> where);
  
}