using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Fintech.Library.DataAccess.Util;
using Fintech.Library.Entities.Models;
using Fintech.Library.Utilities.Extensions;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Fintech.Library.DataAccess.Concrete.Repository;

public class DapperRepositoryBase : IGenericRepository
{

    private readonly IConfiguration _configuration;

    public string LangCode { get; set; } = "tr";

    public DapperRepositoryBase()
    {
        _configuration = DependencyInjectionExtensions.ServiceTool.ServiceProvider.GetService<IConfiguration>();

        var httpContextAccessor = DependencyInjectionExtensions.ServiceTool.ServiceProvider.GetService<IHttpContextAccessor>();
        if (httpContextAccessor != null)
        {
            var langHeader = httpContextAccessor.HttpContext?.Request?.Headers["Lang"];
            LangCode = !string.IsNullOrEmpty(langHeader) ? langHeader?.ToString().ToLower().Trim() : "tr";
        }
    }

    private IDbConnection CreateConnection()
    {
        var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        conn.Open();
        return conn;
    }


    public async Task<int> DeleteByIdAsync<T>(int id)
    {
        using var connection = CreateConnection();
        return await connection.ExecuteAsync($"UPDATE {typeof(T).GetTableName()} SET IsDeleted = 1 WHERE Id=@Id", new { Id = id });
    }

    public async Task<BaseResponse<IEnumerable<T>>> GetAllAsync<T>()
    {
        try
        {
            using var connection = CreateConnection();

            var rslt = await connection.QueryAsync<T>($"SELECT * FROM {typeof(T).GetTableName() ?? typeof(T).Name}");
            return await Task.FromResult(new BaseResponse<IEnumerable<T>>(rslt, true, rslt.Count()));
        }
        catch (Exception e)
        {

            return await Task.FromResult(new BaseResponse<IEnumerable<T>>(new List<T>(), false) { error = new Error { exception = e }, Success = false });
        }
       
    }

    public async Task<T> GetByIdAsync<T>(int id)
    {
        using var connection = CreateConnection();
        var result = await connection.QuerySingleOrDefaultAsync<T>($"SELECT * FROM {typeof(T).GetTableName()} WHERE Id=@Id", new { Id = id });
        return result;
    }

    public async Task<BaseResponse<int>> InsertAsync<T>(T t)
    {
        var insertQuery = GenerateInsertQuery<T>();

        using var connection = CreateConnection();
        try
        {
            int Id = await connection.ExecuteScalarAsync<int>(insertQuery, t);
            return await Task.FromResult(new BaseResponse<int>(Id, true));
        }
        catch (Exception e)
        {
            return await Task.FromResult(new BaseResponse<int> { error = new Error { exception = e }, Success = false });
        }

    }

    public async Task<int> SaveRangeAsync<T>(IEnumerable<T> list)
    {
        var inserted = 0;
        var query = GenerateInsertQuery<T>();
        using (var connection = CreateConnection())
        {
            inserted += await connection.ExecuteAsync(query, list);
        }

        return inserted;
    }

    public async Task<BaseResponse<int>> UpdateAsync<T>(T t)
    {
        var updateQuery = GenerateUpdateQuery<T>(t);

        using var connection = CreateConnection();

        try
        {
            int Id = await connection.ExecuteAsync(updateQuery, t);
            return await Task.FromResult(new BaseResponse<int>(Id, true));
        }
        catch (Exception e)
        {
            return await Task.FromResult(new BaseResponse<int> { error = new Error { exception = e }, Success = false });
        }

    }


    private static List<string> GenerateListOfProperties(IEnumerable<PropertyInfo> listOfProperties)
    {


        return (from prop in listOfProperties
                let attributes = prop.GetCustomAttributes(typeof(DescriptionAttribute), false)
                where (attributes.Length <= 0 || (attributes[0] as DescriptionAttribute)?.Description != "ignore")
                select prop.Name).ToList();
    }

    private static IEnumerable<PropertyInfo> GetProperties<T>() => typeof(T).GetProperties();
    private static string GenerateInsertQuery<T>()
    {
        var insertQuery = new StringBuilder($"INSERT INTO {typeof(T).GetTableName() ?? typeof(T).Name} ");

        insertQuery.Append('(');

        var properties = GenerateListOfProperties(GetProperties<T>().Where(w => w.Name != "Id"));
        properties.ForEach(prop => { insertQuery.Append($"[{prop}],"); });

        insertQuery
            .Remove(insertQuery.Length - 1, 1)
            .Append(") OUTPUT inserted.Id VALUES (");

        properties.ForEach(prop => { insertQuery.Append($"@{prop},"); });

        insertQuery
            .Remove(insertQuery.Length - 1, 1)
            .Append(')');

        return insertQuery.ToString();
    }
    private static string GenerateUpdateQuery<T>(T t)
    {
        var updateQuery = new StringBuilder($"UPDATE {typeof(T).GetTableName() ?? typeof(T).Name} SET ");
        var properties = GenerateListOfProperties(GetProperties<T>());

        properties.ForEach(property =>
        {
            if (!property.Equals("Id") && typeof(T).GetProperty(property).GetValue(t, null) != null)
            {
                updateQuery.Append($"{property}=@{property},");
            }
        });

        updateQuery.Remove(updateQuery.Length - 1, 1); //remove last comma
        updateQuery.Append(" WHERE Id=@Id");

        return updateQuery.ToString();
    }

    public async Task<BaseResponse<List<T>>> SelectAsync<T>(Expression<Func<T, bool>> where)
    {
        try
        {
            var properties = ParsePropertiesExpression(where);
            var sqlPairs = GetSqlPairs(properties.AllNames, " AND ", where);
            var sql = string.Format("SELECT * FROM [{0}] WHERE {1}", typeof(T).GetTableName() ?? typeof(T).Name, sqlPairs);
            //sql += string.Format(" AND IsDeleted =  {0} ", IsDeleted ? 1 : 0);
            var List = GetItems<T>(CommandType.Text, sql, properties.AllPairs).ToList();
            var rowCount = List.Count;
            return await Task.FromResult(new BaseResponse<List<T>>(List, true, rowCount));
        }
        catch (Exception e)
        {
            return await Task.FromResult(new BaseResponse<List<T>>(new List<T>(), false) { error = new Error { exception = e }, Success = false });
        }


    }

    public async Task<BaseResponse<bool>> ExistAsync<T>(Expression<Func<T, bool>> where)
    {
        try
        {
            var properties = ParsePropertiesExpression(where);
            var sqlPairs = GetSqlPairs(properties.AllNames, " AND ", where);
            var sql = string.Format("SELECT  Count(*) FROM [{0}] WHERE {1}", typeof(T).GetTableName() ?? typeof(T).Name, sqlPairs);

            using var connection = CreateConnection();

            var rslt = connection.QuerySingle<bool>(sql, properties.AllPairs);
            return await Task.FromResult(new BaseResponse<bool>(rslt, true));
        }
        catch (Exception e)
        {
            return await Task.FromResult(new BaseResponse<bool> { error = new Error { exception = e }, Success = false, Data = false });
        }


    }

    public async Task<BaseResponse<T>> FirstAsync<T>(Expression<Func<T, bool>> where)
    {
        try
        {
            var properties = ParsePropertiesExpression(where);
            var sqlPairs = GetSqlPairs(properties.AllNames, " AND ", where);
            var sql = string.Format("SELECT top 1 * FROM [{0}] WHERE {1}", typeof(T).GetTableName() ?? typeof(T).Name, sqlPairs);
            //  sql += string.Format(" AND IsDeleted =  {0} ", IsDeleted ? 1 : 0);
            var rslt = GetItems<T>(CommandType.Text, sql, properties.AllPairs).FirstOrDefault();

            return await Task.FromResult(new BaseResponse<T>(rslt, true, rowCount: (rslt != null) ? 1: 0));
        }
        catch (Exception e)
        {
            return await Task.FromResult(new BaseResponse<T>(default, false) { error = new Error { exception = e }, Success = false });
        }
    }
    private static PropertyContainer ParsePropertiesExpression<T>(Expression<Func<T, bool>> expression = null)
    {
        var propertyContainer = new PropertyContainer();

        var Exp = new DapperExpression(expression);
        var filters = Exp.Build().ToList();

        foreach (var filter in filters)
        {
            if (filter.FieldName == null) continue;
            string fieldName = filter.FieldName.Split(".")[1];
            fieldName = (!propertyContainer.ValuePairs.ContainsKey(fieldName)) ? fieldName : fieldName + fieldName.GetHashCode().ToString().Replace("-", "");
            propertyContainer.AddValue(fieldName, filter.Value);
        }
        return propertyContainer;
    }

    private class PropertyContainer
    {
        private readonly Dictionary<string, object> _ids;
        private readonly Dictionary<string, object> _values;

        #region Properties

        internal IEnumerable<string> IdNames
        {
            get { return _ids.Keys; }
        }

        internal IEnumerable<string> ValueNames
        {
            get { return _values.Keys; }
        }

        internal IEnumerable<string> AllNames
        {
            get { return _ids.Keys.Union(_values.Keys); }
        }

        internal IDictionary<string, object> IdPairs
        {
            get { return _ids; }
        }

        internal IDictionary<string, object> ValuePairs
        {
            get { return _values; }
        }

        internal IEnumerable<KeyValuePair<string, object>> AllPairs
        {
            get { return _ids.Concat(_values); }
        }

        #endregion

        #region Constructor

        internal PropertyContainer()
        {
            _ids = new Dictionary<string, object>();
            _values = new Dictionary<string, object>();
        }

        #endregion

        #region Methods

        internal void AddId(string name, object value)
        {
            _ids.Add(name, value);
        }

        internal void AddValue(string name, object value)
        {
            _values.Add(name, value);
        }

        #endregion
    }

    private static string GetSqlPairs
  (IEnumerable<string> keys, string separator = ", ", Expression where = null)
    {
        var pairs = new List<string>();
        if (where == null)
        {
            pairs = keys.Select(key => string.Format("{0}=@{0}", key)).ToList();
        }
        else
        {
            var Exp = new DapperExpression(where);
            var filters = Exp.Build().ToList();


            foreach (var filter in filters)
            {
                if (filter.FieldName == null) continue;
                string fieldName = filter.FieldName.Split(".")[1];
                fieldName = (!pairs.Contains(fieldName)) ? fieldName : fieldName + fieldName.GetHashCode().ToString().Replace("-", "");
                pairs.Add(string.Format("{0}" + filter.StringOperator + "@{1}", filter.FieldName.Split(".")[1], fieldName));
            }
            // pairs = keys.Select(key => string.Format("{0}=@{0}", key)).ToList();
        }

        return string.Join(separator, pairs);
    }

    private IEnumerable<T>
    GetItems<T>(CommandType commandType, string sql, object parameters = null)
    {
        using var connection = CreateConnection();
        var con = connection.Query<T>(sql, parameters, commandType: commandType);
        return con;
    }


}

