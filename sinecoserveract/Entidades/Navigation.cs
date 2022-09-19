using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace sinecoserveract.Entidades
{
    public class Navigation
    {
        [JsonProperty(PropertyName = "compact")]
        public List<Modulos> Compact { get; set; }
        [JsonProperty(PropertyName = "default")]
        public List<Modulos> Default { get; set; }
        [JsonProperty(PropertyName = "futuristic")]
        public List<Modulos> Futuristic { get; set; }
        [JsonProperty(PropertyName = "horizontal")]
        public List<Modulos> Horizontal { get; set; }


       
    }
}
