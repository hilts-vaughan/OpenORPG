// Distributed as part of TiledSharp, Copyright 2012 Marshall Ward
// Licensed under the Apache License, Version 2.0
// http://www.apache.org/licenses/LICENSE-2.0

using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Server.Infrastructure.Content.Tilemaps
{
    public class TmxObjectGroup : ITmxElement
    {
        public enum TmxObjectType : byte
        {
            Basic,
            Tile,
            Ellipse,
            Polygon,
            Polyline
        }

        public TmxObjectGroup(XElement xObjectGroup)
        {
            Name = (string) xObjectGroup.Attribute("name");
            Color = new TmxColor(xObjectGroup.Attribute("color"));
            Opacity = (double?) xObjectGroup.Attribute("opacity") ?? 1.0;
            Visible = (bool?) xObjectGroup.Attribute("visible") ?? true;

            Objects = new TmxList<TmxObject>();
            foreach (XElement e in xObjectGroup.Elements("object"))
                Objects.Add(new TmxObject(e));

            Properties = new PropertyDict(xObjectGroup.Element("properties"));
        }

        public TmxColor Color { get; private set; }
        public double Opacity { get; private set; }
        public bool Visible { get; private set; }

        public TmxList<TmxObject> Objects { get; private set; }
        public PropertyDict Properties { get; private set; }
        public string Name { get; private set; }

        public class TmxObject : ITmxElement
        {
            // Many TmxObjectTypes are distinguished by null values in fields
            // It might be smart to subclass TmxObject
            public TmxObject(XElement xObject)
            {
                Name = (string) xObject.Attribute("name") ?? "";
                Type = (string) xObject.Attribute("type");
                X = (int) xObject.Attribute("x");
                Y = (int) xObject.Attribute("y");
                Visible = (bool?) xObject.Attribute("visible") ?? true;
                Width = (int?) xObject.Attribute("width") ?? 0;
                Height = (int?) xObject.Attribute("height") ?? 0;
                Rotation = (double?) xObject.Attribute("rotation") ?? 0.0;

                // Assess object type and assign appropriate content
                XAttribute xGid = xObject.Attribute("gid");
                XElement xEllipse = xObject.Element("ellipse");
                XElement xPolygon = xObject.Element("polygon");
                XElement xPolyline = xObject.Element("polyline");

                if (xGid != null)
                {
                    Tile = new TmxLayerTile((uint) xGid, X, Y);
                    ObjectType = TmxObjectType.Tile;
                }
                else if (xEllipse != null)
                {
                    ObjectType = TmxObjectType.Ellipse;
                }
                else if (xPolygon != null)
                {
                    Points = ParsePoints(xPolygon);
                    ObjectType = TmxObjectType.Polygon;
                }
                else if (xPolyline != null)
                {
                    Points = ParsePoints(xPolyline);
                    ObjectType = TmxObjectType.Polyline;
                }
                else ObjectType = TmxObjectType.Basic;

                Properties = new PropertyDict(xObject.Element("properties"));
            }

            public TmxObjectType ObjectType { get; private set; }
            public string Type { get; private set; }
            public int X { get; private set; }
            public int Y { get; private set; }
            public int Width { get; private set; }
            public int Height { get; private set; }
            public double Rotation { get; private set; }
            public TmxLayerTile Tile { get; private set; }
            public bool Visible { get; private set; }

            public List<Tuple<int, int>> Points { get; private set; }
            public PropertyDict Properties { get; private set; }
            public string Name { get; private set; }

            public List<Tuple<int, int>> ParsePoints(XElement xPoints)
            {
                var points = new List<Tuple<int, int>>();

                var pointString = (string) xPoints.Attribute("points");
                string[] pointStringPair = pointString.Split(' ');
                foreach (string s in pointStringPair)
                {
                    string[] pt = s.Split(',');
                    int x = int.Parse(pt[0]);
                    int y = int.Parse(pt[1]);
                    points.Add(Tuple.Create(x, y));
                }
                return points;
            }
        }
    }
}