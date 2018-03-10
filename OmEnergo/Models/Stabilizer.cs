using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OmEnergo.Models
{
    public class Stabilizer
    {
        [Key]
        public int Id { get; set; }

        public string Series { get; set; } //Enum in the future

        public string MainImageLink { get; set; }

        public string ShortDescription { get; set; }

        public string LongDescription { get; set; }

        public string StabilizationType { get; set; } //Enum in the future 

        public string StabilizationAccuracy { get; set; } //Class in the future, e.g. ("220B +- 5%") 

        public int? PhasesAmount { get; set; }

        public string OperatingFrequencyOfNetwork { get; set; } //Class in the future, e.g. ("50 (60) Гц") 

        public string AllowableDurableOverload { get; set; } //Int in the future, e.g. ("<= 110%") 

        public string InputVoltageRange { get; set; } //Class in the future, e.g. ("140 - 260 B")

        public string Efficiency { get; set; } //Int in the future, e.g. ("98%") 

        public string SwitchingTime { get; set; } //Int in the future, e.g. ("4 мс") 

        public string Indication { get; set; } //List<Enum> in the future 

        public bool? ShortCircuitProtection { get; set; }

        public bool? VoltageSurgesProtection { get; set; }

        public bool? AdjustableDelay { get; set; }

        public string BypassMode { get; set; }

        public string WorkingTemperature { get; set; } //Class in the future, e.g. ("-20 - +40°С")

        public string Dimensions { get; set; } //Class in the future, e.g. ("500x100x100 mm")

        public int? Weight { get; set; }
        
        public List<Booklet> Booklets { get; set; }

        public List<Picture> Pictures { get; set; }

        public double? Price { get; set; }
    }
}
