using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjAula1B
{
    internal class ReadFile
    {
        public static List<PenalidadesAplicadas> GetData(string path)
        {
            StreamReader r = new StreamReader(path);
            string jsonString = r.ReadToEnd();
            var lst = JsonConvert.DeserializeObject<MotoristaHabilitado>(jsonString, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy"});

            return lst != null ? lst.PenalidadesAplicadas : null;

        }
    }
}
