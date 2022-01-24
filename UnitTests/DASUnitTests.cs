using DataModels;
using DomainOps;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDataAccess;
using MongoDB.Bson;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestClass]
    public class DASUnitTests
    {
        [TestMethod]
        public void A1_DAS_Tests()
        {
            Console.WriteLine(DateTime.Now.ToString());
            try { 
            Assert.IsNotNull(DAS.CollectionService );

            Service a = DAS.GetService("collections");
            Assert.IsTrue(a.name == "collections", "getservice ok");

            var data = @"{'name': 'xxquerydefs','description': 'connection info for querydefs','engineType': 'mongo','connectionString': 'mongodb://localhost:27017/','dbName' : 'querydefs','collectionName': 'querydefs','uniqueKeys' : ['name'],'schema':''}";

            ConnectionModel model = ConnectionModel.deserialize(data);

            DAS.AddCollectionDefinition(model);
            var b = DAS.GetCollectionDefinition("xxquerydefs");
                var c = DAS.GetService("xxquerydefs");
            Assert.IsTrue(b.name == "xxquerydefs",  "adding model querydefs check");
                Assert.IsTrue(c.name == "xxquerydefs");


                var data2 = @"{'name': 'xgetConnection','description': 'Gets a list of templates','version': '0.1.1','tags': ['connection', 'queryDef'],'connection': 'connections','fields': [{'name': 'name','dbNAme': 'name','dataType': 'string','description': 'specific name of connection','validation': 'none','inputType': 'text'},{'name': 'description','dbNAme': 'description','dataType': 'string','description': 'description of the connection','validation': 'none','inputType': 'text'},{'name': 'EngineType','dbNAme': 'engineType','dataType': 'string','description': 'describes engine we are connecting to.  i.e. SQLServer, Mongo, WebAPI','validation': 'none','options' : ['SQLServer', 'Mongo', 'WebAPI'],'inputType': 'select'},{'name': 'ConnectionString','dbNAme': 'connectionString','dataType': 'string','description': 'uri to access engine','validation': 'string','inputType': 'text'},{'name': 'collectionName','dbNAme': 'collectionName','dataType': 'string','description': 'name of target database or collection','validation': 'string','inputType': 'text'},{'name': 'id','dbNAme': '_id','dataType': 'string','description': 'template id assigned by system','validation': 'none','inputType': 'text','autoassigned' : true}],'getQuery': '{}','abilities': {'get': true,'insert': true,'delete': true,'update': true},'roles': ['queryDefAdmin'],'Modifications': [{'when': '12/27/21','who': 'Scott','jiraTicket': 'none','description': 'i created this'}]}";
                QueryDefModel model2 = QueryDefModel.deserialize(data2);

                DAS.AddQueryDefinition(model2);
                var d = DAS.GetQueryDefinition("xgetConnection");
                Assert.IsTrue(d.name == "xgetConnection");

                var e = DAS.GetCollectionDefinition("templates");
                Assert.IsTrue(e.name == "templates");

                var f = DAS.GetService("querydefs").GetOne("{  \"name\": \"GetNamedQueryDef\" }");
                Assert.IsNotNull(f);
                Assert.IsTrue(f.GetValue("name") == "GetNamedQueryDef");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Assert.Fail();
            }
        }
        [TestMethod]
        public void B1_ResolveCriteriaTest()
        {
            Console.WriteLine(DateTime.Now.ToString());
            var criteria = @"{ ""name"" : ""${ddd}"" }{ ""description"": ""${ccc} "" }";
            Dictionary<string, string> pin = new Dictionary<string, string>();
            pin.Add("ddd", "replaced_D_");
            pin.Add("ccc", "replaced_C_");

            var result = criteria.ResolveCriteria(pin);

            Console.WriteLine("result: "+ result);

            Assert.IsTrue(result.Contains("replaced_C_"));
            Assert.IsTrue(result.Contains("replaced_D_"));
        }
        [TestMethod]
        public void B2_ResolveCriteriaTest()
        {
            var criteria = @"{ ""name"" : ""${ddd}"" }{ ""description"": ""${ccc} "" }";

            var pin = JObject.Parse(@" { ""ddd"" : ""replaced_D_"",  ""ccc"": ""replaced_C_"" }");

            var result = criteria.ResolveCriteria(pin);

            Console.WriteLine("result: " + result);

            Assert.IsTrue(result.Contains("replaced_C_"));
            Assert.IsTrue(result.Contains("replaced_D_"));
        }
        [TestMethod]
        public void C1_Test_ExecuteQuery_GetOne()
        {
            Console.WriteLine(DateTime.Now.ToString());
            var c = @"{""docname"" : ""GetQueryDefList"" }";
            ExecuteQuery eq = new();
            var results = eq.GetOne("GetNamedQueryDef", JObject.Parse(c));
            Console.WriteLine(results.ToString());
        }

        [TestMethod]
        public void C3_Test_ExecuteQuery_GetOne()
        {
            Console.WriteLine(DateTime.Now.ToString());
            var c = @"{ }";
            ExecuteQuery eq = new();
            var results = eq.GetOne("GetNamedQueryDef", JObject.Parse(c));
            Console.WriteLine(results.ToString());
        }
        [TestMethod]
        public void C2_Test_ExecuteQuery_Get()
        {
            Console.WriteLine(DateTime.Now.ToString());
            var c = @"{}";
            ExecuteQuery eq = new();
            var results = eq.Get("GetNamedQueryDef", JObject.Parse(c));
            Console.WriteLine(results.ToString());
        }

        [TestMethod]
        public void C4_Test_ExecuteQuery_GetById()
        {
            Console.WriteLine(DateTime.Now.ToString());
            var c = @"61e863f3d316edadbde874d5";
            ExecuteQuery eq = new();
            JObject results = eq.GetById("GetNamedQueryDef", c);
            Console.WriteLine(results.ToString());

            Assert.IsTrue(results["_id"].ToString() == c);
        }

        [TestMethod]
        public void D1_Test_AddUpdateDelete()
        {
            Console.WriteLine(DateTime.Now.ToString());
            var doc = "{\"name\":\"QueryDefTest\",\"description\":\"Used for unit testing\",\"version\":\"1.0\",\"connection\":\"querydefs\",\"tags\":[\"connections\",\"Admin\"],\"fields\":[{\"name\":\"Name\",\"dbName\":\"name\",\"description\":\"\",\"dataType\":\"\",\"validation\":\"\",\"validationType\":0,\"inputType\":\"\"},{\"name\":\"Description\",\"dbName\":\"description\",\"description\":\"Describes the connection\",\"dataType\":\"string\",\"validation\":\"none\",\"validationType\":0,\"inputType\":\"text\"},{\"name\":\"Version\",\"dbName\":\"version\",\"description\":\"identifies the document version\",\"dataType\":\"string\",\"validation\":\"none\",\"validationType\":0,\"inputType\":\"versionText\"},{\"name\":\"Connection\",\"dbName\":\"connection\",\"description\":\"Connection this def should use\",\"dataType\":\"string\",\"validation\":\"none\",\"validationType\":0,\"inputType\":\"Text\"},{\"name\":\"Fields\",\"dbName\":\"fields\",\"description\":\"Fields that belong to this object\",\"dataType\":\"qdefField[]\",\"validation\":\"none\",\"validationType\":0,\"inputType\":\"FieldList\",\"Children\":[{\"name\":\"Name\",\"dbName\":\"name\",\"description\":\"Name of Field\",\"dataType\":\"string\",\"validation\":\"none\",\"validationType\":0,\"inputType\":\"Text\"},{\"name\":\"DbName\",\"dbName\":\"dbName\",\"description\":\"Database/Collection Name\",\"dataType\":\"string\",\"validation\":\"none\",\"validationType\":0,\"inputType\":\"Text\"},{\"name\":\"Description\",\"dbName\":\"description\",\"description\":\"The description for this field\",\"dataType\":\"string\",\"validation\":\"none\",\"validationType\":0,\"inputType\":\"Text\"},{\"name\":\"DataType\",\"dbName\":\"dataType\",\"description\":\"Data Type of the field\",\"dataType\":\"string\",\"validation\":\"none\",\"validationType\":0,\"inputType\":\"Text\"},{\"name\":\"Validation\",\"dbName\":\"validation\",\"description\":\"validation strategy\",\"dataType\":\"object\",\"validation\":\"none\",\"validationType\":0,\"inputType\":\"Text\"},{\"name\":\"Validation Type\",\"dbName\":\"validationType\",\"description\":\"Validation Action Type\",\"dataType\":\"ValidationEnum\",\"validation\":\"none\",\"validationType\":0,\"inputType\":\"ValidationChoice\"},{\"name\":\"InputType\",\"dbName\":\"inputType\",\"description\":\"Input Type to use to service this\",\"dataType\":\"string\",\"validation\":\"none\",\"validationType\":0,\"inputType\":\"Text\"}]},{\"name\":\"Tags\",\"dbName\":\"tags\",\"description\":\"Identifies the key categoriess\",\"dataType\":\"Array\",\"validation\":\"none\",\"validationType\":0,\"inputType\":\"textArray\"}],\"Modifications\":[{\"dateModified\":\"2022-01-18T14:59:18.0610963-06:00\",\"who\":\"scott\",\"jiraTicket\":\"\",\"description\":\"initial Test\"}],\"getQuery\":\"{ }\",\"deleteQuery\":\"\",\"updateQuery\":\"\",\"addQuery\":\"\",\"roles\":[\"anyone\"]}";

            // add
            
            ExecuteQuery eq = new();
            JObject results = eq.AddOne("QueryDef",doc);
             
            // update

            results["version"] = "changed";

            string doc3 = JsonConvert.SerializeObject(results);
            bool updateResult = eq.ReplaceOne("QueryDef", doc3);

            Assert.IsTrue(updateResult, "ensure update was successful");
            //test get...
            JObject queryResult = eq.GetById("QueryDef", results["_id"].ToString());

            Assert.IsTrue(queryResult["_id"].ToString() == results["_id"].ToString(), "was able to get added record");
            Assert.IsTrue(queryResult["version"].ToString() == "changed", "record reflects the change");

            // delete
            bool deleteResult = eq.RemoveOne("QueryDef", queryResult.ToString());
            Assert.IsTrue(updateResult, "ensure delete was successful");


        }


    }
}
