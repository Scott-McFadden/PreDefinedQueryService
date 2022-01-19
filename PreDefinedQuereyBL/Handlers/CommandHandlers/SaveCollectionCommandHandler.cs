using MongoDataAccess;
using MongoDB.Bson;
using PreDefinedQuereyBL.Contracts.CommandHandlers;
using PreDefinedQuereyBL.RequestModels.CommandRequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreDefinedQuereyBL.Handlers.CommandHandlers
{
    public class SaveCollectionCommandHandler : ISaveCollectionCommandHandler
    {
        public async Task<BsonDocument> SaveAsync(SaveCollectionRequestModel requestModel)
        {
            BsonDocument doc = requestModel.ToBsonDocument();
            DataAccessServices.CollectionService.AddOne(doc);

            return doc;

        }

        
    }
}
