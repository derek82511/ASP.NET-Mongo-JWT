using MongoDB.Bson;
using VieshowManager.Services.Helpers;

namespace VieshowManager.Models.Mongo
{
    public abstract class Persistence
    {
        public BsonDocument toDoc()
        {
            return new BsonDocument(ObjectToDictionaryHelper.toDictionary(this));
        }
    }
}