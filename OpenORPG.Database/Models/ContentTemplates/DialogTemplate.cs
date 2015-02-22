using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Game.Database.Models.ContentTemplates;

namespace OpenORPG.Database.Models.ContentTemplates
{
    /// <summary>
    /// This template is a special type of template, it uses JSON payloads to keep things simple.
    /// Dialog trees do not persist to a DBMS very well, so we use a special type of repository in this case to take care of things
    /// </summary>
    [Table("dialog_template")]
    public class DialogTemplate : IContentTemplate
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string VirtualCategory { get; set; }

        /// <summary>
        /// Represents a JSON'd payload of a DialogNode that will be loaded into memory
        /// </summary>
        public string JsonPayload { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
