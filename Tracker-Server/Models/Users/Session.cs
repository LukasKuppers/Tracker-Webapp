using Microsoft.AspNetCore.Authorization;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace Tracker_Server.Models.Users
{
    public class Session
    {
        // this is the sesison ID
        [BsonId]
        public Guid Id { get; set; }

        public Guid UserId { get; set; }
    }
}
