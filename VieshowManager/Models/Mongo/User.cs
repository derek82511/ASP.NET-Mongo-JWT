using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VieshowManager.Models.Mongo
{
    public class User : Persistence
    {
        public ObjectId _id { set; get; }
        public string username { set; get; }
        public string password { set; get; }
    }
}