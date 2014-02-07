using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Server.Game.Network.Packets;

namespace Server.Infrastructure.Network.Packets
{
    public class JsonPacketSerializer : IPacketSerializer<string>
    {
        private readonly Dictionary<OpCodes, Type> packetFactory = new Dictionary<OpCodes, Type>();
        private readonly JsonSerializerSettings settings;


        public JsonPacketSerializer()
        {
            foreach (
                Type packetType in
                    typeof (JsonPacketSerializer).Assembly.GetTypes()
                                                 .Where(
                                                     (type) =>
                                                     typeof (IPacket).IsAssignableFrom(type) && !type.IsInterface))
            {
                var obj = (IPacket) FormatterServices.GetUninitializedObject(packetType);
                    // This doesnt require parameterless constructor

                OpCodes opCode = obj.OpCode;

                packetFactory.Add(opCode, packetType);
            }

            settings = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };
        }

        public IPacket Deserialize(string data)
        {
            int start = data.IndexOf("opCode\":") + 8;
            int end = data.IndexOf(",", start);

            var opCode = (OpCodes) ushort.Parse(data.Substring(start, end - start).Trim());


            var packet = (IPacket) JsonConvert.DeserializeObject(data, packetFactory[opCode], settings);

            return packet;
        }

        public string Serialize<TPacket>(TPacket packet) where TPacket : IPacket
        {
            return JsonConvert.SerializeObject(packet, settings);
        }
    }
}