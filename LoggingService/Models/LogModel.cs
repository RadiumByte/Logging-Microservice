using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace LoggingService.Models
{
    public class LogModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        /*
        [BsonElement("DateTime")]
        public DateTime Date { get; set; }

        [BsonElement("User")]
        public string UserName { get; set; }

        [BsonElement("SenderApp")]
        public string SenderApp { get; set; }

        [BsonElement("Brief")]
        public string BInfo { get; set; }

        [BsonElement("Type")]
        public string Type { get; set; }
        */
        [BsonElement("Detailed")]
        public string DInfo { get; set; }
        
    }
}
