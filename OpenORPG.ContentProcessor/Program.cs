using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenORPG.ContentProcessor.Extractors;
using OpenORPG.ContentProcessor.Persistence;

namespace OpenORPG.ContentProcessor
{
    /// <summary>
    /// The main entry point for the extractor program, allows us to do some fancy stuff
    /// </summary>
    class Program
    {
        

        static void Main(string[] args) 
        {
            Console.Title = "Extracting data...";

            var directory = Environment.CurrentDirectory + "\\gamesfiles\\";

            if (Environment.GetCommandLineArgs().Length > 1)
                directory = Environment.GetCommandLineArgs()[1];


            Directory.CreateDirectory(directory);
            var persister = new JsonPersister(directory);

            // persist what we need to do
            new SkillExtractor().ProcessContent(persister);
            new MonsterExtractor().ProcessContent(persister);
            new ItemExtractor().ProcessContent(persister);
            new QuestExtractor().ProcessContent(persister);
            new DialogExtractor().ProcessContent(persister);


        }
    }
}
