using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PlantGrowthServer.Helpers
{
    public class Measure
    {
        public List<Temperature> Temp { get; set; }
        public List<Light> Light { get; set; }
        public List<Humidity> Humidity { get; set; }
    }
}