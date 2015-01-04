using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OpenORPG.Database.Enums;
using OpenORPG.Database.Models;
using Server.Game.Combat;
using Server.Game.Database;
using Server.Game.Database.Models;
using Server.Game.Items;
using Server.Game.Items.Equipment;
using Server.Game.Network.Packets.Server;
using Server.Game.Quests;
using Server.Game.Storage;
using Server.Infrastructure.Logging;
using Server.Infrastructure.Quests;
using Server.Infrastructure.World;

namespace Server.Game.Entities
{
    public delegate void SkillEvent(Skill skill, Player player);

    public delegate void EquipmentEvent(Equipment equipment, Player player, EquipmentSlot slot);

    public delegate void PlayerEvent(Player player);

    public delegate void ValueChanged(int newValue, int oldValue, Player player);

    public class Player : Character
    {


        public event EquipmentEvent EquipmentChanged;
        public event SkillEvent LearnedSkill;

        public event PlayerEvent AcceptedQuest;

        protected virtual void OnAcceptedQuest(Player player)
        {
            PlayerEvent handler = AcceptedQuest;
            if (handler != null) handler(player);
        }


        public event PlayerEvent BackpackChanged;

        protected virtual void OnBackpackChanged(Player player)
        {
            PlayerEvent handler = BackpackChanged;
            if (handler != null) handler(player);
        }

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
        /// Given a key, retrieves that value from a users set of keys. If the value cannot be found, an empty string will be returned instead.
        /// </summary>
        /// <param name="key">The key to search for within the collection of flags</param>
        /// <returns></returns>
        public string GetFlagKey(string key)
        {
            var flags = _hero.Flags;
            var flag = flags.FirstOrDefault(x => x.Key == key);

            if (flag != null)
                return flag.Value;
            return "";
        }

        /// <summary>
        /// Given a key and value, updates the value of the player state. If the value cannot be found,
        /// it will be created and then value will be set to the specified parameter.
        /// </summary>
        /// <param name="key">The key for the flag to be set</param>
        /// <param name="value">The value for the flag to be set</param>
        /// <returns></returns>
        public void SetFlagKey(string key, string value)
        {
            var flags = _hero.Flags;
            var flag = flags.FirstOrDefault(x => x.Key == key);

            if (flag != null)
                flag.Value = value;
            else            
                flags.Add(new UserFlag() {Key = key, Value = value});                                      
        }

        /// <summary>
        /// A read-only collection of all the user flags. Do not attempt to modify this directly.
        /// </summary>
        public ICollection<UserFlag> Flags { get { return _hero.Flags; } }


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

            // Allow extraction of the quest information
            QuestLog = new QuestLog(userHero.QuestInfo);

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




                foreach (var eq in userHero.Equipment)
                {
                    var itemTemplate = context.ItemTemplates.First(x => x.Id == eq.ItemId);
                    var equipment = ItemFactory.CreateItem(itemTemplate) as Equipment;
                    Equipment[(int)equipment.Slot] = equipment;
                }



                UserId = userHero.UserHeroId;
                Experience = userHero.Experience;
                Level = userHero.Level;

                Backpack.StorageChanged += BackpackOnStorageChanged;
                QuestLog.QuestAccepted += QuestLogOnQuestAccepted;
            }


            // Store the user hero internally
            _hero = userHero;
        }

        private void QuestLogOnQuestAccepted(QuestLogEntry entry)
        {
            OnAcceptedQuest(this);
            entry.Quest.Script.OnQuestStarted(this);
        }


        private void BackpackOnStorageChanged(object sender, EventArgs eventArgs)
        {
            OnBackpackChanged(this);
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
        public ItemStorage Bank { get; private set; }

        public QuestLog QuestLog { get; private set; }

        public void PerformLevelUp()
        {
            Level++;
            Experience = 0;
        }


        public void AddSkill(Skill skill)
        {
            Skills.Add(skill);

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

                // If there's something in that slot already, remove it add it
                if (Equipment[(int)itemInInventory.Slot] != null)
                    Backpack.TryAddItem((Equipment[(int)itemInInventory.Slot]));

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
                var success = Backpack.TryAddItem((Equipment[(int)slot]));


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
