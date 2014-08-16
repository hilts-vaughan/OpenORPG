using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OpenORPG.Database.Enums;
using Server.Game.Combat;
using Server.Game.Database;
using Server.Game.Database.Models;
using Server.Game.Items;
using Server.Game.Items.Equipment;
using Server.Game.Network.Packets.Server;
using Server.Game.Storage;
using Server.Infrastructure.Logging;
using Server.Infrastructure.Quests;
using Server.Infrastructure.World;

namespace Server.Game.Entities
{
    public delegate void QuestEvent(UserQuestInfo userQuestInfo, Player player);

    public delegate void SkillEvent(Skill skill, Player player);

    public delegate void EquipmentEvent(Equipment equipment, Player player, EquipmentSlot slot);

    public delegate void ItemEvent(Item item, int amount, Player player);

    public delegate void ValueChanged(int newValue, int oldValue, Player player);

    public class Player : Character
    {

        public event QuestEvent AcceptedQuest;
        public event EquipmentEvent EquipmentChanged;
        public event SkillEvent LearnedSkill;
        public event ItemEvent BackpackChanged;
        public event ValueChanged ExperienceChanged;
        public event ValueChanged LevelChanged;

        protected virtual void OnLevelChanged(int newvalue, int oldvalue, Player player)
        {
            ValueChanged handler = LevelChanged;
            if (handler != null) handler(newvalue, oldvalue, player);
        }

        protected virtual void OnExperienceChanged(int newvalue, int oldvalue, Player player)
        {
            ValueChanged handler = ExperienceChanged;
            if (handler != null) handler(newvalue, oldvalue, player);
        }



        protected virtual void OnBackpackChanged(Item item, int amount, Player player)
        {
            ItemEvent handler = BackpackChanged;
            if (handler != null) handler(item, amount, player);
        }

        protected virtual void OnLearnedSkill(Skill skill, Player player)
        {
            SkillEvent handler = LearnedSkill;
            if (handler != null) handler(skill, player);
        }

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

        public int Experience
        {
            get { return _experience; }
            set
            {
                int oldValue = _experience;
                _experience = value;
                PropertyCollection.WriteValue("Experience", value);
                OnExperienceChanged(_experience, oldValue, this);
            }
        }

        public override int Level
        {
            get { return _level1; }
            set
            {
                int oldValue = _level1; _level1 = value; PropertyCollection.WriteValue("Level", value);
                OnLevelChanged(_level1, oldValue, this);
            }
        }

        /// <summary>
        /// The amount of currency this particular player is holding.
        /// This is used in computing
        /// </summary>
        public long Currency
        {
            get { return _currency; }
            set { _currency = value; PropertyCollection.WriteValue("Currency", value); }
        }


        private UserHero _hero;
        private long _currency;
        private int _experience;
        private int _level1;

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

                foreach (var eq in userHero.Equipment)
                {
                    var itemTemplate = context.ItemTemplates.First(x => x.Id == eq.ItemId);
                    var equipment = ItemFactory.CreateItem(itemTemplate) as Equipment;
                    Equipment[(int)equipment.Slot] = equipment;
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

        public void PerformLevelUp()
        {
            Level++;
            Experience = 0;
        }

        public void AddQuest(UserQuestInfo questInfo)
        {
            QuestInfo.Add(questInfo);
            OnAcceptedQuest(questInfo, this);
        }

        public void AddSkill(Skill skill)
        {
            Skills.Add(skill);

        }

        /// <summary>
        /// Attempts to add an item to the players backpack
        /// </summary>
        /// <param name="item">Attempts to add the given time</param>
        /// <param name="amount">The amount of the item to give</param>
        public bool AddToBackpack(Item item, int amount)
        {
            var r = Backpack.TryAddItem(item, amount);
            OnBackpackChanged(item, amount, this);
            return r;
        }

        public void RemoveFromBackpack(int slotId, int amount)
        {
            var item = Backpack.GetItemInfoAt(slotId);

            for (int i = 0; i < amount; i++)
                Backpack.RemoveSingleAt(slotId);

            OnBackpackChanged(item.Item, amount, this);
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
                RemoveFromBackpack((int)slotId, 1);

                // If there's something in that slot already, remove it add it
                if (Equipment[(int)itemInInventory.Slot] != null)
                    AddToBackpack(Equipment[(int)itemInInventory.Slot], 1);

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
                var success = AddToBackpack(Equipment[(int)slot], 1);

                if (success)
                {
                    Equipment[(int)slot] = null;
                    Logger.Instance.Info("{0} has remove an item from the slot {1}.", Name, slot.ToString());
                    OnEquipmentChanged(null, this, slot);
                }
            }
        }




        public long UserId { get; set; }
    }
}
