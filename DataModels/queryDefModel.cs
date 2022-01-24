using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModels
{
    /// <summary>
    /// this is the data model for a query.
    /// </summary>
    public class QueryDefModel
    {
        [BsonId]
        public object _id { get; set; }
        /// <summary>
        /// name of querydef
        /// </summary>
        public string name { get; set; } = "";
        /// <summary>
        /// description of model
        /// </summary>
        public string description { get; set; } = "";
        /// <summary>
        /// version of definition
        /// </summary>
        public string version { get; set; } = "";
        /// <summary>
        /// connection associated with this querydef - must exist in the connections table
        /// </summary>
        public string connection { get; set; } = "";
        /// <summary>
        /// tags used to catagorized this entry
        /// </summary>
        public IList<string> tags { get; set; } = new String[]{ "" };
        /// <summary>
        /// Field List 
        /// </summary>
        public IList<QdefFieldModel> fields { get; set; } = new List<QdefFieldModel>() { new QdefFieldModel() };
        /// <summary>
        /// Modification History
        /// </summary>
        public IList<ModificationModel> Modifications { get; set; } =  new List<ModificationModel>() { new ModificationModel() };
        /// <summary>
        /// command / parameter for base get query - many
        /// </summary>
        public string getQuery   { get; set; } = "";
        /// <summary>
        /// command / parameter for base get query (single)
        /// </summary>
        public string getOneQuery { get; set; } = "";
        /// <summary>
        /// command / parameter for base DELETE query
        /// </summary>
        public string deleteQuery { get; set; } = "";
        /// <summary>
        /// command / parameter for base delete query
        /// </summary>
        public string updateQuery { get; set; } = "";
        /// <summary>
        /// command / parameter for base ADD query
        /// </summary>
        public string addQuery { get; set; } = "";

        /// <summary>
        /// roles allowed to use this defintion
        /// </summary>
        public IList<string> roles { get; set; } = new String[] { "" };
        [BsonIgnore]
        public bool canAdd { get => this.addQuery.Length > 0; }
        [BsonIgnore]
        public bool canUpdate { get => this.updateQuery.Length > 0; }
        [BsonIgnore]
        public bool canDelete { get => this.deleteQuery.Length > 0; }
        [BsonIgnore]
        public bool canGet { get => this.getQuery.Length > 0; }

        /// <summary>
        /// creates an instance of this object from the string provided.
        /// </summary>
        /// <param name="data">json string representing this object</param>
        /// <returns>new QueryDefModel object</returns>
        public static QueryDefModel deserialize(string data)
        {
            return JsonConvert.DeserializeObject<QueryDefModel>(data);
        }
        /// <summary>
        /// create a json string for the object.
        /// </summary>
        /// <param name="model">QueryDefModel</param>
        /// <returns>json string</returns>
        public static string serialize(QueryDefModel model)
        {
            return JsonConvert.SerializeObject(model, Formatting.Indented);
        }
        /// <summary>
        /// Validate Model
        /// </summary>
        /// <returns>true if valid, otherwise false</returns>
        public bool Validate()
        {
            if (
                String.IsNullOrEmpty(name) ||
                String.IsNullOrEmpty(connection) ||
                !( canAdd ||  canDelete || canGet || canUpdate ) // must have one
                || fields.Count == 0 // must have at least one field defined
                || tags.Count == 0  // must has at least on tags defined
                ) return false;
            return true;
        }
    }
}
