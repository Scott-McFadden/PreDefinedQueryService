using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDataAccess;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using Newtonsoft.Json;

namespace UnitTests
{
    [TestClass]
    public class MongoServiceTests
    {
        private static IService mongoService;
        public static string connectionString = "mongodb://localhost:27017/";
        public static string databaseName = "UnitTest";
        public static string collectionName = "unittest";

        public static BsonDocument ReadyForm;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            mongoService = Service.MakeConnection("unitTest",connectionString, databaseName, collectionName);
        }
        [TestMethod]
        public void A1_OpenConnection()
        { 
            Assert.IsTrue(MongoServiceTests.mongoService.IsServiceReady); 
        }

        [TestMethod]
        public void A1_AddOne()
        {
            var doc = BsonDocument.Parse("{  \"test\":  \"B1_AddOne\" ,\"achieved\" : true   }");
            MongoServiceTests.ReadyForm = doc;
            MongoServiceTests.mongoService.AddOne(doc);
            string s = doc.ToJson(new JsonWriterSettings { OutputMode = JsonOutputMode.CanonicalExtendedJson });
            // Console.WriteLine(JsonConvert.SerializeObject(doc));
            Console.WriteLine(s);
            MongoServiceTests.ReadyForm = doc;

            Assert.IsNotNull(doc.GetElement("_id"));
        }

        [TestMethod]
        public void B1_GetById()
        {
            
            var ret = MongoServiceTests.mongoService.GetById(MongoServiceTests.ReadyForm.GetElement("_id").Value.ToString());
            Assert.IsNotNull(ret);
            string s = ret.ToJson(new JsonWriterSettings { OutputMode = JsonOutputMode.CanonicalExtendedJson });
            // Console.WriteLine(JsonConvert.SerializeObject(doc));
            Console.WriteLine(s);
            
            Assert.IsTrue(ret.GetElement("_id").Value.ToString().Equals( MongoServiceTests.ReadyForm.GetElement("_id").Value.ToString()));
        }

        [TestMethod]
        public void B20_GetById()
        {
            var s = "";
            var ret = MongoServiceTests.mongoService.GetById("aaaaf06b3e0b2ed37a2cddd5");
            if (ret == null)
             s =   "no record was returned";
             
            Console.WriteLine(s);
            Assert.IsTrue(ret == null);
          
        }

        [TestMethod]
        public void B21_Update()
        {
            string newdoc = MongoServiceTests.ReadyForm.ToJson();
            BsonDocument bdoc = BsonDocument.Parse(newdoc);
            bdoc.SetElement( new BsonElement("test", "B2_Update"));

            var ret = MongoServiceTests.mongoService.ReplaceOne(bdoc);
            Assert.IsNotNull(ret, "result from ReplaceOne is null ");

            string s = ret.ToJsonString();
            Console.WriteLine(s);

            Assert.IsTrue(ret,  "true if document was updated");  
            
        }

        [TestMethod]
        public void B22_GetByTestName()
        {

            var doc = BsonDocument.Parse("{  \"test\":  \"B22_GetByTestName\" ,\"achieved\" : true   }");
            MongoServiceTests.mongoService.AddOne(doc);
            var id = doc.GetId();

            var ret = MongoServiceTests.mongoService.GetOne("{ \"test\": \"B22_GetByTestName\"}");
            Assert.IsNotNull(ret, "result from ReplaceOne is null ");

            string s = ret.ToJsonString();
            Console.WriteLine(s);

        }


        [TestMethod]
        public void B23_GetMany()
        {
             
            var ret = MongoServiceTests.mongoService.GetMany("{}");
            Assert.IsNotNull(ret, "result from ReplaceOne is null ");
            Assert.IsTrue(ret.Count > 0, "has items");
            string s = ret.ToJsonString();
            Console.WriteLine(s);
        }

        [TestMethod]
        public void B3_Remove()
        {

            var ret = MongoServiceTests.mongoService.RemoveOne(MongoServiceTests.ReadyForm);
            Assert.IsTrue(ret);
             
        }
        [TestMethod]
        public void U1_utils()
        {
            var x = MongoServiceTests.ReadyForm.GetElement("_id").Value.ToString();
            var y = x.FilterById();

            Assert.IsNotNull(y);
            
        }


        [TestMethod]
        public void zzzCleanUp()
        {
            var ret = MongoServiceTests.mongoService.GetMany("{}");
            foreach(BsonDocument b in ret)
            {
                MongoServiceTests.mongoService.RemoveOne(b);
            }
        }

    }


}

