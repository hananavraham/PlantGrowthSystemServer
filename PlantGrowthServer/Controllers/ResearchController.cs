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
    public class ResearchController : Controller
    {
        private MongoDBContext dBContext;
        private IMongoCollection<ResearchModel> researchCollection;

        public ResearchController()
        {
            dBContext = new MongoDBContext();
            researchCollection = dBContext.database.GetCollection<ResearchModel>("Research");
        }



        // GET: Research
        public ActionResult Index()
        {
            try
            {
                List<ResearchModel> researches = researchCollection.AsQueryable<ResearchModel>().ToList();
                return Content(JsonConvert.SerializeObject(researches));
            }
            catch
            {
                return null;
            }
            
        }

        //GET : Research/GetResearchById
        public ActionResult GetResearchById(string id)
        {
            try
            {
                var researchId = new ObjectId(id);
                var research = researchCollection.AsQueryable<ResearchModel>().SingleOrDefault(x => x.Id == researchId);
                return Content(JsonConvert.SerializeObject(research));
            }

            catch
            {
                return null;
            }
            
        }
        

        // POST : Research/Create
        [HttpPost]
        public ActionResult Create(ResearchModel research)
        {
            try
            {
                researchCollection.InsertOne(research);
                return View();
            }

            catch
            {
                return View();
            }
        }

        // POST : Research/Edit
        [HttpPost]
        public ActionResult Edit(string id, ResearchModel research)
        {
            try
            {
                var filter = Builders<ResearchModel>.Filter.Eq("_id", ObjectId.Parse(id));
                var update = Builders<ResearchModel>.Update
                    .Set("Start_date", research.Start_date)
                    .Set("End_date", research.End_date)
                    .Set("Owners", research.Owners);
                var result = researchCollection.UpdateOne(filter, update);
                return View();
            }

            catch
            {
                return View();
            }
        }

        // GET : Research/Delete
        [HttpGet]
        public ActionResult Delete(string id)
        {
            try
            {
                var research = researchCollection.AsQueryable<ResearchModel>().SingleOrDefault(x => x.Id == ObjectId.Parse(id));
                researchCollection.DeleteOne(Builders<ResearchModel>.Filter.Eq("_id", ObjectId.Parse(id)));
                //deleting all the plants belnogs to the research
                var plantCollection = dBContext.database.GetCollection<ResearchModel>("Plant");
                foreach(var plantId in research.Plants_id)
                    plantCollection.DeleteOne(x => x.Id == ObjectId.Parse(plantId));
                return View();
            }

            catch
            {
                return null;
            }
            
        }

        // GET : Research/AddOwnersToResearch 
        [HttpGet]
        public ActionResult AddOwnersToResearch(string researchId, List<string> ownersId)
        {
            try
            {
                var filter = Builders<ResearchModel>.Filter.Eq("_id", researchId);
                var builder = Builders<ResearchModel>.Update;
                var update = builder
                    .Push("Owners", ownersId);
                var result = researchCollection.UpdateOne(filter, update);
                return View();
            }

            catch
            {
                return null;
            }
            
        }

        // GET : Research/GetResearchesByOwner
        [HttpGet]
        public ActionResult GetResearchesByOwner(string ownerId)
        {
            //var researches = researchCollection.AsQueryable<ResearchModel>().SingleOrDefault(x => x.Owners == ObjectId.Parse(ownerId));
            //return Content(JsonConvert.SerializeObject(researchs));
            return View();
        }
    }
}