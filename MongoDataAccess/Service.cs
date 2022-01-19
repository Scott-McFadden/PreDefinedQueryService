using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq;
using DataModels;

namespace MongoDataAccess
{
    public class Service : IService
    {
        public string name { get; set; }
        public string CollectionName { get; set; }
        public MongoClientSettings Settings { get; private set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; private set; }
        public IMongoClient Client { get; private set; }
        public IMongoDatabase Database { get; set; }
        public IMongoCollection<BsonDocument> Collection { get; private set; }
        public string UserName { get; set; } = "";
        public string Password { get; set; } = "";
        public bool IsServiceReady { get; set; } = false;


        public static Service MakeConnection(ConnectionModel connectionModel)
        {
            return Service.MakeConnection(
                connectionModel.name, 
                connectionModel.connectionString, 
                connectionModel.dbName, 
                connectionModel.collectionName, 
                connectionModel.UserId, 
                connectionModel.Password);
        }
        /// <summary>
        /// builds a mongo collection service class based on the paramaters provided
        /// </summary>
        /// <param name="connectionString">mongo connnection string</param>
        /// <param name="databaseName">name of database</param>
        /// <param name="collectionName">name of collection</param>
        /// <param name="userName">user id = defaults to "" </param>
        /// <param name="password">password - can not be blank if userName is provided.</param>
        /// <returns>instance of the Service Class.</returns>
        public static Service MakeConnection(string Name, string connectionString, string databaseName, string collectionName, string userName = "", string password = "")
        {
            // validate 
            if (!userName.Equals(""))
                if (password.Equals(""))
                    throw new Exception("Password may not be blank if username is populated");

            if (String.IsNullOrEmpty(connectionString))
                throw new Exception("connection string cannot be empty");

            if (String.IsNullOrEmpty(collectionName))
                throw new Exception("connection name must be populated");

            if (String.IsNullOrEmpty(databaseName))
                throw new Exception("databaseName name must be populated");

            if (String.IsNullOrEmpty(Name))
                throw new Exception("Name cannot be empty");

            // create class

            Service ret = new Service();
            ret.ConnectionString = connectionString; // + connectionString[connectionString.Length-1] == "/" ? "" : "/"; //add a closing / if it does not exist.
            ret.DatabaseName = databaseName;
            ret.CollectionName = collectionName;
            ret.name = Name;

            // build settings

            ret.Settings = MongoClientSettings.FromConnectionString(connectionString );
            if (!String.IsNullOrEmpty(userName))
            {
                ret.Settings.Credential = MongoCredential.CreateCredential(databaseName, userName, password);
            }

            ret.Client = new MongoClient(ret.Settings);

            ret.Database = ret.Client.GetDatabase(databaseName);
            ret.Collection = ret.Database.GetCollection<BsonDocument>(collectionName);
            ret.IsServiceReady = true;

            return ret;
        }

        public  BsonDocument   AddOne (BsonDocument data)
        {
            if (!this.IsServiceReady)
                throw new Exception("the service has not been initialized yet.");

            try
            {
                Collection.InsertOne(data);
            }
            catch (Exception ex)
            {
                throw new Exception("Service.AddOne :  Could not execute Insert One", ex);
            }
            return data;
        }

        public BsonDocument GetById(string id)
        {
            if (!this.IsServiceReady)
                throw new Exception("the service has not been initialized yet.");

            BsonDocument ret = Collection.Find(id.FilterById()).FirstOrDefault();
            return ret;
        }

        public IList<BsonDocument> GetMany(string criteria)
        {
            if (!this.IsServiceReady)
                throw new Exception("the service has not been initialized yet.");

            var filter = new BsonDocument();
            try
            {
                filter = BsonDocument.Parse(criteria);
            }
            catch (Exception ex)
            {
                throw new Exception("The criteria is not correct", ex);
            }
            var ret = Collection.Find(filter).ToList<BsonDocument>();

            return ret;
        }

        public BsonDocument GetOne(string criteria)
        {
            if (!this.IsServiceReady)
                throw new Exception("the service has not been initialized yet.");
            var filter = new BsonDocument();
            try
            {
                filter =BsonDocument.Parse(criteria);  
            }
            catch (Exception ex)
            {
                throw new Exception("The criteria is not correct", ex);
            }
            var ret = Collection.Find(filter).FirstOrDefault();

            return ret;
        }

        public bool RemoveOne(BsonDocument data)
        {
            if (!this.IsServiceReady)
                throw new Exception("the service has not been initialized yet.");
            var ret = false;
            var filter = new BsonDocument { { "_id", ObjectId.Parse(data.GetId()) } };
            var result = Collection.DeleteOne(filter);
            if (result.DeletedCount == 1)
            {
                ret = true;
            }
            return ret;
        }

        public bool ReplaceOne(BsonDocument data)
        {
            if (!this.IsServiceReady)
                throw new Exception("the service has not been initialized yet.");
            var ret = false;
            var filter = new BsonDocument { { "_id", ObjectId.Parse(data.GetId()) } };

            var result = Collection.ReplaceOne(filter, data, new ReplaceOptions { IsUpsert = false } );
            if (result.ModifiedCount == 1)
                ret = true;

            return ret;
        }

        /// <summary>
        /// verify that the model is usable
        /// </summary>
        /// <returns>boolean</returns>
        public bool Validate()
        {
            if (
                String.IsNullOrEmpty(name) ||
                String.IsNullOrEmpty(CollectionName) ||
                String.IsNullOrEmpty(ConnectionString) ||
                String.IsNullOrEmpty(DatabaseName))
                return false;
            return true;
        }
    }
}
