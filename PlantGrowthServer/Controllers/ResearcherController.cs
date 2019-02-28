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
    public class ResearcherController : Controller
    {
        private MongoDBContext dBContext;
        private IMongoCollection<ResearcherModel> researcherCollection;

        public ResearcherController()
        {
            dBContext = new MongoDBContext();
            researcherCollection = dBContext.database.GetCollection<ResearcherModel>("Researcher");
        }


        // GET: Researcher
        public ActionResult Index()
        {
            try
            {
                List<ResearcherModel> researches = researcherCollection.AsQueryable<ResearcherModel>().ToList();
                return Content(JsonConvert.SerializeObject(researches));
            }
            
            catch
            {
                return null;
            }
            
        }

        // GET : Researcher/GetResearcherById
        [HttpGet]
        public ActionResult GetResearcherById(string id)
        {
            try
            {
                var researcherId = new ObjectId(id);
                var researcher = researcherCollection.AsQueryable<ResearcherModel>().SingleOrDefault(x => x.Id == researcherId);
                return Content(JsonConvert.SerializeObject(researcher));
            }

            catch
            {
                return null;
            }
        }

        // POST : Researcher/GetResearcherByEmail
        [HttpPost]
        public ActionResult GetResearcherByEmail(string email)
        {
            try
            {
                var researcher = researcherCollection.AsQueryable<ResearcherModel>().SingleOrDefault(x => x.Email == email);
                return Content(JsonConvert.SerializeObject(researcher));
            }

            catch
            {
                return null;
            }
        }

        // POST : Researcher/Edit
        [HttpPost]
        public ActionResult Edit(string id, ResearcherModel researcher)
        {
            try
            {
                var filter = Builders<ResearcherModel>.Filter.Eq("_id", ObjectId.Parse(id));
                var update = Builders<ResearcherModel>.Update
                    .Set("Email", researcher.Email)
                    .Set("Degree", researcher.Degree);
                var result = researcherCollection.UpdateOne(filter, update);
                return View();
            }

            catch
            {
                return null;
            }
            
        }

        // POST : Researcher/Create
        [HttpPost]
        public ActionResult Create(ResearcherModel researcher)
        {
            try
            {
                researcherCollection.InsertOne(researcher);
                return View();
            }

            catch
            {
                return null;
            }
        }

        // GET : Researcher/GetOwnerResearches
        [HttpGet]
        public ActionResult GetOwnerResearches(string id)
        {
            List<ResearchModel> researches;
            var owner = researcherCollection.AsQueryable<ResearcherModel>().SingleOrDefault(x => x.Id == ObjectId.Parse(id));
            var researchCollection = dBContext.database.GetCollection<ResearchModel>("Research");
            //var query = researchCollection.AsQueryable< ResearchModel>().Join(researcherCollection.AsQueryable<ResearcherModel>()),


            //researches = researchCollection.AsQueryable<ResearchModel>().SingleOrDefault(x => x.Owners == owner.Id);


            var result = researchCollection.AsQueryable<ResearchModel>().SelectMany(x => x.Owners.Find(b => b == id));
            return Content(JsonConvert.SerializeObject(result));





        }

    }
}