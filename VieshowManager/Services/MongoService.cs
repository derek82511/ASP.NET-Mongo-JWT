using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using VieshowManager.Models.Mongo;

namespace VieshowManager.Services
{
    public static class MongoService
    {
        private static MongoClient client;

        private static IMongoDatabase db;

        public static IMongoCollection<BsonDocument> users { private set; get; }

        static MongoService()
        {
            client = new MongoClient(ConfigurationManager.AppSettings["MongoConnectionString"]);

            db = client.GetDatabase("test");

            users = db.GetCollection<BsonDocument>("users");
        }

    }
}