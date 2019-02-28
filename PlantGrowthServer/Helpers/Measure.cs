using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PlantGrowthServer.Helpers
{
    public class Measure
    {
        public Temperature Temp { get; set; }
        public Light Light { get; set; }
        public Humidity Humidity { get; set; }
    }
}