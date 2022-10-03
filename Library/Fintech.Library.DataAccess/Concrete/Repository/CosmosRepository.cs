using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using System.Net;

namespace Fintech.Library.DataAccess.Concrete.Repository;

public class CosmosRepository : ICosmosRepository
{
    private readonly CosmosClient cosmosClient;
    private readonly Database database;
    private readonly string _EndpointUri;
    private readonly string _PrimaryKey;
    private readonly string _CosmosDbName;
    //private readonly string _CosmosContainerName;
    private Container container;
    private readonly IConfiguration _config;

    public CosmosRepository(IConfiguration config, string EndpointUri = null, string PrimaryKey = null, string CosmosDbName = null)
    {
        _config = config;
        _EndpointUri = EndpointUri ?? config.GetSection("EndPointUri").Value;
        _PrimaryKey = PrimaryKey ?? config.GetSection("PrimaryKey").Value;
        _CosmosDbName = CosmosDbName ?? config.GetSection("CosmosDbName").Value;

        this.cosmosClient = new CosmosClient(_EndpointUri, _PrimaryKey);

        //    database = cosmosClient.CreateDatabaseIfNotExistsAsync(_CosmosDbName).Result.Database;

    }
     
    public async Task<ItemResponse<T>> AddItemsToContainerAsync<T>(T Model)
    {
        ItemResponse<T> ModelResponse;

        try
        {

            ModelResponse = await container.ReadItemAsync<T>(typeof(T).GetProperty("Id").GetValue(Model, null).ToString(), new PartitionKey(typeof(T).GetProperty("Id").GetValue(Model, null).ToString()));
        }
        catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            ModelResponse = await container.CreateItemAsync<T>(Model);
        }
        catch (Exception ex)
        {
            return default;
        }
        return ModelResponse;
    }
    public async Task<List<ItemResponse<T>>> AddItemsToContainerAsync<T>(List<T> list)
    {

        List<ItemResponse<T>> Responses = new();

        ItemResponse<T> ModelResponse;




        foreach (var item in list)
        {

            try
            {

                ModelResponse = await container.ReadItemAsync<T>(typeof(T).GetProperty("Id").GetValue(item, null).ToString(), new PartitionKey(typeof(T).GetProperty("Id").GetValue(item, null).ToString()));

                Responses.Add(ModelResponse);
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                ModelResponse = await container.CreateItemAsync<T>(item);
                Responses.Add(ModelResponse);
            }
        }




        return Responses;
    }


    public async Task<ContainerResponse> DeleteContainerAsync(string ContainerName)
    {

        Container container = database.GetContainer(ContainerName);
        return await container.DeleteContainerAsync();
    }
    public async Task BeginCosmos(string ContainerName, string PartionKey)
    {
        ContainerResponse containerResponse;

        containerResponse = await database.CreateContainerIfNotExistsAsync(ContainerName, PartionKey);
        this.container = containerResponse.Container;
    }
    public async Task<List<T>> QueryItemsAsync<T>(string expression)
    {

        string Sql = "Select * from c  " + (string.IsNullOrEmpty(expression) ? "" : " where " + expression);
        QueryDefinition queryDefinition = new(Sql);
        FeedIterator<T> queryResultSetIterator = this.container.GetItemQueryIterator<T>(queryDefinition);
        List<T> TList = new List<T>();
        while (queryResultSetIterator.HasMoreResults)
        {
            FeedResponse<T> currentResultSet = await queryResultSetIterator.ReadNextAsync();
            foreach (T TResult in currentResultSet)
            {
                TList.Add(TResult);
            }
        }
        return TList;
    }

}
