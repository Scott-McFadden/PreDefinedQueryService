using MongoDB.Bson;
using PreDefinedQuereyBL.RequestModels.CommandRequestModels;
using System.Threading.Tasks;

namespace PreDefinedQuereyBL.Contracts.CommandHandlers
{
    public interface ISaveCollectionCommandHandler
    {
        Task<BsonDocument> SaveAsync(SaveCollectionRequestModel requestModel);
    }
}
