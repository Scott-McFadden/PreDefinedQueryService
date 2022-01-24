using DataModels;
using MongoDataAccess;
using MongoDB.Bson;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DomainOps
{
    public class ExecuteQuery
    {
        public ExecuteQuery()
        {

        }

        #region GetOne
        public JObject GetOne(string QueryDefName, JObject parameters)
        {
            QueryDefModel qd = DAS.GetQueryDefinition(QueryDefName);
            if (!qd.canGet)
                throw new Exception("The query definition does not support get operations");
            ConnectionModel cm = DAS.GetCollectionDefinition(qd.connection);

            var criteria = qd.getOneQuery.ResolveCriteria(parameters);

            if (cm.engineType == "mongo")
                return GetOneMongo(qd, cm, criteria);
            else if (cm.engineType == "mssql")
                return GetSQL(qd, cm, parameters);
            else
                throw new Exception("Execute Query - engineType (" + cm.engineType + ") was not defined");
        }
        private JObject GetOneMongo(QueryDefModel qd, ConnectionModel cm, string criteria)
        {
            var result = DAS.GetService(cm.name).GetOne(criteria);
            if (result == null)
                return JObject.Parse("[{}]");

            var fields = GetFields(qd);
            return SelectFields(result, fields);
        }

        #endregion

        #region Get
        public JObject Get(string QueryDefName, JObject parameters )
        {
            QueryDefModel qd = DAS.GetQueryDefinition(QueryDefName);
            ConnectionModel cm = DAS.GetCollectionDefinition(qd.connection);
            string criteria;

            criteria = qd.getQuery.ResolveCriteria(parameters);
            
            if (cm.engineType == "mongo")
                return GetMongo(qd, cm, criteria);
            else if (cm.engineType == "mssql")
                return GetSQL(qd, cm, parameters);
            else
                throw new Exception("Execute Query - engineType (" + cm.engineType + ") was not defined");
        }

        public JObject Get(string QueryDefName)
        {
            QueryDefModel qd = DAS.GetQueryDefinition(QueryDefName);
            ConnectionModel cm = DAS.GetCollectionDefinition(qd.connection); 

            if (cm.engineType == "mongo")
                return GetMongo(qd, cm, "{}");
            else if (cm.engineType == "mssql")
                return GetSQL(qd, cm, new JObject());
            else
                throw new Exception("Execute Query - engineType (" + cm.engineType + ") was not defined");
        }

        /// <summary>
        /// querydef - get - override querydef.criteria 
        /// </summary>
        /// <param name="QueryDefName">name of queryDef</param>
        /// <param name="criteria">The Query Parameters to use</param>
        /// <returns>Jobject of the items</returns>
        public JObject Get(string QueryDefName, string criteria)
        {
            QueryDefModel qd = DAS.GetQueryDefinition(QueryDefName);
            if (!qd.canGet)
                throw new Exception("The query definition does not support get operations");

            ConnectionModel cm = DAS.GetCollectionDefinition(qd.connection);

            if (cm.engineType == "mongo")
                return GetMongo(qd, cm, criteria);
            else if (cm.engineType == "mssql")
                return GetSQL(qd, cm, criteria);
            else
                throw new Exception("Execute Query - engineType (" + cm.engineType + ") was not defined");
        }

        private JObject GetMongo(QueryDefModel qd, ConnectionModel cm, string criteria)
        {
            if (String.IsNullOrEmpty(criteria))
                criteria = "{}";
            var result = DAS.GetService(cm.name).GetMany(criteria);
            if (result == null)
                return JObject.Parse("[{}]");
            JArray j = new();
            var fields = GetFields(qd);
            foreach (var d in result)
                j.Add(SelectFields(d, fields));

            JObject ret = new();
            ret.Add("docs", j);
            return ret;
        }

        private JObject GetSQL(QueryDefModel qd, ConnectionModel cm, JObject criteria)
        {
            throw new NotImplementedException();
        }
        private JObject GetSQL(QueryDefModel qd, ConnectionModel cm, string criteria)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region GetById
        public JObject GetById(string QueryDefName, string id)
        {
            QueryDefModel qd = DAS.GetQueryDefinition(QueryDefName);
            if (!qd.canGet)
                throw new Exception("The query definition does not support get operations");

            ConnectionModel cm = DAS.GetCollectionDefinition(qd.connection);

            if (cm.engineType == "mongo")
                return GetByIdMongo(qd, cm, id);
            else if (cm.engineType == "mssql")
                return GetByIdSQL(qd, cm, id);
            else
                throw new Exception("Execute Query - engineType (" + cm.engineType + ") was not defined");
        }

        private JObject GetByIdSQL(QueryDefModel qd, ConnectionModel cm, string id)
        {
            throw new NotImplementedException();
        }

        private JObject GetByIdMongo(QueryDefModel qd, ConnectionModel cm, string id)
        {
            var result = DAS.GetService(cm.name).GetById(id);
            if (result == null)
                return JObject.Parse("[{}]");

            var fields = GetFields(qd);
            return SelectFields(result, fields);
        }


        #endregion

        #region AddOne
        public JObject AddOne(string QueryDefName, string doc)
        {
            QueryDefModel qd = DAS.GetQueryDefinition(QueryDefName);
            if (!qd.canAdd)
                throw new Exception("The query definition does not support add operations");
            ConnectionModel cm = DAS.GetCollectionDefinition(qd.connection);

            if (cm.engineType == "mongo")
                return AddOneMongo(qd, cm, doc);
            else if (cm.engineType == "mssql")
                return GetSQL(qd, cm, doc);
            else
                throw new Exception("Execute Query - engineType (" + cm.engineType + ") was not defined");
        }

        private JObject AddOneMongo(QueryDefModel qd, ConnectionModel cm, string doc)
        {
            var result = DAS.GetService(cm.name).AddOne(BsonDocument.Parse(doc));
            if (result == null)
                return JObject.Parse("[{}]");
            
            return result.ToJObject();
        }

        #endregion
        #region ReplaceOne
        public bool ReplaceOne(string QueryDefName, string doc)
        {
            QueryDefModel qd = DAS.GetQueryDefinition(QueryDefName);
            if (!qd.canUpdate)
                throw new Exception("The query definition does not support update operations");
            ConnectionModel cm = DAS.GetCollectionDefinition(qd.connection);

            if (cm.engineType == "mongo")
                return ReplaceOneMongo(qd, cm, doc);
            else if (cm.engineType == "mssql")
                return ReplaceOneSQL(qd, cm, doc);
            else
                throw new Exception("Execute Query - engineType (" + cm.engineType + ") was not defined");
        }

        private bool ReplaceOneSQL(QueryDefModel qd, ConnectionModel cm, string doc)
        {
            throw new NotImplementedException();
        }

        private bool ReplaceOneMongo(QueryDefModel qd, ConnectionModel cm, string doc)
        {
            var ret = false;
            var d = BsonDocument.Parse(doc);
            if (DAS.GetService(cm.name).RemoveOne(d))
            {
                var jobject = DAS.GetService(cm.name).AddOne(d);
                if (jobject != null)
                    ret = true;
            }

            return ret; ;
        }

        #endregion
        #region RemoveOne
        public bool RemoveOne(string QueryDefName, string doc)
        {
            QueryDefModel qd = DAS.GetQueryDefinition(QueryDefName);
            if (!qd.canDelete)
                throw new Exception("The query definition does not support remove operations");
            ConnectionModel cm = DAS.GetCollectionDefinition(qd.connection);

            if (cm.engineType == "mongo")
                return RemoveOneMongo(qd, cm, doc);
            else if (cm.engineType == "mssql")
                return RemoveOneSQL(qd, cm, doc);
            else
                throw new Exception("Execute Query - engineType (" + cm.engineType + ") was not defined");
        }

        private bool RemoveOneSQL(QueryDefModel qd, ConnectionModel cm, string doc)
        {
            throw new NotImplementedException();
        }

        private bool RemoveOneMongo(QueryDefModel qd, ConnectionModel cm, string doc)
        {
            var result = DAS.GetService(cm.name).RemoveOne(BsonDocument.Parse(doc));
            return result;
        }

        #endregion
        private string[] GetFields(QueryDefModel qd)
        {
            List<string> fields = new();
            if (qd.fields.Count() > 0)
            {
                for (int a = 0; a < qd.fields.Count(); a++)
                {
                    if (!String.IsNullOrEmpty(qd.fields[a].dbName) )
                        if( qd.fields[a].dbName != "_id")
                            fields.Add(qd.fields[a].dbName);
                }
            }
            return fields.ToArray();
        }

        /// <summary>
        /// selects the fields identified in the document defintion
        /// </summary>
        /// <param name="doc">document to get data from</param>
        /// <param name="fields">list of fields that will be provided</param>
        /// <param name="includeId">allows the developer to include the id value, overriding the querydef list.</param>
        /// <returns></returns>
        private JObject SelectFields(BsonDocument doc, string[] fields, bool includeId = true)
        {
            JObject ret = new();
            
            if (fields.Length > 0)
            { 
                foreach (string f in fields)
                {
                    ret.Add(f, doc.GetValue(f).ToString());
                }
                if (includeId)
                    ret.Add("_id", doc.GetId());

            }
            else
                ret = JObject.Parse(doc.ToString());

            return ret;
        }
       
    }
}
