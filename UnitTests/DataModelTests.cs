using DataModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTests
{
    [TestClass]
    public class DataModelTests
    {
        [TestMethod]
        public void Test_1_CollectionDeserilazation()
        {
            var data = @"{'name': 'querydefs','description': 'connection info for querydefs','engineType': 'mongo','connectionString': 'mongodb://localhost:27017/','dbName' : 'querydefs','collectionName': 'querydefs','uniqueKeys' : ['name'],'schema':''}";

            ConnectionModel model = ConnectionModel.deserialize(data);

             Assert.IsTrue(model.Name == "querydefs");
            Assert.IsTrue(model.uniqueKeys.Length == 1);
            Assert.IsTrue(model.connectionString == "mongodb://localhost:27017/");



        }

        [TestMethod]
        public void Test_2_QueryDefDeserialization()
        {

            var data = @"{'name': 'getConnection','description': 'Gets a list of templates','version': '0.1.1','tags': ['connection', 'queryDef'],'connection': 'connections','fields': [{'name': 'name','dbNAme': 'name','dataType': 'string','description': 'specific name of connection','validation': 'none','inputType': 'text'},{'name': 'description','dbNAme': 'description','dataType': 'string','description': 'description of the connection','validation': 'none','inputType': 'text'},{'name': 'EngineType','dbNAme': 'engineType','dataType': 'string','description': 'describes engine we are connecting to.  i.e. SQLServer, Mongo, WebAPI','validation': 'none','options' : ['SQLServer', 'Mongo', 'WebAPI'],'inputType': 'select'},{'name': 'ConnectionString','dbNAme': 'connectionString','dataType': 'string','description': 'uri to access engine','validation': 'string','inputType': 'text'},{'name': 'collectionName','dbNAme': 'collectionName','dataType': 'string','description': 'name of target database or collection','validation': 'string','inputType': 'text'},{'name': 'id','dbNAme': '_id','dataType': 'string','description': 'template id assigned by system','validation': 'none','inputType': 'text','autoassigned' : true}],'getQuery': '{}','abilities': {'get': true,'insert': true,'delete': true,'update': true},'roles': ['queryDefAdmin'],'Modifications': [{'when': '12/27/21','who': 'Scott','jiraTicket': 'none','description': 'i created this'}]}";
           QueryDefModel model = QueryDefModel.deserialize(data);

            Assert.IsTrue(model.name == "getConnection");
            Assert.IsTrue(model.tags.Count == 2);
            Assert.IsTrue(model.connection == "connections");


        }

        [TestMethod]
        public void Test_3_ConnectionSerialization()
        {
            string data = ConnectionModel.serialize( new ConnectionModel());

            Assert.IsTrue(data.Length > 10);
            Console.WriteLine(data);
        }

        [TestMethod]
        public void Test_4_QueryDefSerialization()
        {
            string data = QueryDefModel.serialize(new QueryDefModel());

            Assert.IsTrue(data.Length > 10);
             Console.WriteLine(data);
        }

    }
}
