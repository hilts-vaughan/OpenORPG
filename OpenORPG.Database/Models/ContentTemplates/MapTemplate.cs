﻿namespace Server.Game.Database.Models.ContentTemplates
{
    /// <summary>
    ///     A map template contains information about a particular map and zone in the game world.
    /// </summary>
    public class MapTemplate : IContentTemplate
    {
        public MapTemplate(int id, string name, string virtualCategory)
        {
            Id = id;
            Name = name;
            VirtualCategory = virtualCategory;
        }


        public int Id { get; set; }
        public string Name { get; set; }
        public string VirtualCategory { get; set; }
    }
}