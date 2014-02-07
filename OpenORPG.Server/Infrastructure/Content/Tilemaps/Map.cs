// Distributed as part of TiledSharp, Copyright 2012 Marshall Ward
// Licensed under the Apache License, Version 2.0
// http://www.apache.org/licenses/LICENSE-2.0

using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Server.Infrastructure.Content.Tilemaps
{
    public class TmxMap : TmxDocument
    {
        public enum OrientationType : byte
        {
            Orthogonal,
            Isometric,
            Staggered
        }

        public TmxMap(Stream stream)
        {
            XDocument xDoc = ReadXml(stream);
            XElement xMap = xDoc.Element("map");

            Version = (string) xMap.Attribute("version");
            Orientation = (OrientationType) Enum.Parse(
                typeof (OrientationType),
                xMap.Attribute("orientation").Value,
                true);
            Width = (int) xMap.Attribute("width");
            Height = (int) xMap.Attribute("height");
            TileWidth = (int) xMap.Attribute("tilewidth");
            TileHeight = (int) xMap.Attribute("tileheight");
            BackgroundColor = new TmxColor(xMap.Attribute("backgroundcolor"));

            Tilesets = new TmxList<TmxTileset>();
            foreach (XElement e in xMap.Elements("tileset"))
                Tilesets.Add(new TmxTileset(e, TmxDirectory));

            Layers = new TmxList<TmxLayer>();
            foreach (XElement e in xMap.Elements("layer"))
                Layers.Add(new TmxLayer(e, Width, Height));

            ObjectGroups = new TmxList<TmxObjectGroup>();
            foreach (XElement e in xMap.Elements("objectgroup"))
                ObjectGroups.Add(new TmxObjectGroup(e));

            if (xMap.Elements("imagelayer").Any())
                throw new NotSupportedException(
                    "Image layers are not supported in the current implementation. You can disable this warning at your own risk.");


            Properties = new PropertyDict(xMap.Element("properties"));
        }

        public string Version { get; private set; }
        public OrientationType Orientation { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int TileWidth { get; private set; }
        public int TileHeight { get; private set; }
        public TmxColor BackgroundColor { get; private set; }

        public TmxList<TmxTileset> Tilesets { get; private set; }
        public TmxList<TmxLayer> Layers { get; private set; }
        public TmxList<TmxObjectGroup> ObjectGroups { get; private set; }
        public PropertyDict Properties { get; private set; }
    }
}