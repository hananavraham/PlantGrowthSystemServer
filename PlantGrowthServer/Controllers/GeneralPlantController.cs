using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using PlantGrowthServer.App_Start;
using PlantGrowthServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PlantGrowthServer.Controllers
{
    public class GeneralPlantController : Controller
    {
        private MongoDBContext dBContext;
        private IMongoCollection<GeneralPlantModel> generalPlantCollection;

        public GeneralPlantController()
        {
            dBContext = new MongoDBContext();
            generalPlantCollection = dBContext.database.GetCollection<GeneralPlantModel>("General_Plant");
        }

        // GET: GeneralPlant
        public ActionResult Index()
        {
            List<GeneralPlantModel> generalPlants = generalPlantCollection.AsQueryable<GeneralPlantModel>().ToList();
            return Content(JsonConvert.SerializeObject(generalPlants));
        }

        // GET: GeneralPlant/getGeneralPlantById
        [HttpGet]
        public ActionResult getGeneralPlantById(string id)
        {
            try
            {
                var generalPlantId = new ObjectId(id);
                var generalPlant = generalPlantCollection.AsQueryable<GeneralPlantModel>().SingleOrDefault(x => x.Id == generalPlantId);
                return Content(JsonConvert.SerializeObject(generalPlant));
            }

            catch
            {
                return null;
            }
        }

        // POST: GeneralPlant/Create
        [HttpPost]
        public ActionResult Create(GeneralPlantModel generalPlant)
        {
            try
            {
                generalPlantCollection.InsertOne(generalPlant);
                return View();
            }

            catch
            {
                return null;
            }
            
        }

        // POST: GeneralPlant/Edit
        [HttpPost]
        public ActionResult Edit(string id, GeneralPlantModel generalPlant)
        {
            try
            {
                var filter = Builders<GeneralPlantModel>.Filter.Eq("_id", ObjectId.Parse(id));
                var update = Builders<GeneralPlantModel>.Update
                    .Set("Name", generalPlant.Name)
                    .Set("Image", generalPlant.Image);

                var result = generalPlantCollection.UpdateOne(filter, update);
                return View();
            }

            catch
            {
                return null;
            }
            
        }

        // GET: GeneralPlant/Delete
        [HttpGet]
        public ActionResult Delete(string id)
        {
            try
            {
                //var generalPlantId = new ObjectId(id);
                //var generalPlant = generalPlantCollection.AsQueryable<GeneralPlantModel>().SingleOrDefault(x => x.Id == generalPlantId);
                generalPlantCollection.DeleteOne(Builders<GeneralPlantModel>.Filter.Eq("_id", ObjectId.Parse(id)));
                return View();
            }

            catch
            {
                return null;
            }
        }
    }
}