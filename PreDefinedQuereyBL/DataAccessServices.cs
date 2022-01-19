using DataModels;
using MongoDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreDefinedQuereyBL
{
    public static class DataAccessServices
    {
        static DataAccessServices()
        {

        }


        private static ConnectionModel GetCollectionDef(string name)
        {
            return ConnectionList.Where(a => a.name == name).First();
        }
        private static List<ConnectionModel> ConnectionList = new List<ConnectionModel>();

        private static Service collectionService = null;
        public static Service CollectionService {
            get {
                if (collectionService == null)
                {
                    var cdef = DataAccessServices.GetCollectionDef("collections");
                    collectionService = Service.MakeConnection(
                        cdef.connectionString,
                        cdef.dbName,
                        cdef.collectionName,
                        cdef.UserId,
                        cdef.Password);
                }
                return collectionService;
            }
        }
    }
}
