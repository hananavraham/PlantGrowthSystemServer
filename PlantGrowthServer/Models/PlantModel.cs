using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using PlantGrowthServer.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PlantGrowthServer.Models
{
    public class PlantModel
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("Env_control_address")]
        public string Env_control_address { get; set; }

        [BsonElement("Growth_control_address")]
        public string Growth_control_address { get; set; }

        [BsonElement("Min_temperature")]
        public List<float> Min_temperature { get; set; }

        [BsonElement("Max_temperature")]
        public List<float> Max_temperature { get; set; }

        [BsonElement("Min_light")]
        public List<float> Min_light { get; set; }

        [BsonElement("Max_light")]
        public List<float> Max_light { get; set; }

        [BsonElement("Min_humidity")]
        public List<int> Min_humidity { get; set; }

        [BsonElement("Max_humidity")]
        public List<int> Max_humidity { get; set; }

        [BsonElement("Temperature")]
        public List<Temperature> Temperature { get; set; }

        [BsonElement("Light")]
        public List<Light> Light { get; set; }

        [BsonElement("Humidity")]
        public List<Humidity> Humidity { get; set; }

        [BsonElement("Size")]
        public List<Size> Plant_size { get; set; }
    }
}