using Newtonsoft.Json.Linq;
using System.IO;
using System.Reflection;

namespace Tracker_Server.Services.Constants
{
    // handle retrieval of constants from systemConstants.resx
    public class Constants
    {
        private static readonly string RESOURCE_PATH = "Tracker-Server.Resources.SystemConstants.json";

        private static JObject getJson()
        {
            var assembly = Assembly.GetEntryAssembly();
            var resStream = assembly.GetManifestResourceStream(RESOURCE_PATH);
            using (var reader = new StreamReader(resStream))
            {
                string jsonContent = reader.ReadToEnd();
                JObject data = JObject.Parse(jsonContent);
                return data;
            }
        }

        public static string getString(string key)
        {
            JObject data = getJson();
            return (string)data[key];
        }
    }
}
