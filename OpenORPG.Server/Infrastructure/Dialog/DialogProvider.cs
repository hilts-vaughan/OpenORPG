using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OpenORPG.Common.Dialog;
using OpenORPG.Database.Models.ContentTemplates;
using Server.Game.Entities;
using Server.Infrastructure.Logging;

namespace Server.Infrastructure.Dialog
{
    /// <summary>
    /// Provides dialog services for players and maintains an internal state table for them
    /// </summary>
    public class DialogProvider
    {
        public delegate void DialogProviderEvent(Player player, DialogNode newNode);

        public event DialogProviderEvent DialogNodeChanged;

        protected virtual void OnDialogNodeChanged(Player player, DialogNode newnode)
        {
            DialogProviderEvent handler = DialogNodeChanged;
            if (handler != null) handler(player, newnode);
        }

        /// <summary>
        /// The root node of this dialog provider
        /// </summary>
        private DialogNode _rootDialogNode;
        private DialogReceiver _receiver = new DialogReceiver();


        private Dictionary<Player, DialogNode> _playerDialogNodeStateTable = new Dictionary<Player, DialogNode>();

        private JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
        {
            PreserveReferencesHandling = PreserveReferencesHandling.Objects,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            TypeNameHandling = TypeNameHandling.Auto
        };


        public DialogProvider(DialogTemplate dialogTemplate)
        {
            // Deflate our root dialog node
            _rootDialogNode = JsonConvert.DeserializeObject<DialogNode>(dialogTemplate.JsonPayload, jsonSerializerSettings);
        }


        public DialogNode FollowLinkWithAndUpdate(Player player, int linkId)
        {
            var node = _playerDialogNodeStateTable[player];

            if (linkId > node.Links.Count)
                throw new ArgumentOutOfRangeException("linkId", "linkId must be within the range of the current node's link count.");

            var link = node.Links[linkId];

            // We should do nothing if the user decided to use a link that was invalid
            if (!link.IsAvailable(player))
            {
                Logger.Instance.Warn("{0} attempted to follow a link with ID {1} which they were not allowed to use.", player, linkId);
                return null;
            }

            // Perform actions as required
            PerformLinkActions(player, link);

            // Get and return the next available node for the given link
            _playerDialogNodeStateTable[player] = link.NextNode;

            OnDialogNodeChanged(player, link.NextNode);

            return link.NextNode;
        }


        private void PerformLinkActions(Player player, DialogLink link)
        {
            _receiver.BeginSession(player);

            try
            {
                link.DialogActions.ForEach(action => action.Execute(_receiver));
            }
            catch (Exception exception)
            {
                Logger.Instance.Warn("An action was attempted to be executed and fail. It was likely not implemented. Check for unimplemented actions.");
            }


            _receiver.EndSession();
        }

        /// <summary>
        /// Begins a transaction with a player, resetting state if it contained any.
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public DialogNode BeginDialogWith(Player player)
        {
            if (!_playerDialogNodeStateTable.ContainsKey(player))
                _playerDialogNodeStateTable.Add(player, _rootDialogNode);
            else
                ResetState(player);

            OnDialogNodeChanged(player, _rootDialogNode);

            return _playerDialogNodeStateTable[player];
        }

        /// <summary>
        /// Removes the state table for a player. This is useful for when a player leaves a zone and maintaining
        /// state about this is no longer useful.
        /// </summary>
        /// <param name="player"></param>
        public void RemoveState(Player player)
        {
            _playerDialogNodeStateTable.Remove(player);
        }

        /// <summary>
        /// Resets the state of a player to the root node.
        /// </summary>
        /// <param name="player"></param>
        private void ResetState(Player player)
        {
            _playerDialogNodeStateTable[player] = _rootDialogNode;
        }


    }
}
