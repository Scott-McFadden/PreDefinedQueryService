﻿

using Newtonsoft.Json;

namespace DataModels
{
    /// <summary>
    /// provides information about a connection
    /// </summary>
    public class ConnectionModel
    {
        /// <summary>
        /// the name of the connection
        /// </summary>
        public string Name { get; set; } = "";
        /// <summary>
        /// a description for the connection
        /// </summary>
        public string description { get; set; } = "";
        /// <summary>
        /// The database engine time - mongo, sql, api, etc  
        /// a cooresponding handler needs to exist in data access execution method.
        /// </summary>
        public string engineType { get; set; } = "";
        /// <summary>
        /// database name
        /// </summary>
        public string dbName { get; set; } = "";
        /// <summary>
        /// how to connect to the data engine 
        /// </summary>
        public string connectionString { get; set; } = "";
        /// <summary>
        /// collection / table name  
        /// </summary>
        public string collectionName { get; set; } = "";
        /// <summary>
        /// list of unique key fields 
        /// </summary>
        public string[] uniqueKeys { get; set; } = { "" };
        /// <summary>
        /// schema for the object - used if strongly typed json objects are to be used. 
        /// </summary>
        public string schema { get; set; } = "";
        /// <summary>
        /// list of tags that help catagorize this in the list.
        /// </summary>
        public string[] tags { get; set; } = { "" };
        /// <summary>
        /// what is the user id
        /// </summary>
        public string UserId { get; set; } = "";
        /// <summary>
        /// what is the password for the given userid
        /// </summary>
        public string Password { get; set; } = "";
        /// <summary>
        /// how should authentication be handled
        /// - in connection
        /// - Active directory
        /// - none
        /// - jwt
        /// - users credentials
        /// - service account credentials 
        /// - special 
        /// </summary>
        public string AuthModel { get; set; } = "none";

        /// <summary>
        /// creates an instance of this object from the string provided.
        /// </summary>
        /// <param name="data">json string representing this object</param>
        /// <returns>new ConnectionModel object</returns>
        public static ConnectionModel deserialize(string data)
        {
            return JsonConvert.DeserializeObject<ConnectionModel>(data);
        }

        /// <summary>
        /// create a json string for the object.
        /// </summary>
        /// <param name="model">ConnectionModel</param>
        /// <returns>json string</returns>
        public static string  serialize(ConnectionModel model)
        {
            return JsonConvert.SerializeObject(model, Formatting.Indented);
        }
    }
}
