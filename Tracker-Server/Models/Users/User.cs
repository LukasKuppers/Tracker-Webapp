using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tracker_Server.Models.Users
{
    public class User
    {
        [BsonId]
        public Guid Id { get; set; }

        public UserCredentials Credentials { get; set; }

        public string Username { get; set; }

        public List<Guid> Projects { get; set; }
    }
}
