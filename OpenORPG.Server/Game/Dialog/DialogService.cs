using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Game.Entities;
using Server.Game.Network.Packets.Server;
using Server.Game.Zones;
using Server.Infrastructure.Dialog;
using Server.Infrastructure.Logging;
using Server.Infrastructure.World;
using Server.Infrastructure.World.Systems;
using ServiceStack;

namespace Server.Game.Dialog
{
    public class DialogService : GameSystem
    {
        /// <summary>
        /// Provides lookups for dialog providers where available
        /// </summary>
        Dictionary<Player, NpcDialogCollection> _dialogProviders = new Dictionary<Player, NpcDialogCollection>();

        public DialogService(Zone world)
            : base(world)
        {
        }

        public override void Update(float frameTime)
        {
        }

        /// <summary>
        /// Given an NPC and player, begins a dialog between the two.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="npc"></param>
        public void BeginDialog(Player player, Npc npc)
        {
            var npcDialog = npc.DialogTemplate;

            DialogProvider dialogProvider;

            if (_dialogProviders[player].FindProviderFor(npc) == null)
            {

                //TODO: Frequent IO hit; bad
                dialogProvider = new DialogProvider(npcDialog);

                // Insert the provider
                _dialogProviders[player].InsertProviderFor(npc, dialogProvider);

                dialogProvider.DialogNodeChanged -= DialogProviderOnDialogNodeChanged;
                dialogProvider.DialogNodeChanged += DialogProviderOnDialogNodeChanged;

            }
            else
            {
                dialogProvider = _dialogProviders[player].FindProviderFor(npc);
            }

            // Begin the dialog
            dialogProvider.BeginDialogWith(player);

        }

        public void AdvanceDialog(Player player, Npc npc, int linkId)
        {
            // Advance the dialog, which will fire an update to the client anyway
            var dialogProvider = _dialogProviders[player].FindProviderFor(npc);
            dialogProvider.FollowLinkWithAndUpdate(player, linkId);
        }

        // Use this for sending notifications to client
        private void DialogProviderOnDialogNodeChanged(Player player, DialogNode newNode)
        {
            Logger.Instance.Info("{0} is advancing dialog.", player);

            string message = "The conversation ends.";
            ICollection<string> links = new Collection<string>();
 
            if (newNode != null)
            {
                message = newNode.Text;
                links = newNode.Links.Where(x => x.IsAvailable(player)).Select(x => x.Text).ToList();
            }

            var packet = new ServerDialogPresentPacket(message, links);
            player.Client.Send(packet);
        }

        private void OnPlayerRemoved(Player player)
        {
            // Remove the key if it exists
            if (_dialogProviders.ContainsKey(player))
            {
                foreach (var provider in _dialogProviders[player])
                    provider.RemoveState(player);
            }

            _dialogProviders.Remove(player);
        }


        private void OnPlayerAdded(Player player)
        {
            _dialogProviders.Add(player, new NpcDialogCollection());
        }


        public override void OnEntityAdded(Entity entity)
        {
            if (entity is Player)
                OnPlayerAdded(entity as Player);
        }

        public override void OnEntityRemoved(Entity entity)
        {
            if (entity is Player)
                OnPlayerRemoved(entity as Player);
        }




    }
}
