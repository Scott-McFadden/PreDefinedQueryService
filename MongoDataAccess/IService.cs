using System.Collections.Generic;
using MongoDB.Bson;

namespace MongoDataAccess
{
    public interface IService
    {
        public string name { get; set; }
        public string ConnectionString { get; set; }

        public string CollectionName { get;  set; }
        public bool IsServiceReady { get; set; }
        public BsonDocument GetOne(string criteria);
        public IList<BsonDocument> GetMany(string criteria);
        public BsonDocument AddOne (BsonDocument data);
        public bool ReplaceOne (BsonDocument data);
        public bool RemoveOne (BsonDocument data);
        public BsonDocument GetById (string id);
        public bool Validate();
    }
}
