using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Server.Infrastructure.Content
{
    public class JsonAssetSerializer
    {
        private readonly JsonSerializerSettings settings;

        public JsonAssetSerializer()
        {
            settings = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };
        }

        public T Deserialize<T>(string data)
        {
            return JsonConvert.DeserializeObject<T>(data, settings);
        }

        public object Deserialize(string data, Type type)
        {
            return JsonConvert.DeserializeObject(data, type, settings);
        }
    }
}