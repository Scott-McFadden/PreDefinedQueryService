using DataModels;
using DomainOps;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDataAccess;
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


    }
}
