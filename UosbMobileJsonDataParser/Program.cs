using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace UosbMobileJsonDataParser
{
    class Program
    {
        static void Main()
        {
            var file = @"Company.json";

            var data = JsonConvert.DeserializeObject<List<UOSBCompany>>(JsonDataString.Data);
            var id = 1;

            var retVal = new List<UOSBCompany>();
            foreach (var uosbCompany in data)
            {
                uosbCompany.Id = id.ToString();
                uosbCompany.Company = uosbCompany.Company.Trim();
                uosbCompany.Address = uosbCompany.Address.Trim();
                for (var i = 0; i < uosbCompany.Phone.Count; i++)
                {
                    uosbCompany.Phone[i] = uosbCompany.Phone[i].Trim();
                }

                var latValue = uosbCompany.Location.Latitude.Split(' ').Where(x => !string.IsNullOrEmpty(x)).Select(x => float.Parse(x.Replace('.', ','))).ToArray();

                if (latValue.Length != 3) continue;

                var decLatValue = latValue[0] + latValue[1] / 60 + latValue[2] / 3600;
                uosbCompany.Location.Latitude = decLatValue.ToString().Replace(',', '.');

                var lngValue = uosbCompany.Location.Longitude.Split(' ').Where(x => !string.IsNullOrEmpty(x)).Select(x => float.Parse(x.Replace('.', ','))).ToArray();
                var decLngValue = lngValue[0] + lngValue[1] / 60 + lngValue[2] / 3600;
                uosbCompany.Location.Longitude = decLngValue.ToString().Replace(',', '.');

                uosbCompany.Company = Regex.Replace(uosbCompany.Company, @"\s+", " ").Trim();
                uosbCompany.Address = Regex.Replace(uosbCompany.Address, @"\s+", " ").Trim();

                retVal.Add(uosbCompany);
                ++id;
            }
            if (File.Exists(file))
            {
                File.Delete(file);
            } 

            using (var fs = File.Open(file, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
            using (var sw = new StreamWriter(fs))
            using (var jw = new JsonTextWriter(sw))
            {

                jw.Formatting = Formatting.Indented;

                var serializer = new JsonSerializer();
                serializer.Serialize(jw, retVal);
            }
        }
    }
}
