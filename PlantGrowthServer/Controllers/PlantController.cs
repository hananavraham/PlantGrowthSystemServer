using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using PlantGrowthServer.App_Start;
using PlantGrowthServer.Helpers;
using PlantGrowthServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PlantGrowthServer.Controllers
{
    public class PlantController : Controller
    {
        private MongoDBContext dBContext;
        private IMongoCollection<PlantModel> plantCollection;

        public PlantController()
        {
            dBContext = new MongoDBContext();
            plantCollection = dBContext.database.GetCollection<PlantModel>("Plant");
        }



        // GET: Plant
        public ActionResult Index()
        {
            try
            {
                List<PlantModel> plants = plantCollection.AsQueryable<PlantModel>().ToList();
                return Content(JsonConvert.SerializeObject(plants));
            }

            catch
            {
                return null;
            }
            
        }

        //GET : Plant/GetPlantById
        [HttpGet]
        public ActionResult GetPlantById(string id)
        {
            try
            {
                var plantId = new ObjectId(id);
                var plant = plantCollection.AsQueryable<PlantModel>().SingleOrDefault(x => x.Id == plantId);
                return Content(JsonConvert.SerializeObject(plant));
            }

            catch
            {
                return null;
            }
            
        }


        // POST : Plant/Create
        // Create from Client side
        [HttpPost]
        public ActionResult Create(PlantModel plant)
        {
            try
            {
                plantCollection.InsertOne(plant);
                return View();
            }

            catch
            {
                return View();
            }
        }

        // POST : Plant/Edit
        // Edit from Client side
        [HttpPost]
        public ActionResult Edit(string id, PlantModel plant)
        {
            try
            {
                var filter = Builders<PlantModel>.Filter.Eq("_id", ObjectId.Parse(id));
                var update = Builders<PlantModel>.Update
                    .Set("Min_temperature", plant.Min_temperature)
                    .Set("Max_temperature", plant.Max_temperature)
                    .Set("Min_light", plant.Min_light)
                    .Set("Max_light", plant.Max_light)
                    .Set("Min_humidity", plant.Min_humidity)
                    .Set("Max_humidity", plant.Max_humidity);
                var result = plantCollection.UpdateOne(filter, update);
                return View();
            }

            catch
            {
                return View();
            }
        }

        // POST : Plant/UpdateMeasure
        // UpdateMeasure from enviroment control unit
        [HttpPost]
        public ActionResult UpdateTemperature(string address, Measure measure)
        {
            try
            {
                var plant = plantCollection.AsQueryable<PlantModel>().SingleOrDefault(x => x.Env_control_address == address);

                var filter = Builders<PlantModel>.Filter.Eq("_id", plant.Id);
                var builder = Builders<PlantModel>.Update;
                var update = builder
                    .Push("Temperature", measure.Temp)
                    .Push("Light", measure.Light)
                    .Push("Humidity", measure.Humidity);
                var result = plantCollection.UpdateOne(filter, update);
                return View();
            }

            catch
            {
                return View();
            }
        }

        // GET : Plant/UpdateSize
        // UpdateSize from measurement control unit
        [HttpGet]
        public ActionResult UpdateSize(string address, float size)
        {
            try
            {
                var plant = plantCollection.AsQueryable<PlantModel>().SingleOrDefault(x => x.Env_control_address == address);
                var filter = Builders<PlantModel>.Filter.Eq("_id", plant.Id);
                var update = Builders<PlantModel>.Update.Push("Size", new Size
                {
                    _Size = size,
                    Date = DateTime.Now
                }
                    );
                var result = plantCollection.UpdateOne(filter, update);
                return View();
            }

            catch
            {
                return View();
            }
        }

        // GET : Plant/GetMeasuresByRange
        // GetMeasuresByRange from Client side
        [HttpGet]
        public ActionResult GetMeasuresByRange(string id, DateTime start_date, DateTime end_date)
        {
            try
            {
                var plantId = new ObjectId(id);
                var filterBuilder = Builders<PlantModel>.Filter;
                var plant = plantCollection.AsQueryable<PlantModel>().SingleOrDefault(x => x.Id == plantId);
                var temp = plant.Temperature.Select(d => d.Date >= start_date && d.Date <= end_date);
                var light = plant.Light.Select(d => d.Date >= start_date && d.Date <= end_date);
                var humidity = plant.Humidity.Select(d => d.Date >= start_date && d.Date <= end_date);
                return Content(JsonConvert.SerializeObject(temp));
            }

            catch
            {
                return View();
            }
        }


    }
}