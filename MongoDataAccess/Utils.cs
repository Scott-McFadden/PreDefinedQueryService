using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace MongoDataAccess
{
    public static class MongoExtensions
    {
        /// <summary>
        /// returns a filter document based on searching the _id of a mongo document.
        /// </summary>
        /// <param name="id">24 byte string representing an object id</param>
        /// <returns>bsone document that can be used to filter for the given _id</returns>
        public static FilterDefinition<BsonDocument> FilterById(this string id)
        {
            return Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(id));
        }

        /// <summary>
        /// gets the id from a mongo document
        /// </summary>
        /// <param name="doc">document to extract the id from</param>
        /// <returns>string representing the object id</returns>
        public static string GetId(this BsonDocument doc)
        {
            return doc.GetElement("_id").Value.ToString();
        }
        /// <summary>
        /// simple way to convert an object to a json string.
        /// </summary>
        /// <param name="x">object to be converted</param>
        /// <returns>json string </returns>
        public static string ToJsonString(this object x)
        {
            return JsonConvert.SerializeObject(x);
        }
    }
}
