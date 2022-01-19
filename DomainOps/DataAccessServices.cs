using DataModels;
using MongoDataAccess;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainOps
{
    public static class DAS
    {
        /// <summary>
        /// data Access Service
        /// </summary>
        static DAS() 
        {
            // TODO:  Get details from configuration file.
            DAS.ConnectionList.Add( "collections",  new ConnectionModel
            {
                name = "collections",
                connectionString = "mongodb://localhost:27017/",
                dbName = "querydefs",
                collectionName = "connections",
                engineType = "mongo",
                uniqueKeys = new string[] {"name"},
                tags = new string[] { "collections", "system" }
            });

            DAS.LoadCollections();
            DAS.LoadQueryDefs();
        }


        private static Dictionary<String, QueryDefModel> QueryDefList = new Dictionary<string, QueryDefModel>();
        private static Dictionary<String, ConnectionModel> ConnectionList = new Dictionary<String, ConnectionModel>();
        private static Dictionary<String, Service> serviceList = new Dictionary<String, Service>();
        private static Service collectionService = null;
        public static Service CollectionService {
            get {
                if (collectionService == null)
                {
                    var cdef = DAS.GetCollectionDefinition("collections");
                    collectionService = Service.MakeConnection(
                        cdef.name,
                        cdef.connectionString,
                        cdef.dbName,
                        cdef.collectionName,
                        cdef.UserId,
                        cdef.Password);

                    DAS.AddService(DAS.collectionService);
                }
                return collectionService;
            }
        }

        public static Service GetService(string name)
        {
            if (!serviceList.ContainsKey(name))
            {
                if (!DAS.ConnectionList.ContainsKey(name))
                    throw new Exception("Cannot find key " + name + " in ConnectionList");
                var c = DAS.GetCollectionDefinition(name);
                Service s = Service.MakeConnection(c);
                DAS.AddService(s);
            }
            return serviceList[name];
             
        }

        public static void AddService(Service connection)
        {
            if (!serviceList.ContainsKey(connection.name))
            {
                if (!connection.Validate())
                    throw new Exception("Cannot add connectionModel because it is not valid.");

                serviceList.Add(
                    connection.name,
                    Service.MakeConnection(
                        connection.name,
                        connection.ConnectionString,
                        connection.DatabaseName,
                        connection.CollectionName,
                        connection.UserName, 
                        connection.Password
                        )
                    );
            }
        }

        public static ConnectionModel GetCollectionDefinition(string name)
        {
            return ConnectionList[name];
        }

        public static void AddCollectionDefinition(ConnectionModel model)
        {
            if (model.Validate())
            {
                ConnectionList.Add(model.name, model);
                serviceList.Add(
                    model.name,
                    Service.MakeConnection(
                        model.name,
                        model.connectionString,
                        model.dbName,
                        model.collectionName,
                        model.UserId,
                        model.Password
                        )
                    );
            }    
            else
                throw new Exception("ConnectionModel Not Valid");
        }

        public static void AddQueryDefinition(QueryDefModel model)
        {
            if (model.Validate())
                QueryDefList.Add(model.name, model);
            else
                throw new Exception("QueryDefModel Not Valid");
        }
        /// <summary>
        /// Gets a particular querydef from cache
        /// </summary>
        /// <param name="name">name of querydef model</param>
        /// <returns>D</returns>
        public static QueryDefModel GetQueryDefinition(String name)
        {
            return QueryDefList[name];
        }


        public static void LoadCollections()
        {
            var docs = DAS.CollectionService.GetMany("{}");
            foreach(BsonDocument doc in docs)
            {

                var n = doc.ToJson3();
                var x = ConnectionModel.deserialize(n);
                DAS.AddCollectionDefinition(x);
            }
        }

        public static void LoadQueryDefs()
        {
             
            var docs = DAS.GetService("querydefs").GetMany("{}");
            foreach (BsonDocument doc in docs)
            {

                var n = doc.ToJson3();
                var x = QueryDefModel.deserialize(n);
                
                DAS.AddQueryDefinition(x); 

            }
        }
    }
}
