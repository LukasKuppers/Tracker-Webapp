using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tracker_Server.Services.DataAccess
{
    public class DbClient : IDbClient
    {
        private readonly IMongoDatabase db;

        public DbClient(string path)
        {
            var client = new MongoClient();
            db = client.GetDatabase(path);
        }

        public List<T> GetAll<T>(string table)
        {
            var collection = db.GetCollection<T>(table);
            var query = collection.Find(Builders<T>.Filter.Empty);

            return query.ToList();
        }

        public List<T> FindByField<T, U>(string table, string fieldName, U data)
        {
            var collection = db.GetCollection<T>(table);
            var filter = Builders<T>.Filter.Eq(fieldName, data);
            var query = collection.Find(filter);

            return query.ToList();
        }

        public DbStatus InsertRecord<T>(string table, T record)
        {
            var collection = db.GetCollection<T>(table);
            collection.InsertOne(record);

            return DbStatus.SUCCESS;
        }

        public DbStatus DeleteRecord<T>(string table, Guid id)
        {
            var collection = db.GetCollection<T>(table);
            var filter = Builders<T>.Filter.Eq("_id", id);

            long count = collection.CountDocuments(filter);
            if (count <= 0)
            {
                return DbStatus.NOT_FOUND;
            }
            if (count > 1)
            {
                return DbStatus.INTERNAL_ERROR;
            }

            collection.DeleteOne(filter);
            return DbStatus.SUCCESS;
        }

        public DbStatus UpdateRecord<T, U>(string table, Guid id, string fieldName, U data)
        {
            var collection = db.GetCollection<T>(table);
            var filter = Builders<T>.Filter.Eq("_id", id);

            long count = collection.CountDocuments(filter);
            if (count <= 0)
            {
                return DbStatus.NOT_FOUND;
            }
            if (count > 1)
            {
                return DbStatus.INTERNAL_ERROR;
            }

            var update = Builders<T>.Update.Set(fieldName, data);
            collection.UpdateOne(filter, update);
            return DbStatus.SUCCESS;
        }

        public bool Contains<T, U>(string table, string fieldName, U data)
        {
            var collection = db.GetCollection<T>(table);
            var filter = Builders<T>.Filter.Eq(fieldName, data);

            long count = collection.CountDocuments(filter);
            return count > 0;
        }
    }
}
