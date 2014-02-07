// Distributed as part of TiledSharp, Copyright 2012 Marshall Ward
// Licensed under the Apache License, Version 2.0
// http://www.apache.org/licenses/LICENSE-2.0

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Ionic.Zlib;
using CompressionMode = System.IO.Compression.CompressionMode;
using GZipStream = System.IO.Compression.GZipStream;

namespace Server.Infrastructure.Content.Tilemaps
{
    public class TmxDocument
    {
        public string TmxDirectory { get; private set; }

        protected XDocument ReadXml(Stream stream)
        {
            XDocument xDoc;

            xDoc = XDocument.Load(stream);

            return xDoc;
        }
    }

    public interface ITmxElement
    {
        string Name { get; }
    }

    public class TmxList<T> : KeyedCollection<string, T> where T : ITmxElement
    {
        public static Dictionary<Tuple<TmxList<T>, string>, int> nameCount
            = new Dictionary<Tuple<TmxList<T>, string>, int>();

        public new void Add(T t)
        {
            // Rename duplicate entries by appending a number
            Tuple<TmxList<T>, string> key = Tuple.Create(this, t.Name);
            if (Contains(t.Name))
                nameCount[key] += 1;
            else
                nameCount.Add(key, 0);
            base.Add(t);
        }

        protected override string GetKeyForItem(T t)
        {
            Tuple<TmxList<T>, string> key = Tuple.Create(this, t.Name);
            int count = nameCount[key];

            int dupes = 0;
            string itemKey = t.Name;

            // For duplicate keys, append a counter
            // For pathological cases, insert underscores to ensure uniqueness
            while (Contains(itemKey))
            {
                itemKey = t.Name + String.Concat(Enumerable.Repeat("_", dupes))
                          + count;
                dupes++;
            }

            return itemKey;
        }
    }

    public class PropertyDict : Dictionary<string, string>
    {
        public PropertyDict(XElement xmlProp)
        {
            if (xmlProp == null) return;

            foreach (XElement p in xmlProp.Elements("property"))
            {
                string pname = p.Attribute("name").Value;
                string pval = p.Attribute("value").Value;
                Add(pname, pval);
            }
        }
    }

    public class TmxImage
    {
        public TmxImage(XElement xImage, string tmxDir = "")
        {
            if (xImage == null) return;

            XAttribute xSource = xImage.Attribute("source");

            if (xSource != null)
                // Append directory if present
                Source = Path.Combine(tmxDir, (string) xSource);
            else
            {
                Format = (string) xImage.Attribute("format");
                XElement xData = xImage.Element("data");
                var decodedStream = new TmxBase64Data(xData);
                Data = decodedStream.Data;
            }

            Trans = new TmxColor(xImage.Attribute("trans"));
            Width = (int?) xImage.Attribute("width");
            Height = (int?) xImage.Attribute("height");
        }

        public string Format { get; private set; }
        public string Source { get; private set; }
        public Stream Data { get; private set; }
        public TmxColor Trans { get; private set; }
        public int? Width { get; private set; }
        public int? Height { get; private set; }
    }

    public class TmxColor
    {
        public int B;
        public int G;
        public int R;

        public TmxColor(XAttribute xColor)
        {
            if (xColor == null) return;

            string colorStr = ((string) xColor).TrimStart("#".ToCharArray());

            R = int.Parse(colorStr.Substring(0, 2), NumberStyles.HexNumber);
            G = int.Parse(colorStr.Substring(2, 2), NumberStyles.HexNumber);
            B = int.Parse(colorStr.Substring(4, 2), NumberStyles.HexNumber);
        }
    }

    public class TmxBase64Data
    {
        public TmxBase64Data(XElement xData)
        {
            if ((string) xData.Attribute("encoding") != "base64")
                throw new Exception(
                    "TmxBase64Data: Only Base64-encoded data is supported.");

            byte[] rawData = Convert.FromBase64String(xData.Value);
            Data = new MemoryStream(rawData, false);

            var compression = (string) xData.Attribute("compression");
            if (compression == "gzip")
                Data = new GZipStream(Data, CompressionMode.Decompress, false);
            else if (compression == "zlib")
                Data = new ZlibStream(Data,
                                      Ionic.Zlib.CompressionMode.Decompress, false);
            else if (compression != null)
                throw new Exception("TmxBase64Data: Unknown compression.");
        }

        public Stream Data { get; private set; }
    }
}