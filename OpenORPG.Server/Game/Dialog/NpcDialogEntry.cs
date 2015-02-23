using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Game.Entities;
using Server.Infrastructure.Dialog;

namespace Server.Game.Dialog
{

    public class NpcDialogEntry
    {
        public Npc Npc { get; private set; }

        public DialogProvider Provider { get; private set; }

        public NpcDialogEntry(Npc npc, DialogProvider provider)
        {
            Npc = npc;
            Provider = provider;
        }
    }

    public class NpcDialogCollection : IEnumerable<DialogProvider>
    {
        private List<NpcDialogEntry> _entries = new List<NpcDialogEntry>();


        public DialogProvider FindProviderFor(Npc npc)
        {
            var entry = _entries.FirstOrDefault(e => e.Npc == npc);
            return entry != null ? entry.Provider : null;
        }

        public void InsertProviderFor(Npc npc, DialogProvider provider)
        {
            _entries.Add(new NpcDialogEntry(npc, provider));
        }


        public IEnumerator<DialogProvider> GetEnumerator()
        {
            foreach (var entry in _entries)
                yield return entry.Provider;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    }
}
