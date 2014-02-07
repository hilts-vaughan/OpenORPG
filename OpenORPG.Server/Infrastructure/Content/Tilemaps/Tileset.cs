/* Distributed as part of TiledSharp, Copyright 2012 Marshall Ward
 * Licensed under the Apache License, Version 2.0
 * http://www.apache.org/licenses/LICENSE-2.0 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace Server.Infrastructure.Content.Tilemaps
{
    public class TmxTileset : TmxDocument, ITmxElement
    {
        public TmxTileset(XDocument xDoc, string tmxDir) :
            this(xDoc.Element("tileset"), tmxDir)
        {
        }

        // TMX tileset element constructor
        public TmxTileset(XElement xTileset, string tmxDir = "")
        {
            XAttribute xFirstGid = xTileset.Attribute("firstgid");
            var source = (string) xTileset.Attribute("source");

            if (source != null)
            {
                throw new NotSupportedException(
                    "External tilesets are not yet supported. Please move the tileset or implement this.");

                // This won't work just yet; we'll need to implement a seperate loader probably

                // Prepend the parent TMX directory if necessary
                source = Path.Combine(tmxDir, source);

                // source is always preceded by firstgid
                FirstGid = (int) xFirstGid;

                // Everything else is in the TSX file
                XDocument xDocTileset = ReadXml(new FileStream(source, FileMode.Open));
                var ts = new TmxTileset(xDocTileset, TmxDirectory);

                Name = ts.Name;
                TileWidth = ts.TileWidth;
                TileHeight = ts.TileHeight;
                Spacing = ts.Spacing;
                Margin = ts.Margin;
                TileOffset = ts.TileOffset;
                Image = ts.Image;
                Terrains = ts.Terrains;
                Tiles = ts.Tiles;
                Properties = ts.Properties;
            }
            else
            {
                // firstgid is always in TMX, but not TSX
                if (xFirstGid != null)
                    FirstGid = (int) xFirstGid;

                Name = (string) xTileset.Attribute("name");
                TileWidth = (int) xTileset.Attribute("tilewidth");
                TileHeight = (int) xTileset.Attribute("tileheight");
                Spacing = (int?) xTileset.Attribute("spacing") ?? 0;
                Margin = (int?) xTileset.Attribute("margin") ?? 0;

                TileOffset = new TmxTileOffset(xTileset.Element("tileoffset"));
                Image = new TmxImage(xTileset.Element("image"), tmxDir);

                Terrains = new TmxList<TmxTerrain>();
                XElement xTerrainType = xTileset.Element("terraintypes");
                if (xTerrainType != null)
                {
                    foreach (XElement e in xTerrainType.Elements("terrain"))
                        Terrains.Add(new TmxTerrain(e));
                }

                Tiles = new List<TmxTilesetTile>();
                foreach (XElement xTile in xTileset.Elements("tile"))
                {
                    var tile = new TmxTilesetTile(xTile, Terrains, tmxDir);
                    Tiles.Add(tile);
                }

                Properties = new PropertyDict(xTileset.Element("properties"));
            }
        }

        public int FirstGid { get; private set; }
        public int TileWidth { get; private set; }
        public int TileHeight { get; private set; }
        public int Spacing { get; private set; }
        public int Margin { get; private set; }

        public TmxTileOffset TileOffset { get; private set; }
        public TmxImage Image { get; private set; }
        public TmxList<TmxTerrain> Terrains { get; private set; }
        public List<TmxTilesetTile> Tiles { get; private set; }
        public PropertyDict Properties { get; private set; }
        public string Name { get; private set; }

        // TSX file constructor
    }

    public class TmxTileOffset
    {
        public TmxTileOffset(XElement xTileOffset)
        {
            if (xTileOffset == null)
            {
                X = 0;
                Y = 0;
            }
            else
            {
                X = (int) xTileOffset.Attribute("x");
                Y = (int) xTileOffset.Attribute("y");
            }
        }

        public int X { get; private set; }
        public int Y { get; private set; }
    }

    public class TmxTerrain : ITmxElement
    {
        public TmxTerrain(XElement xTerrain)
        {
            Name = (string) xTerrain.Attribute("name");
            Tile = (int) xTerrain.Attribute("tile");
            Properties = new PropertyDict(xTerrain.Element("properties"));
        }

        public int Tile { get; private set; }

        public PropertyDict Properties { get; private set; }
        public string Name { get; private set; }
    }

    public class TmxTilesetTile
    {
        public TmxTilesetTile(XElement xTile, TmxList<TmxTerrain> Terrains,
                              string tmxDir = "")
        {
            Id = (int) xTile.Attribute("id");

            TerrainEdges = new List<TmxTerrain>(4);

            int result;
            TmxTerrain edge;

            string strTerrain = (string) xTile.Attribute("terrain") ?? ",,,";
            foreach (string v in strTerrain.Split(','))
            {
                bool success = int.TryParse(v, out result);
                if (success)
                    edge = Terrains[result];
                else
                    edge = null;
                TerrainEdges.Add(edge);
            }

            Probability = (double?) xTile.Attribute("probability") ?? 1.0;
            Properties = new PropertyDict(xTile.Element("properties"));
        }

        public int Id { get; private set; }
        public List<TmxTerrain> TerrainEdges { get; private set; }
        public double Probability { get; private set; }

        public TmxImage Image { get; private set; }
        public PropertyDict Properties { get; private set; }

        // Human-readable aliases to the Terrain markers
        public TmxTerrain TopLeft
        {
            get { return TerrainEdges[0]; }
        }

        public TmxTerrain TopRight
        {
            get { return TerrainEdges[1]; }
        }

        public TmxTerrain BottomLeft
        {
            get { return TerrainEdges[2]; }
        }

        public TmxTerrain BottomRight
        {
            get { return TerrainEdges[3]; }
        }
    }
}