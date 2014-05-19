using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Server.Game.Combat;
using Server.Game.Database;
using Server.Game.Database.Models;
using Server.Game.Items;
using Server.Game.Items.Equipment;
using Server.Game.Storage;
using Server.Infrastructure.Logging;
using Server.Infrastructure.Quests;
using Server.Infrastructure.World;

namespace Server.Game.Entities
{
    public delegate void QuestEvent(UserQuestInfo userQuestInfo, Player player);

    public delegate void EquipmentEvent(Equipment equipment, Player player, EquipmentSlot slot);


    public class Player : Character
    {

        public event QuestEvent AcceptedQuest;
        public event EquipmentEvent EquipmentChanged;

        protected virtual void OnEquipmentChanged(Equipment equipment, Player player, EquipmentSlot slot)
        {
            EquipmentEvent handler = EquipmentChanged;
            if (handler != null) handler(equipment, player, slot);
        }


        protected virtual void OnAcceptedQuest(UserQuestInfo userquestinfo, Player player)
        {
            QuestEvent handler = AcceptedQuest;
            if (handler != null) handler(userquestinfo, player);
        }

        public int Experience { get; set; }


        private UserHero _hero;

        public Player(string sprite, GameClient client, UserHero userHero)
            : base(sprite)
        {
            Client = client;
            Backpack = new ItemStorage();
            Bank = new ItemStorage();
            QuestInfo = new List<UserQuestInfo>();

            using (var context = new GameDatabaseContext())
            {
                // Add the skills this player knows
                foreach (var skillEntry in userHero.Skills)
                {
                    var skillTemplate = context.SkillTemplates.First(x => x.Id == skillEntry.SkillId);
                    var skill = new Skill(skillTemplate);
                    Skills.Add(skill);
                }

                // Add the inventory stuff...
                foreach (var inventoryItem in userHero.Inventory)
                {
                    var itemTemplate = context.ItemTemplates.First(x => x.Id == inventoryItem.ItemId);
                    var item = ItemFactory.CreateItem(itemTemplate);
                    Backpack.TryAddItemAt(item, inventoryItem.ItemAmount, inventoryItem.SlotId);
                }


                // Add the state of the quest world to the user
                foreach (var questEntry in userHero.QuestInfo)
                {
                    QuestInfo.Add(questEntry);
                }

                UserId = userHero.UserHeroId;
                Experience = userHero.Experience;
                Level = userHero.Level;

            }


            // Store the user hero internally
            _hero = userHero;
        }

        public long HomepointZoneId
        {
            get { return _hero.HomepointZoneId; }
            set { _hero.HomepointZoneId = value; }
        }

        public long HomepointZoneX
        {
            get { return _hero.HomepointZoneX; }
            set { _hero.HomepointZoneX = value; }
        }

        public long HomepointZoneY
        {
            get { return _hero.HomepointZoneY; }
            set { _hero.HomepointZoneY = value; }
        }


        /// <summary>   
        /// The client attached to this player. 
        /// </summary>
        [JsonIgnore]
        public GameClient Client { get; set; }


        /// <summary>
        /// The backpack this player is currently holding
        /// </summary>
        public ItemStorage Backpack { get; set; }

        /// <summary>
        /// A bank is a players storage, usually it can hold a lot more than the standard backpack.
        /// </summary>
        public ItemStorage Bank { get; set; }

        public List<UserQuestInfo> QuestInfo { get; set; }

        public void AddQuest(UserQuestInfo questInfo)
        {
            QuestInfo.Add(questInfo);
            OnAcceptedQuest(questInfo, this);
        }

        /// <summary>
        /// Attempts to equip the given item to the character.
        /// The item will be removed the users inventory.
        /// </summary>
        /// <param name="slotId">The slot in the inventory to equip the item from</param>
        /// <returns>If the operation is not possible, false is returned. Otherwise, returns true.</returns>
        public bool TryEquipItem(long slotId)
        {
            var itemInInventory = Backpack.GetItemInfoAt(slotId).Item as Equipment;


            if (itemInInventory != null)
            {
                // Remove the item from the backpack
                Backpack.RemoveItemAt(slotId);

                // Assign it onto the hero
                Equipment[(int)itemInInventory.Slot] = itemInInventory;

                Logger.Instance.Info("{0} has equipped a {1}.", Name, itemInInventory.Name);

                // Notify everyone who cares
                OnEquipmentChanged(itemInInventory, this, itemInInventory.Slot);

                return true;
            }
            else
                Logger.Instance.Warn("{0} tried to equip an item that they did not have.", Name);

            return false;
        }

        public void RemoveEquipment(EquipmentSlot slot)
        {
            if (Equipment[(int)slot] != null)
            {
                var success = Backpack.TryAddItem(Equipment[(int)slot]);

                if (success)
                {
                    Equipment[(int)slot] = null;

                    OnEquipmentChanged(null, this, slot);
                }
            }
        }




        public long UserId { get; set; }
    }
}
